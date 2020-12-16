using SimpleCAP.Handlers;

namespace SimpleCAP.Contracts
{
    /// <summary>
    /// Overrides default topic Name and Group. Usable with TRequest and <see cref="ICapEventHandler"/>
    /// </summary>
    public interface IOverrideSubscribe
    {
        /// <summary>
        /// Topic or exchange route key name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Default group name is CapOptions setting.(Assembly name)
        /// kafka --> groups.id
        /// rabbit MQ --> queue.name
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Returns Name if exists
        /// </summary>
        bool TryGetName(out string name);

        /// <summary>
        /// Returns Group if exists
        /// </summary>
        bool TryGetGroup(out string group);
    }
}
