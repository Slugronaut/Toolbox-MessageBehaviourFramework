using UnityEngine;
using System;

namespace Peg.Messaging
{
    /// <summary>
    /// Derivable convenience base class for behaviours that need access to the LocalMessageDispatch
    /// attached to their Autonomous Entity hierarchy.
    /// </summary>
    [Serializable]
    public abstract partial class LocalListenerBehaviour : MonoBehaviour, ILocalDispatchListener
    {
        LocalMessageDispatch _DispatchRoot;
        public LocalMessageDispatch DispatchRoot
        {
            get
            {
                if (_DispatchRoot == null)
                {
                    _DispatchRoot = gameObject.FindComponentInEntity<LocalMessageDispatch>();
                    if (_DispatchRoot == null) throw new UnityException("The component '" + this.GetType().Name + "' attached to '" + gameObject.name + "' requires there to be a LocalMessageDispatch attached to its autonomous entity hierarchy.");
                }
                return _DispatchRoot;
            }
        }

        protected virtual void OnDestroy()
        {
            _DispatchRoot = null;
        }
    }

}