﻿namespace Smartstore.Core.Platform.AI.Prompting
{
    /// <summary>
    /// Interface to be implemented by all suggestion generation prompts.
    /// </summary>
    public interface ISuggestionPrompt
    {
        string TargetProperty { get; set; }
        string Input { get; set; }
        int NumberOfSuggestions { get; set; }
    }
}
