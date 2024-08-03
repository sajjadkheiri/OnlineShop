﻿using System.Net;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Smartstore.Caching;
using Smartstore.Core.Common.Services;
using Smartstore.Core.Data;
using Smartstore.Core.Localization;
using Smartstore.Core.Web;
using Smartstore.Net;

namespace Smartstore.Core.Identity
{
    public partial class CookieConsentManager : ICookieConsentManager
    {
        private readonly static object _lock = new();
        private static IList<Type> _cookiePublisherTypes = null;

        const string CookieConsentKey = "cookieconsent";

        private readonly SmartDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private readonly ITypeScanner _typeScanner;
        private readonly PrivacySettings _privacySettings;
        private readonly IComponentContext _componentContext;
        private readonly IGeoCountryLookup _countryLookup;
        private readonly IRequestCache _requestCache;

        private bool? _isCookieConsentRequired;

        public CookieConsentManager(
            SmartDbContext db,
            IHttpContextAccessor httpContextAccessor,
            IWebHelper webHelper,
            ITypeScanner typeScanner,
            PrivacySettings privacySettings,
            IComponentContext componentContext,
            IGeoCountryLookup countryLookup,
            IRequestCache requestCache)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _webHelper = webHelper;
            _typeScanner = typeScanner;
            _privacySettings = privacySettings;
            _componentContext = componentContext;
            _countryLookup = countryLookup;
            _requestCache = requestCache;
        }

        public async Task<bool> IsCookieConsentRequiredAsync()
        {
            return _isCookieConsentRequired ??= await IsCookieConsentRequiredCoreAsync(_webHelper.GetClientIpAddress());
        }

        protected virtual async Task<bool> IsCookieConsentRequiredCoreAsync(IPAddress ipAddress)
        {
            if (_privacySettings.CookieConsentRequirement == CookieConsentRequirement.NeverRequired)
            {
                return false;
            }
            else 
            {
                var geoCountry = _countryLookup.LookupCountry(ipAddress);
                if (geoCountry != null)
                {
                    if (_privacySettings.CookieConsentRequirement == CookieConsentRequirement.DependsOnCountry)
                    {           
                        var country = await _db.Countries
                            .AsNoTracking()
                            .ApplyIsoCodeFilter(geoCountry.IsoCode)
                            .FirstOrDefaultAsync();

                        if (country != null && !country.DisplayCookieManager)
                        {
                            return true;
                        }
                    }
                    else if (_privacySettings.CookieConsentRequirement == CookieConsentRequirement.RequiredInEUCountriesOnly)
                    {
                        return geoCountry.IsInEu || geoCountry.IsoCode == "CH"; 
                    }
                }
            }
            
            return true;
        }

        public virtual async Task<IList<CookieInfo>> GetCookieInfosAsync(bool withUserCookies = false)
        {
            var result = new List<CookieInfo>();
            var publishers = GetAllCookiePublishers();

            foreach (var publisher in publishers)
            {
                var cookieInfo = await publisher.GetCookieInfosAsync();
                if (cookieInfo != null)
                {
                    result.AddRange(cookieInfo);
                }
            }

            // Add user defined cookies from privacy settings.
            if (withUserCookies)
            {
                result.AddRange(GetUserCookieInfos(true));
            }

            return result;
        }

        public virtual IReadOnlyList<CookieInfo> GetUserCookieInfos(bool translated = true)
        {
            if (_privacySettings.CookieInfos.HasValue())
            {
                var cookieInfos = JsonConvert.DeserializeObject<List<CookieInfo>>(_privacySettings.CookieInfos);

                if (cookieInfos != null && cookieInfos.Count > 0)
                {
                    if (translated)
                    {
                        foreach (var info in cookieInfos)
                        {
                            info.Name = info.GetLocalized(x => x.Name);
                            info.Description = info.GetLocalized(x => x.Description);
                        }
                    }

                    return cookieInfos;
                }
            }

            return new List<CookieInfo>();
        }

        public virtual async Task<bool> IsCookieAllowedAsync(CookieType cookieType)
        {
            Guard.NotNull(cookieType);
            
            if (!await IsCookieConsentRequiredAsync())
            {
                return true;
            }

            var consentCookie = _requestCache.Get(CookieConsentKey, () =>
            {
                var request = _httpContextAccessor?.HttpContext?.Request;
                if (request != null && request.Cookies.TryGetValue(CookieNames.CookieConsent, out var value) && value.HasValue())
                {
                    try
                    {
                        var consentCookie = JsonConvert.DeserializeObject<ConsentCookie>(value);

                        // If date is not set it's a cookie that was saved pre 5.2.0 and thus is HttpOnly.
                        // We must remove it and set a new one with HttpOnly = false because we need to read it in JS from 5.2.0 on.
                        if (consentCookie.ConsentedOn == null)
                        {
                            // TODO: (mh) Check whether it is ok to return the consent date. It was null before and felt wrong?!
                            consentCookie.ConsentedOn = DateTime.UtcNow;
                            SetConsentCookieCore(consentCookie);
                        }

                        return consentCookie;
                    }
                    catch
                    {
                        // TODO: (mh) No date. Is that ok?
                        // Let's be tolerant in case of error.
                        return new ConsentCookie 
                        {
                            AllowRequired = true,
                            AllowAnalytics = true,
                            AllowThirdParty = true,
                            AdPersonalizationConsent = true,
                            AdUserDataConsent = true
                        };
                    }
                }

                // TODO: (mh) No date. Is that ok?
                // There is no cookie consent cookie.
                return new ConsentCookie
                {
                    AllowRequired = false,
                    AllowAnalytics = false,
                    AllowThirdParty = false,
                    AdPersonalizationConsent = false,
                    AdUserDataConsent = false
                };
            });

            // Initialise allowedTypes with the CookieType.None which means no cookie is set yet and not even required cookies are allowed.
            CookieType allowedTypes = CookieType.None;

            if (consentCookie.AllowRequired) allowedTypes |= CookieType.Required;
            if (consentCookie.AllowAnalytics) allowedTypes |= CookieType.Analytics;
            if (consentCookie.AllowThirdParty) allowedTypes |= CookieType.ThirdParty;
            if (consentCookie.AdUserDataConsent) allowedTypes |= CookieType.ConsentAdUserData;
            if (consentCookie.AdPersonalizationConsent) allowedTypes |= CookieType.ConsentAdPersonalization;

            return allowedTypes.HasFlag(cookieType); 
        }

