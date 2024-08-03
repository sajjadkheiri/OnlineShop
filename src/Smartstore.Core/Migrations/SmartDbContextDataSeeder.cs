using Smartstore.Core.Configuration;
using Smartstore.Data.Migrations;

namespace Smartstore.Core.Data.Migrations
{
    public class SmartDbContextDataSeeder : IDataSeeder<SmartDbContext>
    {
        public DataSeederStage Stage => DataSeederStage.Early;
        public bool AbortOnFailure => false;

        public async Task SeedAsync(SmartDbContext context, CancellationToken cancelToken = default)
        {
            await context.MigrateSettingsAsync(builder =>
            {
                builder.Delete("CustomerSettings.AvatarMaximumSizeBytes", "CatalogSettings.FileUploadMaximumSizeBytes");
            });

            await context.MigrateLocaleResourcesAsync(MigrateLocaleResources);
            await MigrateSettingsAsync(context, cancelToken);
        }

        public async Task MigrateSettingsAsync(SmartDbContext db, CancellationToken cancelToken = default)
        {
            // ContentSlider: Replace old service in templates.

            var contentSliderTemplate = new[]
            {
                // Template1
                @"\r\n<div class=""container h-100"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col-md-6 py-3 text-md-right text-center"">\r\n\t\t\t<h2 data-aos=""slide-right"" style=""--aos-delay: 600ms"">Slide-Title</h2>\r\n\t\t\t<p class=""d-none d-md-block"" data-aos=""slide-right"" style=""--aos-delay: 800ms"">Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.</p>\r\n\t\t</div>\r\n\t\t<div class=""col-md-6 picture-container"">\r\n\t\t\t<img alt="""" src=""https://picsum.photos/600/600"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n",
                // Template2
                @"\r\n<div class=""container h-100"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col-6 col-md-3 picture-container"">\r\n\t\t\t<img src=""https://picsum.photos/400/600"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-6 col-md-9 py-3"">\r\n\t\t\t<h2 data-aos=""slide-left"" style=""--aos-delay: 600ms"">Slide-Title</h2>\r\n\t\t\t<p data-aos=""slide-left"" style=""--aos-delay: 800ms"">Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</p>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n",
                // Template3
                @"\r\n<div class=""container h-100"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col-md-12 col-lg-6 picture-container"">\r\n\t\t\t<img alt="""" src=""https://picsum.photos/600/600"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-lg-6 d-none d-lg-block"">\r\n\t\t\t<h2 data-aos=""slide-left"" style=""--aos-delay: 600ms"">Slide-Title</h2>\r\n\t\t\t<p data-aos=""slide-left"" style=""--aos-delay: 800ms"">Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.</p>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n",
                // Template4
                @"\r\n<div class=""container h-100"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col-md-6 d-flex align-items-center justify-content-end"">\r\n\t\t\t<figure class=""picture-container vertical-align-middle"" data-aos=""zoom-in"" data-aos-easing=""ease-out-cubic"">\r\n\t\t\t\t<img src=""https://picsum.photos/300/300"" class=""img-fluid"" />\r\n\t\t\t</figure>\r\n\t\t</div>\r\n\t\t<div class=""col-md-6 d-flex align-items-center"">\r\n\t\t\t<div>\r\n\t\t\t\t<h2 data-aos=""slide-left"" style=""--aos-delay: 600ms"">Slide-Title</h2>\r\n\t\t\t\t<p data-aos=""slide-left"" style=""--aos-delay: 900ms"">Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est .</p>\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n",
                // Template5
                @"\r\n<div class=""container h-100"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col col-md-3 col-sm-6"" data-aos=""slide-down"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 1000ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/500/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 col-12 col-sm-6 d-none d-sm-block"" data-aos=""slide-down"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 1500ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/501/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 d-none d-md-block"" data-aos=""slide-down"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 2000ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/502/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 d-none d-md-block"" data-aos=""slide-down"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 2500ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/503/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n",
                // Template6
                @"\r\n<div class=""container"">\r\n\t<div class=""row h-100"">\r\n\t\t<div class=""col-md-3 hidden-sm-down"" data-aos=""fade-up"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 500ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/501/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 col-12 col-sm-6""  data-aos=""fade-right"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 1500ms"">\r\n\t\t\t<img class=""img-fluid"" src=""https://picsum.photos/300/500/"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 col-sm-6""  data-aos=""fade-left"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 1500ms"">\r\n\t\t\t<img class=""img-fluid"" src=""https://picsum.photos/300/500/"" />\r\n\t\t</div>\r\n\t\t<div class=""col-md-3 d-none d-md-block"" data-aos=""fade-up"" data-aos-easing=""ease-out-cubic"" style=""--aos-delay: 500ms"">\r\n\t\t\t<img src=""https://picsum.photos/300/501/"" class=""img-fluid"" />\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n"
            };

            for (var i = 0; i < 6; i++)
            {
                var templateName = $"ContentSlider.Template{i + 1}";
                var template = await db.Settings
                    .Where(x => x.Name == templateName)
                    .FirstOrDefaultAsync(cancelToken);

                if (template != null)
                {
                    db.Remove(template);
                    db.Settings.Add(new Setting
                    {
                        Name = templateName,
                        Value = contentSliderTemplate[i],
                        StoreId = 0
                    });
                }
            }

            await db.SaveChangesAsync(cancelToken);

            await db.Settings
                .Where(x => x.Name == "PaymentSettings.BypassPaymentMethodSelectionIfOnlyOne")
                .ExecuteUpdateAsync(x => x.SetProperty(s => s.Name, s => "PaymentSettings.SkipPaymentSelectionIfSingleOption"), cancelToken);
        }

