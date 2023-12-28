using Peg.MessageDispatcher;
using System;

namespace Peg.Messaging
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