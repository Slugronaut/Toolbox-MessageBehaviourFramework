using UnityEngine;
using System;
using Toolbox.Attributes;

namespace Toolbox.Messaging
{
    /// <summary>
    /// Base class for deriving components that should be able to post messages to the dispatcher.
    /// </summary>
    public abstract class AbstractMessagePoster : MonoBehaviour
    {
        public enum MessageScope
        {
            Local = 1,
            Global = 2,
            All = Local | Global,
        }

        [Tooltip("Does this message post on this gameobject's local dispatcher, the global, or both?")]
        public MessageScope Scope = MessageScope.Local;

        [Tooltip("The message type to post.")]
        [InterfaceList(typeof(IMessage), "Message Type")]
        public InstantiableType MessageOut;

        protected EntityRoot Root;
        protected Type CachedType;
        protected IMessage MessageInstance;


        /// <summary>
        /// 
        /// </summary>
        protected virtual void Awake()
        {
            CreateMessageInstance();

            //Note: listener should be added in OnEnable
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreateMessageInstance()
        {
            if (string.IsNullOrEmpty(MessageOut.TypeName) || MessageOut.TypeName == "None")
            {
                Debug.LogWarning("No outbound message type has been specified for this instance of 'EnableOnLocalMessage' component.");
                return;
            }
            if (MessageOut.Type == null)
                Debug.LogError("The outbound message type '" + MessageOut + "' does not exist in the project assembly. " + name);

            MessageInstance = MessageOut.Instantiate() as IMessage;
            if (MessageInstance == null)
                Debug.LogWarning($"Failed to create an instance of the outbound message type '{MessageOut.TypeName}'.");
            else CachedType = MessageOut.Type;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Start()
        {
            Root = gameObject.GetEntityRoot();
        }

        /// <summary>
        /// Posts the named event based on scope.
        /// </summary>
        public void PostMessage()
        {
            if (((int)Scope & (int)MessageScope.Local) != 0)
                GlobalMessagePump.Instance.ForwardDispatch(gameObject, CachedType, MessageInstance);
            if (((int)Scope & (int)MessageScope.Global) != 0)
                GlobalMessagePump.Instance.PostMessage(CachedType, MessageInstance);
        }
    }
}