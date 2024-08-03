﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Smartstore.Web.TagHelpers.Admin
{
    /// <summary>
    /// The base implementation for AI Tag Helpers.
    /// </summary>
    public class AITagHelperBase : BaseFormTagHelper
    {
        const string EntityIdAttributeName = "entity-id";
        const string EntityNameAttributeName = "entity-name";
        const string EntityTypeAttributeName = "entity-type";

        /// <summary>
        /// Used to determine which prompt should be used to create the text.
        /// </summary>
        [HtmlAttributeName(EntityTypeAttributeName)]
        public string EntityType { get; set; }

        /// <summary>
        /// Used to obtain an entity from the database when needed.
        /// </summary>
        [HtmlAttributeName(EntityIdAttributeName)]
        public string EntityId { get; set; }

        /// <summary>
        /// Used to be passed to the AI provider to describe the text about to be created.
        /// </summary>
        [HtmlAttributeName(EntityNameAttributeName)]
        public string EntityName { get; set; }

        protected virtual AttributeDictionary GetTagHelperAttributes()
        {
            var attributes = new AttributeDictionary
            {
                // INFO: We can't just use For.Name here, because the target property might be a nested property.
                //["data-target-property"] = For.Name,
                ["data-target-property"] = GetHtmlId(),
                ["data-entity-name"] = EntityName,
                ["data-entity-type"] = EntityType,
                ["data-entity-id"] = EntityId
            };

            return attributes;
        }

        protected virtual string GetHtmlId()
        {
            var fullname = HtmlHelper.Name(For.Name);
            return HtmlHelper.GenerateIdFromName(fullname);
        }
    }
}