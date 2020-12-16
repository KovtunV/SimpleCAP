using DotNetCore.CAP.Internal;
using SimpleCAP.Contracts;

namespace SimpleCAP.Services
{
    /// <summary>
    /// Generates the unique key
    /// </summary>
    public class KeyGeneratorService : IKeyGeneratorService
    {
        private readonly SnowflakeId _snowflakeId;

        /// <summary>
        /// Generates the unique key
        /// </summary>
        public KeyGeneratorService()
        {
            _snowflakeId = SnowflakeId.Default();
        }

        /// <summary>
        /// Returns key
        /// </summary>
        public string Generate()
        {
            return _snowflakeId.NextId().ToString();
        }
    }
}