        public void MigrateLocaleResources(LocaleResourcesBuilder builder)
        {
            builder.Delete(
                "Account.ChangePassword.Errors.PasswordIsNotProvided",
                "Common.Wait...",
                "Topic.Button",
                "Admin.Configuration.Settings.RewardPoints.Earning.Hint1",
                "Admin.Configuration.Settings.RewardPoints.Earning.Hint2",
                "Admin.Configuration.Settings.RewardPoints.Earning.Hint3",
                "ShoppingCart.MaximumUploadedFileSize");

            builder.AddOrUpdate("Admin.Report.MediaFilesSize", "Media size", "Mediengröße");
            builder.AddOrUpdate("Admin.Rules.FilterDescriptor.Affiliate", "Affiliate", "Partner");
            builder.AddOrUpdate("Admin.Rules.FilterDescriptor.Authentication", "Authentication", "Authentifizierung");

            builder.AddOrUpdate("Admin.Customers.RemoveAffiliateAssignment",
                "Remove assignment to affiliate",
                "Zuordnung zum Partner entfernen");

            builder.AddOrUpdate("Admin.Configuration.Settings.Order.MaxMessageOrderAgeInDays",
                "Maximum order age for sending messages",
                "Maximale Auftragsalter für den Nachrichtenversand",
                "Specifies the maximum order age in days up to which to create and send messages. Set to 0 to always send messages.",
                "Legt das maximale Auftragsalter in Tagen fest, bis zu dem Nachrichten erstellt und gesendet werden sollen. Setzen Sie diesen Wert auf 0, um Nachrichten immer zu versenden.");

            builder.AddOrUpdate("Admin.MessageTemplate.OrderTooOldForMessageInfo",
                "The message \"{0}\" was not sent. The order {1} is too old ({2}).",
                "Die Nachricht \"{0}\" wurde nicht gesendet. Der Auftrag {1} ist zu alt ({2}).");

            // Typo.
            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.ShowConfirmOrderLegalHint.Hint")
                .Value("de", "Legt fest, ob rechtliche Hinweise in der Warenkorbübersicht auf der Bestellabschlussseite angezeigt werden. Dieser Text kann in den Sprachresourcen geändert werden.");

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.UseNativeNameInLanguageSelector",
                "Display native language name in language selector",
                "In der Sprachauswahl den Sprachnamen in der Landesprache anzeigen",
                "Specifies whether the native language name should be displayed in the language selector. Otherwise, the name maintained in the backend is used.",
                "Legt fest, ob in der Sprachauswahl die Sprachnamen in der nativen Landesprache angezeigt werden soll. Ansonsten wird der im Backend hinterlegte Name verwendet.");

            builder.AddOrUpdate("Common.PageNotFound", "The page does not exist.", "Die Seite existiert nicht.");

            builder.AddOrUpdate("Admin.GiftCards.Fields.Language",
                "Language",
                "Sprache",
                "Specifies the language of the message content.",
                "Legt die Sprache des Nachrichteninhalts fest.");

            builder.AddOrUpdate("RewardPoints.OrderAmount", "Order amount", "Bestellwert");
            builder.AddOrUpdate("RewardPoints.PointsForPurchasesInfo",
                "For every {0} order amount {1} points are awarded.",
                "Für je {0} Auftragswert werden {1} Punkte gewährt.");

            builder.AddOrUpdate("Common.Error.BotsNotPermitted",
                "This process is not permitted for search engine queries (bots).",
                "Dieser Vorgang ist für Suchmaschinenanfragen (Bots) nicht zulässig.");

            // ----- Conditional attributes review (begin)
            builder.AddOrUpdate("Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.EditAttributeDetails",
                "Edit attribute. Product: {0}",
                "Attribut bearbeiten. Produkt: {0}");

            builder.AddOrUpdate("Admin.Catalog.Attributes.ProductAttributes.OptionsSetsInfo",
                "<strong>{0}</strong> option sets and <strong>{1}</strong> options",
                "<strong>{0}</strong> Options-Sets und <strong>{1}</strong> Optionen");

            builder.AddOrUpdate("Admin.Rules.ProductAttribute.OneCondition",
                "<span>Only show the attribute if at least</span> {0} <span>of the following rules are true.</span>",
                "<span>Das Attribut nur anzeigen, wenn mindestens</span> {0} <span>der folgenden Regeln zutrifft.</span>");

