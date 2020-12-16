using Newtonsoft.Json.Linq;
using SimpleCAP.Contracts.Bus;

namespace SimpleCAP.Bus
{
    /// <summary>
    /// Value converter
    /// </summary>
    public class ValueConverterService : IValueConverterService
    {
        /// <summary>
        /// Convert
        /// </summary>
        public TResponse Convert<TResponse>(object value)
        {
            if (value is JObject jObj)
            {
                return jObj.ToObject<TResponse>();
            }

            return (TResponse)value;
        }
    }
}