        public virtual ConsentCookie GetCookieData()
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context != null)
            {
                var cookieName = CookieNames.CookieConsent;

                if (context.Items.TryGetValue(cookieName, out var obj))
                {
                    return obj as ConsentCookie;
                }

                if (context.Request?.Cookies?.TryGetValue(cookieName, out string value) ?? false)
                {
                    try
                    {
                        var cookieData = JsonConvert.DeserializeObject<ConsentCookie>(value);
                        context.Items[cookieName] = cookieData;

                        return cookieData;
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }


        public virtual TagBuilder GenerateScript(bool consented, CookieType consentType, string src)
        {
            // TODO: (mh) Call this method where applicable (instead of the ugly string concat mess)
            Guard.NotEmpty(src);

            var script = new TagBuilder("script");
            if (consented)
            {
                script.Attributes["src"] = src;
            }
            else
            {
                script.Attributes["data-src"] = src;
                // TODO: (mh) Check casing/dasherization
                script.Attributes["data-consent"] = consentType.ToString().ToLowerInvariant();
            }

            return script;
        }

        public virtual TagBuilder GenerateInlineScript(bool consented, CookieType consentType, string code)
        {
            // TODO: (mh) Call this method where applicable (instead of the ugly string concat mess)
            Guard.NotEmpty(code);

            var script = new TagBuilder("script");
            script.InnerHtml.AppendHtml(code);

            if (!consented)
            {
                script.Attributes["type"] = "text/plain";
                // TODO: (mh) Check casing/dasherization
                script.Attributes["data-consent"] = consentType.ToString().ToLowerInvariant();
            }

            return script;
        }

        public void SetConsentCookie(
            bool allowRequired = false,
            bool allowAnalytics = false, 
            bool allowThirdParty = false,
            bool adUserDataConsent = false,
            bool adPersonalizationConsent = false)
        {
            var cookieData = new ConsentCookie
            {
                AllowRequired = allowRequired,
                AllowAnalytics = allowAnalytics,
                AllowThirdParty = allowThirdParty,
                AdUserDataConsent = adUserDataConsent,
                AdPersonalizationConsent = adPersonalizationConsent,
                ConsentedOn = DateTime.UtcNow
            };

            SetConsentCookieCore(cookieData);
        }

        protected virtual void SetConsentCookieCore(ConsentCookie cookieData)
        {
            Guard.NotNull(cookieData);
            
            var context = _httpContextAccessor?.HttpContext;
            if (context != null)
            {
                var cookies = context.Response.Cookies;
                var cookieName = CookieNames.CookieConsent;

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(365),
                    HttpOnly = false,
                    IsEssential = true,
                    Secure = _webHelper.IsCurrentConnectionSecured()
                };

                cookies.Delete(cookieName, options);
                cookies.Append(cookieName, JsonConvert.SerializeObject(cookieData), options);

                _requestCache.Remove(CookieConsentKey);
            }
        }

        protected virtual IEnumerable<ICookiePublisher> GetAllCookiePublishers()
        {
            if (_cookiePublisherTypes == null)
            {
                lock (_lock)
                {
                    if (_cookiePublisherTypes == null)
                    {
                        _cookiePublisherTypes = _typeScanner.FindTypes<ICookiePublisher>().ToList();
                    }
                }
            }

            var cookiePublishers = _cookiePublisherTypes
                .Select(type =>
                {
                    if (_componentContext.IsRegistered(type) && _componentContext.TryResolve(type, out var publisher))
                    {
                        return (ICookiePublisher)publisher;
                    }

                    return _componentContext.ResolveUnregistered(type) as ICookiePublisher;
                })
                .ToArray();

            return cookiePublishers;
        }
    }

    /// <summary>
    /// Infos that will be serialized and stored as string in a cookie.
    /// </summary>
    public class ConsentCookie
    {
        /// <summary>
        /// A value indicating whether required cookies are allowed to be set.
        /// </summary>
        public bool AllowRequired { get; set; }

        /// <summary>
        /// A value indicating whether analytical cookies are allowed to be set.
        /// </summary>
        public bool AllowAnalytics { get; set; }

        /// <summary>
        /// A value indicating whether third party cookies are allowed to be set.
        /// </summary>
        public bool AllowThirdParty { get; set; }

        /// <summary>
        /// A value indicating whether sending of user data is allowed.
        /// </summary>
        public bool AdUserDataConsent { get; set; }

        /// <summary>
        /// A value indicating whether personalization is allowed.
        /// </summary>
        public bool AdPersonalizationConsent { get; set; }

        /// <summary>
        /// A value indicating when the consent was given.
        /// </summary>
        public DateTime? ConsentedOn { get; set; } = null;
    }
}
