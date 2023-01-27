
namespace Toolbox.Messaging
{
    /// <summary>
    /// Handles a LocalCallbackMessage and invokes the supplied callback if it
    /// matches this object's defined id.
    /// </summary>
    public abstract class AbstractLocalCallbackBehaviour : LocalListenerBehaviour
    {
        protected abstract int MyId { get; }

        protected virtual void Awake()
        {
            DispatchRoot.AddLocalListener<LocalCallbackMessage>(HandleCallback);
        }

        protected override void OnDestroy()
        {
            DispatchRoot.RemoveLocalListener<LocalCallbackMessage>(HandleCallback);
            base.OnDestroy();
        }

        public void HandleCallback(LocalCallbackMessage msg)
        {

            if (msg.Id == MyId && msg != null)
                msg.Callback();
        }
    }

}