            builder.AddOrUpdate("Admin.Rules.ProductAttribute.AllConditions",
                "<span>Only show the attribute if</span> {0} <span>of the following rules are true.</span>",
                "<span>Das Attribut nur anzeigen, wenn</span> {0} <span>der folgenden Regeln erfüllt sind.</span>");


            builder.AddOrUpdate("Admin.Catalog.Products.ProductVariantAttributes.EditOptions",
                "Edit <strong>{0}</strong> options",
                "<strong>{0}</strong> Optionen bearbeiten");

            builder.AddOrUpdate("Admin.Catalog.Products.ProductVariantAttributes.EditRules",
                "Edit <strong>{0}</strong> rules",
                "<strong>{0}</strong> Regeln bearbeiten");

            builder.AddOrUpdate("Admin.Catalog.Products.ProductVariantAttributes.EditOptionsAndRules",
                "Edit <strong>{0}</strong> options and <strong>{1}</strong> rules",
                "<strong>{0}</strong> Optionen und <strong>{1}</strong> Regeln bearbeiten");


            builder.AddOrUpdate("Admin.Rules.AddRuleWarning", "Please add a rule first.", "Bitte zuerst eine Regel hinzufügen.");

            builder.AddOrUpdate("Admin.Rules.AddCondition", "Add rule", "Regel hinzufügen");
            builder.AddOrUpdate("Admin.Rules.AllConditions", 
                "<span>If</span> {0} <span>of the following rules are true.</span>", 
                "<span>Wenn</span> {0} <span>der folgenden Regeln erfüllt sind.</span>");

            builder.AddOrUpdate("Admin.Rules.OneCondition",
                "<span>If at least</span> {0} <span>of the following rules are true.</span>",
                "<span>Wenn mindestens</span> {0} <span>der folgenden Regeln zutrifft.</span>");

            builder.AddOrUpdate("Admin.Rules.SaveConditions", "Save all rules", "Alle Regeln speichern");
            builder.AddOrUpdate("Admin.Rules.SaveToCreateConditions",
                "Rules can only be created after saving.",
                "Regeln können erst nach einem Speichern festgelegt werden.");

            builder.AddOrUpdate("Admin.Rules.TestConditions").Value("de", "Regeln {0} Testen {1}");
            
            builder.AddOrUpdate("Admin.Rules.EditRuleSet", "Edit rule set", "Regelsatz bearbeiten");
            builder.AddOrUpdate("Admin.Rules.OpenRuleSet", "Open rule set", "Regelsatz öffnen");
            builder.Delete(
                "Admin.Rules.EditRule",
                "Admin.Rules.OpenRule",
                "Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.ViewLink");
            // ----- Conditional attributes review (end)

            // ----- Quick checkout (begin)
            builder.AddOrUpdate("Checkout.SpecifyDifferingShippingAddress",
                "I would like to specify a different delivery address after defining my billing address.",
                "Ich möchte nach der Festlegung meiner Rechnungsadresse eine abweichende Lieferanschrift festlegen.");

            builder.AddOrUpdate("Address.Fields.IsDefaultBillingAddress",
                "Set as default billing address",
                "Als Standard-Rechnungsanschrift festlegen");

            builder.AddOrUpdate("Address.Fields.IsDefaultShippingAddress",
                "Set as default shipping address",
                "Als Standard-Lieferanschrift festlegen");

            builder.AddOrUpdate("Address.IsDefaultAddress", "Is default address", "Ist Standardadresse");
            builder.AddOrUpdate("Address.IsDefaultBillingAddress", "Is default billing address", "Ist Standard-Rechnungsanschrift");
            builder.AddOrUpdate("Address.IsDefaultShippingAddress", "Is default shipping address", "Ist Standard-Lieferanschrift");

            builder.AddOrUpdate("Address.SetDefaultAddress",
                "Sets the address as the default billing and shipping address.",
                "Legt die Adresse als Standard-Rechnungs- und Lieferanschrift fest.");

