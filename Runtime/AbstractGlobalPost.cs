using Peg.MessageDispatcher;
using UnityEngine;

namespace Peg.Messaging
{
    /// <summary>
    /// Base class for deriving component events that should
    /// be posted when they are destroyed or started.
    /// It is left to the derived class to decide when to post the message
    /// using 'PostMessage()'.
    /// 
    /// TODO: Update this to support local posting?
    /// </summary>
    public abstract class AbstractGlobalPost<T> : MonoBehaviour where T : IMessage
    {
        T StoredMsg;
        protected abstract T ActivateMsg();

        /// <summary>
        /// Posts a message. Usually called in Start() or OnDestroy()
        /// depending on the purpose of the event.
        /// </summary>
        protected void PostMessage()
        {
            StoredMsg = ActivateMsg();
            GlobalMessagePump.Instance.PostMessage(StoredMsg);
        }

        /// <summary>
        /// Ensures all data associated with message is cleaned up.
        /// This should usually be called in OnDestroy().
        /// </summary>
        protected void CleanupMessage()
        {
            IBufferedMessage msg = StoredMsg as IBufferedMessage;
            if (msg != null)
                GlobalMessagePump.Instance.RemoveBufferedMessage(msg);
        }

        /// <summary>
        /// Automatically cleans up buffered messages.
        /// </summary>
        protected virtual void OnDestroy()
        {
            CleanupMessage();
        }
    }
}