using Hexx.DTO;
using System;
using System.Collections.Generic;

namespace Hexx.Connection
{
    public delegate void IncomingMessageDelegate<T>(T response) where T : Message;
    public class IncomingMessageManager
    {
        private static Dictionary<Type, Delegate> messageIncomingListeners = new Dictionary<Type, Delegate>();
        public static void AddIncomingMessageListener<T>(IncomingMessageDelegate<T> listener) where T : Message
        {
            if (!messageIncomingListeners.ContainsKey(typeof(T)))
                messageIncomingListeners.Add(typeof(T), listener);
            else
            {
                IncomingMessageDelegate<T> messageDelegate = messageIncomingListeners[typeof(T)] as IncomingMessageDelegate<T>;
                messageDelegate += listener;
                messageIncomingListeners[typeof(T)] = messageDelegate;
            }
        }

        public static bool RemoveIncomingMessageListener<T>(IncomingMessageDelegate<T> listener) where T : Message
        {
            if (messageIncomingListeners.ContainsKey(typeof(T)))
            {
                IncomingMessageDelegate<T> messageDelegate = messageIncomingListeners[typeof(T)] as IncomingMessageDelegate<T>;
                messageDelegate -= listener;
                messageIncomingListeners[typeof(T)] = messageDelegate;
                return true;
            }
            return false;
        }

        public static void PushMessage(Message message)
        {
            if (messageIncomingListeners.ContainsKey(message.GetType()))
                messageIncomingListeners[message.GetType()]?.DynamicInvoke(message);
        }
    }
}