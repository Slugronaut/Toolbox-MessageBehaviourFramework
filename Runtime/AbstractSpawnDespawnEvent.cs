using UnityEngine;
using UnityEngine.SceneManagement;

namespace Peg.Messaging
{
    /// <summary>
    /// Base class for creating a message posting component that posts a buffered
    /// 'spawn' event when activatedand a 'removed' message when deactivated/destroyed.
    /// </summary>
    /// <typeparam name="S">A buffered message that will be posted when the objects spawns.</typeparam>
    /// <typeparam name="D">A normal message that will be posted when the object de-spawns.</typeparam>
    public abstract class AbstractSpawnDespawnEvent<S, D> : AbstractAuthoritativeGlobalPost<S> where S : IBufferedMessage where D : IMessage
    {
        [Tooltip("When does this spawn message post?")]
        public StartEventTiming TriggersOn;

        [Tooltip("Should the internal spawn message be removed when a scene unloads?")]
        public bool CleanupOnSceneChange = true;

        bool Posted;


        protected virtual void Awake()
        {
            if (TriggersOn == StartEventTiming.Awake)
                Post();
        }

        protected virtual void Start()
        {
            if (TriggersOn == StartEventTiming.Start)
                Post();
        }

        protected virtual void OnEnable()
        {
            if (TriggersOn == StartEventTiming.Enable)
                Post();
        }

        protected virtual void OnDisable()
        {
            Unpost();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Unpost();
        }

        void SceneUnloaded(Scene scene)
        {
            if (CleanupOnSceneChange)
                CleanupMessage();
        }

        void Post()
        {
            if (ValidateAuthority())
            {
                PostMessage();
                Posted = true;
            }
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        void Unpost()
        {
            if (Posted)
                GlobalMessagePump.Instance.PostMessage(ActivateDespawnMsg());
        }

        protected abstract D ActivateDespawnMsg();
    }

}
