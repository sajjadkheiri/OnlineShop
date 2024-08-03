﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Smartstore.Web.Rendering;

namespace Smartstore.Web.TagHelpers.Admin
{
    /// <summary>
    /// Renders a button or dropdown (depending on the number of active AI providers) to open a dialog for text suggestions.
    /// </summary>
    [HtmlTargetElement("ai-suggestion", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AISuggestionTagHelper(AIToolHtmlGenerator aiToolHtmlGenerator) : AITagHelperBase()
    {
        const string MandatoryEntityFieldsAttributeName = "mandatory-entity-fields";

        private readonly AIToolHtmlGenerator _aiToolHtmlGenerator = aiToolHtmlGenerator;

        /// <summary>
        /// List of comma separated mandatory fields of the target entity.
        /// Used to fill them with a placeholder after the suggestion has been accepted.
        /// Thus the entity can be saved directly after a suggestion has been accepted.
        /// </summary>
        [HtmlAttributeName(MandatoryEntityFieldsAttributeName)]
        public string MandatoryEntityFields { get; set; }

        protected override void ProcessCore(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = null;

            var attributes = GetTagHelperAttributes();
            var tool = _aiToolHtmlGenerator.GenerateSuggestionTool(attributes);
            if (tool == null)
            {
                return;
            }

            output.WrapContentWith(tool);
        }

        protected override AttributeDictionary GetTagHelperAttributes()
        {
            var attrs = base.GetTagHelperAttributes();
            attrs["data-mandatory-entity-fields"] = MandatoryEntityFields;
            return attrs;
        }
    }
}