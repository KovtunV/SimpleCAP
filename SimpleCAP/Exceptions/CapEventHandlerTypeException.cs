using System;

namespace SimpleCAP.Exceptions
{
    /// <summary>
    /// Wrong handler exception
    /// </summary>
    public class CapEventHandlerTypeException : Exception
    {
        /// <summary>
        /// Wrong handler exception
        /// </summary>
        public CapEventHandlerTypeException(string message) : base(message)
        {
            
        }
    }
}
