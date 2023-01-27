using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections;

namespace Toolbox.Messaging
{
    /// <summary>
    /// Base class for creating Toolbox event handler components.
    /// </summary>
    public abstract class AbstractBaseMessageReciever : MonoBehaviour//Sirenix.OdinInspector.SerializedMonoBehaviour
    {
        #region Fields and Properties
        [ValueDropdown("ListOfTypes")]
        public string MessageType;

        [Tooltip("If set, this object will only listen for the event when active and enabled. During runtime, setting this to false while this component is not active will cause errant behaviour.")]
        public bool CanDisable;

#if UNITY_EDITOR || FUCKED_SERIALIZER
        /// <summary>
        /// This field is for editor use only! It will be stripped in the final build!
        /// </summary>
        [FoldoutGroup("Debugging")]
        [Tooltip("Editor-only. Pauses the playmode in the editor upon receiving a message.")]
        public bool PauseOnReceive;

        protected IEnumerable ListOfTypes() => ClassDropDownList.AllTypesFromInterface(typeof(IMessage), false);

        public enum DebugType
        {
            None,
            Simple,
            StackTrace,
        }

        [FoldoutGroup("Debugging")]
        [Tooltip("Prints debug information to the console to aid in tracking the sequence of messages.")]
        public DebugType PrintDebug;

        [FoldoutGroup("Debugging")]
        [SerializeField]
        [TextArea(3, 5)]
        [Tooltip("Place to leave useful notes about the nature of this component.\n\nWARNING: Editor data only! This will be stripped after a build!")]
#pragma warning disable CS0169 // The field 'AbstractBaseMessageReciever.Notes' is never used
#pragma warning disable IDE0044 // Add readonly modifier
        string Notes;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0169 // The field 'AbstractBaseMessageReciever.Notes' is never used

#endif

        protected Type MsgType;
        bool Registered;
        #endregion


        #region Methods
        protected virtual void Awake()
        {

            if (string.IsNullOrEmpty(MessageType) || MessageType == "None")
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(this);
#endif
                Debug.LogWarning("No message type has been specified for this instance of <color=green>" + GetType().Name + "</color> component on <color=blue>" + name + "</color>.");
                return;
            }


            MsgType = TypeHelper.GetType(MessageType);
            if (MsgType == null) Debug.LogError("GameObject: " + name + " - The message type '" + MessageType + "' does not exist in the project assembly.");
            //Note: listener is added in OnEnable
        }

        protected virtual void OnDestroy()
        {
            if (MsgType != null && Registered)
            {
                Registered = false;
                UnregisterListener(MsgType, InnerHandleMessage);
            }
        }

        protected virtual void OnEnable()
        {
            if (MsgType != null && !Registered)
            {
                Registered = true;
                RegisterListener(MsgType, InnerHandleMessage);
            }
        }

        protected virtual void OnDisable()
        {
            if (CanDisable && MsgType != null && Registered)
            {
                Registered = false;
                UnregisterListener(MsgType, InnerHandleMessage);
            }
        }
        #endregion

        /// <summary>
        /// Internally perform some filtering before forwarding
        /// to the actual overriding handler.
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        protected virtual void InnerHandleMessage(Type msgType, object msg)
        {
#if UNITY_EDITOR
            if (PrintDebug > 0)
            {
                string stack = PrintDebug == DebugType.StackTrace ? StackTraceUtility.ExtractStackTrace() : string.Empty;
                Debug.Log("<color=blue>{Toolbox Message Received}</color> <color=green>Receiver:</color> " + name + "." + GetType().Name + "  <color=green>MessageType:</color> " + msgType.Name + @"
                            " + stack);
            }
            if (PauseOnReceive)
                UnityEditor.EditorApplication.isPaused = true;
#endif
            if (CanDisable && !isActiveAndEnabled) return;
            else HandleMessage(msgType, msg);
        }

        /// <summary>
        /// Can be used to manually invoke the handler without passign any parameters. Mostly
        /// used for rigging to UnityEvents when no parameters are needed.
        /// </summary>
        public void Invoke()
        {
            HandleMessage(null, null);
        }

        /// <summary>
        /// Override this to handle the message this component is waiting to receive.
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        protected abstract void HandleMessage(Type msgType, object msg);
        protected abstract void RegisterListener(Type msgType, MessageHandler handler);
        protected abstract void UnregisterListener(Type msgType, MessageHandler handler);
    }
}
