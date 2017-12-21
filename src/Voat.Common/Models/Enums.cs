using System;
using System.Collections.Generic;
using System.Text;

namespace Voat.Common
{

    /// <summary>
    /// Specifies the direction result sets should be sorted.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Default sort order for result set
        /// </summary>
        Default,

        /// <summary>
        /// Reversed sort order for result set
        /// </summary>
        Reverse
    }

    /// <summary>
    /// Specifies the time span window to use to filter results in set.
    /// </summary>
    public enum SortSpan
    {
        /// <summary>
        /// Default value
        /// </summary>
        All = 0,

        /// <summary>
        /// Limits search span to 1 hour
        /// </summary>
        Hour,

        /// <summary>
        /// Limits search span to 1 day
        /// </summary>
        Day,

        /// <summary>
        /// Limits search span to 1 week
        /// </summary>
        Week,

        /// <summary>
        /// Limits search span to ~30 days
        /// </summary>
        Month,

        /// <summary>
        /// Limits search span to ~90 days
        /// </summary>
        Quarter,

        /// <summary>
        /// Limits search span to 1 year
        /// </summary>
        Year
    }
}
