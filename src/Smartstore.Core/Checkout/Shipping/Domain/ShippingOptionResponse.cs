﻿namespace Smartstore.Core.Checkout.Shipping
{
    /// <summary>
    /// Represents a response from get shipping options
    /// </summary>
    public partial class ShippingOptionResponse
    {
        /// <summary>
        /// Gets shipping options
        /// </summary>
        public List<ShippingOption> ShippingOptions { get; set; } = [];

        /// <summary>
        /// Gets or sets errors
        /// </summary>
        public List<string> Errors { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether the response is successful
        /// </summary>
        public bool Success
            => Errors.Count == 0;
    }
}
