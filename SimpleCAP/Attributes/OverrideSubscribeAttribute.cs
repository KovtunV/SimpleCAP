using System;
using SimpleCAP.Contracts;
using SimpleCAP.Handlers;

namespace SimpleCAP.Attributes
{
    /// <summary>
    /// Overrides default topic Name and Group. Usable with TRequest and <see cref="ICapEventHandler"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OverrideSubscribeAttribute : Attribute, IOverrideSubscribe
    {
        /// <summary>
        /// Topic or exchange route key name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default group name is CapOptions setting.(Assembly name)
        /// kafka --> groups.id
        /// rabbit MQ --> queue.name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Returns Name if exists
        /// </summary>
        public bool TryGetName(out string name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                name = null;
                return false;
            }

            name = Name;
            return true;
        }

        /// <summary>
        /// Returns Group if exists
        /// </summary>
        public bool TryGetGroup(out string group)
        {
            if (string.IsNullOrWhiteSpace(Group))
            {
                group = null;
                return false;
            }

            group = Group;
            return true;
        }
    }
}
