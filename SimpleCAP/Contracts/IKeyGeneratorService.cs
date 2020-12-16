namespace SimpleCAP.Contracts
{
    /// <summary>
    /// Generates the unique key
    /// </summary>
    public interface IKeyGeneratorService
    {
        /// <summary>
        /// Returns key
        /// </summary>
        public string Generate();
    }
}
