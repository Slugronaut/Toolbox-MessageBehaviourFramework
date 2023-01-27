using System;

namespace Toolbox.Messaging
{

    /// <summary>
    /// 
    /// </summary>
    public class LocalCallbackMessage : IDeferredMessage
    {
        public int Id { get; private set; }
        public Action Callback { get; private set; }

        public LocalCallbackMessage(int id, Action callback)
        {
            Callback = callback;
        }
    }
}