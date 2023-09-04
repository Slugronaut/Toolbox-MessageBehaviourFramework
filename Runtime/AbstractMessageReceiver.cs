using UnityEngine;
using System;

namespace Peg.Messaging
{
    /// <summary>
    /// Base class for creating Toolbox event handler components.
    /// </summary>
    public abstract class AbstractMessageReciever : AbstractBaseMessageReciever, ILocalDispatchListener
    {
#if UNITY_EDITOR || FUCKED_SERIALIZER
        public bool SupressListenerWarning = false;
#endif
        public enum ListenerType
        {
            Local = 1,
            Global = 2,
            All = Local | Global,
        }

        [Space(10)]
        [Tooltip("Does this message listen for locally dispatched messages, global, or both? NOTE: This cannot be changed in the inspector at runtime.")]
        [SerializeField]
        ListenerType _ListensFor = ListenerType.Local;
        public ListenerType ListensFor
        {
            get { return _ListensFor; }
            set
            {
                if (value != _ListensFor)
                {
                    //if listenertype has changed, we need to change our listener too
                    UnregisterListener(MsgType, InnerHandleMessage);
                    RegisterListener(MsgType, InnerHandleMessage);
                }
                _ListensFor = value;
            }
        }

        LocalMessageDispatch _DispatchRoot;
        public LocalMessageDispatch DispatchRoot
        {
            get
            {
                if (_DispatchRoot == null)
                {
                    _DispatchRoot = gameObject.FindComponentInEntity<LocalMessageDispatch>();
#if UNITY_EDITOR
                    if (_DispatchRoot == null && !SupressListenerWarning)
                        throw new UnityException("The component '" + this.GetType().Name + "' attached to '" + gameObject.name + "' requires there to be a LocalMessageDispatch attached to its autonomous entity hierarchy.");
#else
                    Debug.LogWarning("The component '" + this.GetType().Name + "' attached to '" + gameObject.name + "' requires there to be a LocalMessageDispatch attached to its autonomous entity hierarchy.");
#endif
                }
                return _DispatchRoot;
            }
        }
        

#if UNITY_EDITOR
        protected override void Awake()
        {
            if (!Application.isPlaying)
            {
                //if we are in edit-mode, check that we have a LocalMessageDispatch somewhere in this hierarchy.
                //This won't catch it at runtime but for the most part these kinds of things will only ever
                //be added at edit-time so it's fairly safe.
                bool error = false;
                var root = gameObject.GetEntityRoot();
                if (root == null) error = true;
                else
                {
                    var lmd = root.FindComponentInEntity<LocalMessageDispatch>();
                    if (lmd == null) error = true;
                }

                if (error)
                {
                    Debug.LogError("GameObject '" + gameObject.name + "' does not have a LocalMessageDispatch. Cannot attach the script '" + GetType().Name + "' without one. Removing script now.");
                    DestroyImmediate(this);
                    return;
                }
            }
            base.Awake();
        }
#endif

        protected override void OnDestroy()
        {
            _DispatchRoot = null;
            base.OnDestroy();
        }

        protected override void RegisterListener(Type msgType, MessageHandler handler)
        {
            if ((_ListensFor & ListenerType.Local) != 0 && DispatchRoot != null)
                DispatchRoot.AddLocalListener(msgType, handler);
            else if ((_ListensFor & ListenerType.Global) != 0)
                GlobalMessagePump.Instance.AddListener(msgType, handler);

        }

        protected override void UnregisterListener(Type msgType, MessageHandler handler)
        {
            if ((_ListensFor & ListenerType.Local) != 0 && DispatchRoot != null)
                DispatchRoot.RemoveLocalListener(msgType, handler);
            else if ((_ListensFor & ListenerType.Global) != 0)
                GlobalMessagePump.Instance.RemoveListener(msgType, handler);
        }

    }
}