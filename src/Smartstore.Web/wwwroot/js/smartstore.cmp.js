/*
*  Project: Consent Management Platform (CMP)
*  Author: Michael Herzog, SmartStore AG
*  Description: This script provides a Consent Management Platform (CMP) that allows scripts to be loaded and executed only after the user has given his consent.
*/

"use strict";

function ConsentManagementPlatform() {
    // Private methods

    // Scripts that require consent are included with the data-src attribute if no consent is given yet.
    // If consent is given, we replace the data-src attribute with the src attribute.
    function loadScriptFromUrl(script) {
        const src = script.getAttribute('data-src');
        if (src) {
            // Load script now.
            script.src = src;
            script.removeAttribute('data-consent-required');
            script.removeAttribute('data-src');
        }
    }
    // We assume scripts that require specific consent are included with the type attribute text/plain.
    // Thus they won't be executed until we inject them into the DOM.
    // If consent is given, we replace the script tag with a new script tag that contains the actual script code.
    function loadInlineScript(script) {
        var newScript = document.createElement('script');
        newScript.innerHTML = script.innerHTML;
        script.parentNode.replaceChild(newScript, script);
        //document.body.appendChild(newScript);
    }

    // Topics that depend on cookie consent before they can be loaded are wrapped in template tags. 
    // If consent is given, we unwrap the template tags with the actual content.
    function loadTemplateContent(template) {
        var clone = template.content.cloneNode(true);
        template.parentNode.replaceChild(clone, template);
    }

    // Returns the cookie value by name.
    function getCookie(name) {
        let match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? match[2] : null;
    }

    // Public properties
    this.CookieManagerDialog = null;
    this.Form = null;
    this.ConsentCookie = null;
    this.ConsentData = null;
    this.ConsentType = {
        Required: "required",
        Analytics: "analytics",
        ThirdParty: "thirdparty",
        ConsentAdUserData: "consentaduserdata",
        ConsentAdPersonalization: "consentadpersonalization"
    };

    // Public methods
    this.init = function () {
        const self = this;

        self.initCookieData();
        self.loadScripts();

        // Open consent dialog when user clicks on cookie manager link.
        $(document).on("click", ".cookie-manager", function (e) {
            e.preventDefault();
            self.showConsentDialog($(this).attr("href"));
        });
    }

    this.initCookieData = function () {
        this.ConsentCookie = getCookie('.Smart.CookieConsent');

        if (this.ConsentCookie) {
            // TODO: (mh) Error handling is missing.
            this.ConsentData = JSON.parse(decodeURIComponent(this.ConsentCookie));
        }
    }

    this.loadScripts = function () {
        for (let prop in this.ConsentType) {
            // Check to make sure the property is not from the prototype chain
            if (this.ConsentType.hasOwnProperty(prop)) {

                //console.log("Check for prop " + prop + ". Type:" + this.ConsentType[prop] + ". Result:" + this.checkConsent(this.ConsentType[prop]));

                // TODO: (mh) (perf) Instead of querying all scripts repetitively, we should query all scripts with data-consent attribute once and then filter them by data-consent attribute value.

                if (this.checkConsent(this.ConsentType[prop])) {
                    // Load scripts included via URL.
                    var scripts = document.querySelectorAll('script[data-consent="' + this.ConsentType[prop] + '"][data-src]');
                    scripts.forEach((script) => {
                        loadScriptFromUrl(script);
                    });

                    // Load scripts included via inline code.
                    var inlineScripts = document.querySelectorAll('script[data-consent="' + this.ConsentType[prop] + '"][type="text/plain"]');
                    inlineScripts.forEach((script) => {
                        loadInlineScript(script);
                    });

                    // Inject HTML from template tags into DOM.
                    var templates = document.querySelectorAll('template[data-consent="' + this.ConsentType[prop] + '"]');
                    templates.forEach((template) => {
                        loadTemplateContent(template);
                    });
                }
            }
        }
    }

    this.checkConsent = function (consentType) {
        // If cookie is not set, user has not given his consent. So nothing is allowed.
        if (!this.ConsentData) {
            return false; 
        }

        switch (consentType) {
            case "required":
                return true;
            case "analytics":
                return this.ConsentData.AllowAnalytics;
            case "thirdparty":
                return this.ConsentData.AllowThirdParty;
            case "consentaduserdata":
                return this.ConsentData.AdUserDataConsent;
            case "consentadpersonalization":
                return this.ConsentData.AdPersonalizationConsent;
            default:
                return false;        
        }
    }

    // Used by CookieManager Dialog to update the UI state of a checkbox.
    this.updateCheckboxUIState = function (elem) {
        const checked = elem.is(":checked");

        if (!checked) {
            elem.trigger("click");
        }
    }

    // Is called when user clicks on the "Save" button in the cookie manager dialog.
    this.onConsented = function () {
        const cmp = Smartstore.Cmp;

        // Update cookie data & load all scripts according to the new consent.
        cmp.initCookieData();
        cmp.loadScripts();

        cmp.hideConsentDialog();
    }

    // Is called when user clicks on a cookie manager link.
    this.showConsentDialog = function (dialogAjaxUrl) {
        var cmp = Smartstore.Cmp;
        var dialog = $("#cookie-manager-window");

        if (dialog.length > 0) {
            // Dialog was already loaded > just open dialog.
            initAndShowConsentDialog(dialog);
        }
        else {
            // Dialog wasn't loaded yet > get view via ajax call and append to body.
            $.ajax({
                cache: false,
                type: "POST",
                url: dialogAjaxUrl,
                data: {},
                success: function (data) {
                    $("body").append(data);
                    initAndShowConsentDialog($("#cookie-manager-window"));
                }
            });
        }
        function initAndShowConsentDialog(dialog) {
            cmp.CookieManagerDialog = dialog;
            cmp.Form = dialog.find("#cookie-manager-consent");
            cmp.CookieManagerDialog.modal('show');
        }
    }

    this.hideConsentDialog = function () {
        this.CookieManagerDialog.modal('hide');
    }

    this.init();
}

(function ($, window, document, undefined) {
    Smartstore.Cmp = new ConsentManagementPlatform();
} (jQuery, this, document));