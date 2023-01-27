using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Toolbox.Messaging
{
    /// <summary>
    /// Convienience base class that can be used to translate between two message types.
    /// </summary>
    public abstract class AbstractMessageTranslator : AbstractMessageReciever
    {
        [Tooltip("The message type to react to.")]
        [ValueDropdown("ListOfTypes")]
        public string MessageOut;
        protected Type MsgTypeOut;


        protected override void Awake()
        {
            base.Awake();
            if (string.IsNullOrEmpty(MessageOut) || MessageOut == "None")
            {
                Debug.LogWarning("No outbound message type has been specified for this instance of 'EnableOnLocalMessage' component.");
                return;
            }

            MsgTypeOut = TypeHelper.GetType(MessageOut);
            if (MsgTypeOut == null) Debug.LogError("The outbound message type '" + MessageOut + "' does not exist in the project assembly. " + name);
            //Note: listener is added in OnEnable
        }
    }
}