using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Etsy.Consts
{
    public class ListingState
    {
        /// <summary>
        /// The Listing is currently for sale.
        /// </summary>
        public const string Active = "active";

        /// <summary>
        /// The Listing has been removed by its owner
        /// </summary>
        public const string Removed = "removed";

        /// <summary>
        /// The Listing has sold out
        /// </summary>
        public const string SoldOut = "sold_out";

        /// <summary>
        /// The Listing has expired
        /// </summary>
        public const string Expired = "expired";

        /// <summary>
        /// The Listing is inactive. (For legacy reasons, this displays as "edit".)
        /// </summary>
        public const string Inactive = "edit";

        /// <summary>
        /// Draft listings are listings that have been saved prior to their first activation.
        /// </summary>
        public const string Draft = "draft";

        /// <summary>
        /// The owner of the Listing has requested that it not appear in API results
        /// </summary>
        public const string Private = "private";

        /// <summary>
        /// The Listing has been removed by Etsy admin for unspecified reasons.
        /// </summary>
        public const string Unavailable = "unavailable"; 
    }
}
