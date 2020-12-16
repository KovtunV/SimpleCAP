using System;

namespace SimpleCAP.Options
{
    /// <summary>
    /// Callback options
    /// </summary>
   public class CallbackOptions
    {
        /// <summary>
        /// Receive message timeout
        /// </summary>
        public TimeSpan? Timeout { get; set; }
    }
}
