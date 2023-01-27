
namespace Toolbox.Messaging
{
    /// <summary>
    /// Implemented by behaviours that provide access to their local AEH message dispatch.
    /// </summary>
    public interface ILocalDispatchListener
    {
        LocalMessageDispatch DispatchRoot { get; }
    }
}