            builder.AddOrUpdate("Account.Fields.PreferredShippingMethod", "Preferred shipping method", "Bevorzugte Versandart");
            builder.AddOrUpdate("Account.Fields.PreferredPaymentMethod", "Preferred payment method", "Bevorzugte Zahlungsart");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.QuickCheckoutEnabled",
                "Quick Checkout",
                "Quick-Checkout",
                "With Quick Checkout, settings from the customer's last order or default purchase settings (e.g. for the billing and shipping address) are applied"
                + " and the associated checkout steps are skipped. This allows the customer to go directly from the shopping cart to the order confirmation page.",
                "Beim Quick-Checkout werden Einstellungen der letzten Bestellung oder Kaufvoreinstellungen des Kunden angewendet (z.B. für die Rechnungs- und Lieferanschrift)"
                + " und die zugehörigen Checkout-Schritte übersprungen. Der Kunde hat so die Möglichkeit direkt vom Warenkorb zur Bestellbestätigungsseite zu gelangen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.CustomersCanChangePreferredShipping",
                "Customers can change their preferred shipping method",
                "Kunden können Ihre bevorzugte Versandart ändern");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.CustomersCanChangePreferredPayment",
                "Customers can change their preferred payment method",
                "Kunden können Ihre bevorzugte Zahlungsart ändern");

            builder.AddOrUpdate("Admin.Configuration.Settings.Payment.SkipPaymentSelectionIfSingleOption",
                "Only display payment method selection if more than one payment method is available",
                "Zahlartauswahl nur anzeigen, wenn mehr als eine Zahlart zur Verfügung steht",
                "Specifies whether the payment method selection in checkout is only displayed if more than one payment method is available.",
                "Legt fest, ob die Zahlartauswahl im Checkout nur angezeigt wird, wenn mehr als eine Zahlart zur Verfügung steht.");

            builder.AddOrUpdate("Checkout.Process.Standard",
                "Standard",
                "Standard",
                "The customer goes through all the necessary checkout steps.",
                "Der Kunde durchläuft alle erforderlichen Checkout-Schritte.");

            builder.AddOrUpdate("Checkout.Process.Terminal",
                "Terminal",
                "Terminal",
                "The customer is directly redirected to the confirmation page. Addresses, shipping and payment methods are skipped.",
                "Der Kunde gelangt direkt zur Bestätigungsseite. Anschriften, Versand- und Zahlart werden übersprungen.");

            builder.AddOrUpdate("Checkout.Process.Terminal.PaymentMethod",
                "Terminal with payment",
                "Terminal mit Zahlung",
                "The customer is redirected to the confirmation page via the payment method selection. Addresses and shipping methods are skipped.",
                "Der Kunde gelangt über die Zahlartauswahl zur Bestätigungsseite. Anschriften und Versandart werden übersprungen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.CheckoutProcess",
                "Checkout process",
                "Checkout-Prozess",
                "Specifies the type of checkout with the steps to be processed.",
                "Legt die Art des Checkout mit den zu durchlaufenden Schritten fest.");

            builder.AddOrUpdate("ShoppingCart.Products", "Products", "Artikel");
            builder.AddOrUpdate("ShoppingCart.EditCart", "Edit cart", "Warenkorb bearbeiten");
            builder.AddOrUpdate("ShoppingCart.Totals.ShippingWithinCountry", "Shipping within {0}", "Lieferung innerhalb {0}");

            builder.AddOrUpdate("ShoppingCart.NotAvailableVariant",
                "The selected product variant is not available.",
                "Die gewählte Produktvariante ist nicht verfügbar.");

            builder.AddOrUpdate("Checkout.ConfirmHint",
                "Please verify the order total and the specifics regarding the billing address and, if required, the shipping address. You can make corrections to your entry anytime by clicking on <strong>Change</strong>. If everything is as it should be, submit your order to us by clicking <strong>Confirm</strong>.",
                "Bitte prüfen Sie die Gesamtsumme und die Rechnungsadresse. Bei abweichender Lieferanschrift prüfen Sie bitte auch diese. Änderungen können Sie jederzeit mit einem Klick auf <strong>Ändern</strong> vornehmen. Sind alle Daten richtig, bestätigen Sie bitte mit einem Klick auf <strong>Kaufen</strong> Ihre Bestellung.");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.ShowSecondBuyButtonBelowCart",
                "Second buy-button below cart",
                "Zweiter Kauf-Button unterhalb des Warenkorbs",
                "Specifies whether to show a second buy button (including order total) below the cart items on the confirmation page. Recommended for stores in the EU for reasons of legal certainty.",
                "Legt fest, ob auf der Bestellabschlussseite ein zweiter Kauf-Button (einschließlich der Gesamtsumme der Bestellung) unterhalb der Warenkorbartikel angezeigt werden soll. Aus Gründen der Rechtssicherheit wird dies für Shops in der EU empfohlen.");

            builder.Delete(
                "Checkout.TermsOfService.PleaseAccept",
                "Checkout.TermsOfService.Read",
                "Checkout.TermsOfService",
                "Admin.Configuration.Settings.Order.TermsOfServiceEnabled",
                "Admin.Configuration.Settings.Order.TermsOfServiceEnabled.Hint");
            // ----- Quick checkout (end)

            builder.AddOrUpdate("Admin.Configuration.Settings.CustomerUser.MaxAvatarFileSize",
                "Maximum avatar size",
                "Maximale Avatar-Größe",
                "Specifies the maximum file size of an avatar (in KB). The default is 10,240 (10 MB).",
                "Legt die maximale Dateigröße eines Avatar in KB fest. Der Standardwert ist 10.240 (10 MB).");

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.ShowOnPasswordRecoveryPage",
                "Show on password recovery page",
                "Auf der Seite zur Passwort-Wiederherstellung anzeigen");

            builder.Delete("Admin.ContentManagement.Topics.Validation.NoWhiteSpace");
            
            builder.AddOrUpdate("Admin.Common.HtmlId.NoWhiteSpace",
                "Spaces are invalid for the HTML attribute 'id'.",
                "Leerzeichen sind für das HTML-Attribut 'id' ungültig.");

            builder.AddOrUpdate("CookieManager.Dialog.AdUserDataConsent.Heading",
                "Marketing",
                "Marketing");

            builder.AddOrUpdate("CookieManager.Dialog.AdUserDataConsent.Intro",
                "With your consent, our advertising partners can set cookies to create an interest profile for you so that we can offer you targeted advertising. For this purpose, we pass on an identifier unique to your customer account to these services.",
                "Unsere Werbepartner können mit Ihrer Einwilligung Cookies setzen, um ein Interessenprofil für Sie zu erstellen, damit wir Ihnen gezielt Werbung anbieten können. Dazu geben wir eine für Ihr Kundenkonto eindeutige Kennung an diese Dienste weiter. ");

            builder.AddOrUpdate("CookieManager.Dialog.AdPersonalizationConsent.Heading",
                "Personalization",
                "Personalisierung");

            builder.AddOrUpdate("CookieManager.Dialog.AdPersonalizationConsent.Intro",
                "Consent to personalisation enables us to offer enhanced functionality and personalisation. They can be set by us or by third-party providers whose services we use on our pages.",
                "Die Zustimmung zur Personalisierung ermöglicht es uns, erweiterte Funktionalität und Personalisierung anzubieten. Sie können von uns oder von Drittanbietern gesetzt werden, deren Dienste wir auf unseren Seiten nutzen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.ShowEssentialAttributesInMiniShoppingCart",
                "Show essential features in mini-shopping cart",
                "Wesentliche Merkmale im Mini-Warenkorb anzeigen");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.LinkManufacturerLogoInLists",
                "Link brand logo",
                "Marken-Logo verlinken");

            builder.AddOrUpdate("Common.OptimizeTableInfo",
                "The table '{0}' is already in an optimal state.",
                "Die Tabelle '{0}' ist bereits in einem optimalen Zustand.");

            builder.AddOrUpdate("Common.OptimizeTableSuccess",
                "The table '{0}' has been successfully optimized: {1} &rarr; {2}. Difference: {3} ({4}).",
                "Die Tabelle '{0}' wurde erfolgreich optimiert: {1} &rarr; {2}. Unterschied: {3} ({4}).");

            builder.AddOrUpdate("Admin.Configuration.Settings.ShoppingCart.AllowActivatableCartItems",
                "Products in shopping cart can be deactivated",
                "Produkte im Warenkorb können deaktiviert werden",
                "Specifies whether products in the shopping cart can be deactivated. Deactivated products will not be ordered and will remain in the shopping cart after the order has been placed.",
                "Legt fest, ob Produkte im Warenkorb deaktiviert werden können. Deaktivierte Produkte werden nicht mitbestellt und verbleiben nach Auftragseingang im Warenkorb.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.ShowProductTags", "Show tags", "Tags anzeigen");

            builder.AddOrUpdate("Common.SearchProducts", "Search products", "Produkte durchsuchen");
            builder.AddOrUpdate("Common.NoProductsFound", "No products were found.", "Es wurden keine Produkte gefunden.");

            // ----- Revamp grouped products (begin)
            builder.AddOrUpdate("Admin.Catalog.Products.GroupedProductConfiguration.SearchMinAssociatedCount",
                "Minimum product count for search",
                "Minimale Produktanzahl für Suche",
                "Specifies the minimum number of associated products from which the search field is displayed.",
                "Legt die Mindestanzahl verknüpfter Produkte fest, ab denen das Suchfeld angezeigt wird.");
            
            builder.AddOrUpdate("Admin.Catalog.Products.GroupedProductConfiguration.Collapsible",
                 "Collapsible associated products",
                 "Aufklappbare verknüpfte Produkte",
                 "Specifies whether details of the associated product are expanded/collapsed by clicking on a header (accordion).",
                 "Legt fest, ob Details zum verknüpften Produkt durch Klick auf eine Titelzeile auf- oder zugeklappt werden (Akkordeon).");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.AssociatedProductsPageSize",
                "Page size of associated products list",
                "Listengröße der verknüpften Produkten",
                "Specifies the number of associated products per page for grouped products.",
                "Legt die Anzahl verknüpfter Produkte pro Seite für Gruppenprodukte fest.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.SearchMinAssociatedProductsCount",
                "Minimum number of associated products for search",
                "Minimale Anzahl verknüpfter Produkte für Suche",
                "Specifies the minimum number of associated products from which a search field is displayed.",
                "Legt die Mindestanzahl verknüpfter Produkte fest, ab denen ein Suchfeld angezeigt wird.");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.CollapsibleAssociatedProducts",
                "Collapsible associated products",
                "Aufklappbare verknüpfte Produkte",
                "Specifies whether details of the associated product are expanded/collapsed by clicking on a header (accordion).",
                "Legt fest, ob Details zum verknüpften Produkt durch Klick auf eine Titelzeile auf- oder zugeklappt werden (Akkordeon).");

            builder.AddOrUpdate("Admin.Configuration.Settings.Catalog.CollapsibleAssociatedProductsHeaders",
                "Header fields of associated products",
                "Felder in der Titelzeile verknüpfter Produkte",
                "Specifies additional fields for the header of an associated product. The product name is always displayed.",
                "Legt zusätzliche Felder für die Titelzeile eines verknüpften Produktes fest. Der Produktname wird immer angezeigt.");

            builder.AddOrUpdate("Admin.Catalog.GroupedProductConfiguration.Note",
                "Settings for grouped products can be overwritten at product level.",
                "Einstellungen für Gruppenprodukte können beim jeweiligen Produkt überschrieben werden.");

            builder.AddOrUpdate("Admin.Catalog.GroupedProductConfiguration.SaveToContinue",
                "To edit, please first save the product as a grouped product.",
                "Zur Bearbeitung bitte zunächst das Produkt als Gruppenprodukt speichern.");

            builder.AddOrUpdate("Admin.Catalog.Products.GroupedProductConfiguration.Title", 
                "Title of associated products list", 
                "Listentitel der verknüpften Produkte");

            builder.AddOrUpdate("Admin.Catalog.Products.GroupedProductConfiguration", "Edit grouped product", "Gruppenprodukt bearbeiten");
            // ----- Revamp grouped products (end)

            builder.AddOrUpdate("Admin.Configuration.Settings.GeneralCommon.DisplayAdditionalLines",
                "Additional lines",
                "Zusätzliche Zeilen");

            builder.AddOrUpdate("Admin.Customers.Customers.Fields.IPAddress.Hint",
                "IP address of last visit",
                "IP-Adresse, mit der der Kunde zuletzt im Shop aktiv war.");

            builder.AddOrUpdate("Admin.DataExchange.Export.CompletedEmailAddresses", "Recipients e-mail addresses", "Empfänger E-Mail Adressen");
            builder.AddOrUpdate("Admin.DataExchange.Export.Deployment.EmailAddresses", "Recipients e-mail addresses", "Empfänger E-Mail Adressen");

            builder.AddOrUpdate("Admin.DataExchange.Export.FileNamePatternDescriptions",
                "ID of export profil;Folder name of export profil;SEO name of export profil;Store ID;SEO name of store;One based file index;Random number;UTC timestamp;Date and time",
                "ID des Exportprofils;Ordername des Exportprofils;SEO Name des Exportprofils;Shop ID;SEO Name des Shops;Mit 1 beginnender Dateiindex;Zufallszahl;UTC Zeitstempel;Datum und Uhrzeit");

            builder.AddOrUpdate("Admin.Orders.List.GoDirectlyToNumber",
                "Search by order number or order reference number",
                "Nach Auftrags- oder Bestellreferenznummer suchen");

            builder.AddOrUpdate("ShoppingCart.RequiredProductWarning",
                "This product requires that the following product is also ordered: <a href=\"{1}\" class=\"alert-link\">{0}</a>.",
                "Dieses Produkt erfordert, dass das folgende Produkt auch bestellt wird: <a href=\"{1}\" class=\"alert-link\">{0}</a>.");

            builder.AddOrUpdate("Admin.Orders.Shipments.Carrier",
                "Shipping is carried out by",
                "Versand erfolgt über",
                "Specifies the name of the carrier, e.g. DHL, Fedex, UPS or USPS.",
                "Legt den Namen des Transportunternehmens fest, z.B. DHL, Hermes, DPD oder UPS.");

            builder.AddOrUpdate("RewardPoints.Message.RewardPointsForProductReview",
                "You will receive <strong>{0}</strong> reward points worth <strong>{1}</strong> for your rating.",
                "Sie erhalten <strong>{0}</strong> Bonuspunkte im Wert von <strong>{1}</strong> für Ihre Bewertung.");

            builder.AddOrUpdate("RewardPoints.Message.RewardPointsForProductPurchase",
                "You will receive <strong>{0}</strong> reward points worth <strong>{1}</strong> for this purchase.",
                "Sie erhalten <strong>{0}</strong> Bonuspunkte im Wert von <strong>{1}</strong> für diesen Einkauf.");

            builder.AddOrUpdate("Admin.Configuration.Settings.RewardPoints.ShowPointsForProductReview",
                "Show points for a product review",
                "Punkte für eine Produkt-Rezension anzeigen",
                "Specifies whether the reward points awarded for a product review, including the corresponding amount, should be displayed on the product detail page.",
                "Legt fest, ob die für eine Produkt-Rezension gewährten Bonuspunkte samt dem entsprechenden Betrag auf der Produktdetailseite angezeigt werden sollen.");

            builder.AddOrUpdate("Admin.Configuration.Settings.RewardPoints.ShowPointsForProductPurchase",
                "Show points for the purchase of a product",
                "Punkte für den Kauf eines Produktes anzeigen",
                "Specifies whether the reward points awarded for purchasing a product, including the corresponding amount, should be displayed on the product detail page.",
                "Legt fest, ob die für den Kauf eines Produktes gewährten Bonuspunkte samt dem entsprechenden Betrag auf der Produktdetailseite angezeigt werden sollen.");

            AddAIResources(builder);
        }

        private static void AddAIResources(LocaleResourcesBuilder builder)
        {
            builder.AddOrUpdate("Admin.AI.CreateImageWith", "Create image with {0}", "Bild erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.CreateTextWith", "Create text with {0}", "Text erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.TranslateTextWith", "Translate with {0}", "Übersetzen mit {0}");
            builder.AddOrUpdate("Admin.AI.MakeSuggestionWith", "Make suggestions with {0}", "Mach mir Vorschläge mit {0}");

            builder.AddOrUpdate("Admin.AI.CreateShortDescWith", "Create short description with {0}", "Kurzbeschreibung erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.CreateMetaTitleWith", "Create title tag with {0}", "Title-Tag erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.CreateMetaDescWith", "Create meta description with {0}", "Meta Description erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.CreateMetaKeywordsWith", "Create meta keywords with {0}", "Meta Keywords erzeugen mit {0}");
            builder.AddOrUpdate("Admin.AI.CreateFullDescWith", "Create full description with {0}", "Langtext erzeugen mit {0}");

            builder.AddOrUpdate("Admin.AI.TextCreation.CreateNew", "Create new", "Neu erstellen");
            builder.AddOrUpdate("Admin.AI.TextCreation.Summarize", "Summarize", "Zusammenfassen");
            builder.AddOrUpdate("Admin.AI.TextCreation.Improve", "Improve", "Schreibstil verbessern");
            builder.AddOrUpdate("Admin.AI.TextCreation.Simplify", "Simplify", "Vereinfachen");
            builder.AddOrUpdate("Admin.AI.TextCreation.Extend", "Extend", "Ausführlicher schreiben");

            builder.AddOrUpdate("Admin.AI.TextCreation.DefaultPrompt", "Create text on the topic: '{0}'", "Erzeuge Text zum Thema: '{0}'.");
            builder.AddOrUpdate("Admin.AI.ImageCreation.DefaultPrompt", "Create a picture on the topic: '{0}'", "Erzeuge ein Bild zum Thema: '{0}'.");
            builder.AddOrUpdate("Admin.AI.Suggestions.DefaultPrompt", "Make suggestions on the topic: '{0}'", "Mache Vorschläge zum Thema '{0}'.");

            builder.AddOrUpdate("Admin.AI.MenuItemTitle.ChangeStyle", "Change style", "Sprachstil ändern");
            builder.AddOrUpdate("Admin.AI.MenuItemTitle.ChangeTone", "Change tone", "Ton ändern");

            builder.AddOrUpdate("Admin.AI.SpecifyTopic", "Please enter a topic", "Bitte geben Sie ein Thema an");

            builder.AddOrUpdate("Smartstore.AI.Prompts.DontUseQuotes",
                "Do not enclose the text in quotation marks.",
                "Schließe den Text nicht in Anführungszeichen ein.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.DontNumberSuggestions",
                "Do not number the suggestions.",
                "Nummeriere die Vorschläge nicht.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.SeparateWithNumberSign",
                "Separate each suggestion with the # sign.",
                "Trenne jeden Vorschlag mit dem #-Zeichen.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.CharLimit",
                "Limit your answer to {0} characters!",
                "Begrenze deine Antwort auf {0} Zeichen!");
            builder.AddOrUpdate("Smartstore.AI.Prompts.WordLimit",
                "The text may contain a maximum of {0} words.",
                "Der Text darf maximal {0} Wörter enthalten.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.SeparateListWithComma",
                "The list should be comma-separated so that it can be inserted directly as a meta tag.",
                "Die Liste soll kommagetrennt sein, so dass sie direkt als META-tag eingefügt werden kann.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.ReserveSpaceForShopName",
                "Do not use the name of the website as this will be added later. Reserve 5 words for this.",
                "Verwende dabei nicht den Namen der Website, da dieser später hinzugefügt wird. Reserviere dafür 5 Wörter.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.CreatePicture",
                "Create an image for the topic: '{0}'.",
                "Erstelle ein Bild zum Thema: '{0}'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.AddCallToAction",
                "Finally, insert a link with the text '{0}' that refers to '{1}'. The link is given the CSS classes 'btn btn-primary'",
                "Füge abschließend einen Link mit dem Text '{0}' ein, der auf '{1}' verweist. Der Link erhält die CSS-Klassen 'btn btn-primary'");
            builder.AddOrUpdate("Smartstore.AI.Prompts.AddLink",
                "Insert a link that refers to '{0}'.",
                "Füge einen Link ein, der auf '{0}' verweist.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.AddNamedLink",
                "Insert a link with the text '{0}' that refers to '{1}'.",
                "Füge einen Link mit dem Text '{0}' ein, der auf '{1}' verweist.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.AddToc",
                "Insert a table of contents with the title '{0}'." +
                " The title receives a {1} tag." +
                " Link the individual points of the table of contents to the respective headings of the paragraphs.",
                "Füge ein Inhaltsverzeichnis mit dem Titel '{0}' ein." +
                " Der Titel erhält ein {1}-Tag." +
                " Verlinke die einzelnen Punkte des Inhaltsverzeichnisses mit den jeweiligen Überschriften der Absätze.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.IncludeImages",
                "After each paragraph, add another p-tag with the style specification 'width:450px', which contains an i-tag with the classes 'far fa-xl fa-file-image ai-preview-file'." +
                " The title attribute of the i-tag should be the heading of the respective paragraph.",
                "Füge nach jedem Absatz ein weiteres p-Tag mit der style-Angabe 'width:450px' ein, das ein i-Tag mit den Klassen 'far fa-xl fa-file-image ai-preview-file' enthält." +
                " Das title-Attribut des i-Tags soll die Überschrift des jeweiligen Absatzes sein.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.NoIntroImage",
                "The intro does not receive a picture.",
                "Das Intro erhält kein Bild.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.NoConclusionImage",
                "The conclusion does not receive a picture.",
                "Das Fazit erhält kein Bild.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.UseKeywords",
                "Use the following keywords: '{0}'.",
                "Verwende folgende Keywords: '{0}'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.MakeKeywordsBold",
                "Include the keywords in b-tags.",
                "Schließe die Keywords in b-Tags ein.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.KeywordsToAvoid",
                "Do not use the following keywords under any circumstances: '{0}'.",
                "Verwende unter keinen Umständen folgende Keywords: '{0}'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.IncludeConclusion",
                "End the text with a conclusion.",
                "Schließe den Text mit einem Fazit ab.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.ParagraphHeadingTag",
                "The headings of the individual sections are given {0} tags.",
                "Die Überschriften der einzelnen Abschnitte erhalten {0}-Tags.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.WriteCompleteParagraphs",
                "Write complete texts for each section.",
                "Schreibe vollständige Texte für jeden Abschnitt.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.ParagraphWordCount",
                "Each section should contain a maximum of {0} words.",
                "Jeder Abschnitt soll maximal {0} Wörter enthalten.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.ParagraphCount",
                "The text should be divided into {0} paragraphs, which are enclosed in p tags.",
                "Der Text ist in {0} Abschnitte zu gliedern, die von p-Tags umschlossen sind.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.MainHeadingTag",
                "The main heading is given a {0} tag.",
                "Die Hauptüberschrift erhält ein {0}-Tag.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.IncludeIntro",
                "Start with an introduction.",
                "Beginne mit einer Einleitung.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.LanguageStyle",
                "The language style should be {0}.",
                "Der Sprachstil soll {0} sein.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.LanguageTone",
                "The tone should be {0}.",
                "Der Ton soll {0} sein.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Language",
                "Write in {0}.",
                "Schreibe in {0}.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.DontCreateTitle",
                "Do not create the title: '{0}'.",
                "Erstelle nicht den Titel: '{0}'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.StartWithDivTag",
                "Start with a div tag.",
                "Starte mit einem div-Tag.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.JustHtml",
                "Just return the HTML you have created so that it can be integrated directly into a website. " +
                "Don't give explanations about what you have created or introductions like: 'Gladly, here is your HTML'. " +
                "Do not include the generated HTML in any delimiters like: '```html'.",
                "Gib nur das erstellte HTML-zurück, so dass es direkt in einer Webseite eingebunden werden kann. " +
                "Mache keine Erklärungen dazu, was du erstellt hast oder Einleitungen wie: 'Gerne, hier ist dein HTML'. " +
                "Schließe das erzeugte HTML auch nicht in irgendwelche Begrenzer ein wie: '```html'.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.CreateHtml",
                "Create HTML text.",
                "Erstelle HTML-Text.");

            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Translator",
                "Be a professional translator.",
                "Sei ein professioneller Übersetzer.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Copywriter",
                "Be a professional copywriter.",
                "Sei ein professioneller Texter.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Marketer",
                "Be a marketing expert.",
                "Sei ein Marketing-Experte.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.SEOExpert",
                "Be a SEO expert.",
                "Sei ein SEO-Experte.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Blogger",
                "Be a professional blogger.",
                "Sei ein professioneller Blogger.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.Journalist",
                "Be a professional journalist.",
                "Sei ein professioneller Journalist.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.SalesPerson",
                "Be a assistant that creates product descriptions that convince a potential customer to make a purchase.",
                "Sei ein Assistent, der Produktbeschreibungen erstellt, die einen potentiellen Kunden von einem Kauf überzeugen.");
            builder.AddOrUpdate("Smartstore.AI.Prompts.Role.ProductExpert",
                "Be an expert for the product: '{0}'.",
                "Sei ein Experte für das Produkt: '{0}'.");
        }
    }
}