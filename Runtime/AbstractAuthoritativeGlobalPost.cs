using UnityEngine;

namespace Peg.Messaging
{

    /// <summary>
    /// Base class for local messages that should be posted when starting. Provides data
    /// and helper method for confirming the network ownership of the object.
    /// It is left to the derived class to decide when to post the message
    /// using 'PostMessage()'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractAuthoritativeGlobalPost<T> : AbstractGlobalPost<T> where T : IMessage
    {
        [Tooltip("If set, this object must be controlled by the local connection or the network system must not be active.")]
        public bool RequiresAuthority = true;

        protected bool ValidateAuthority()
        {
            //removed old code due to outdated UNet HLAPI
            return true;
        }
    }
}