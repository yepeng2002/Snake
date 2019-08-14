using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.App.Controls.Mvvm
{
    public static class MessengerExtensions
    {
        public static void Register<TMessage>(this IMessenger messenger, object recipient, Action<TMessage> action)
        {
            messenger.Register<TMessage>(recipient: recipient, token: null, receiveInheritedMessages: true, action: action);
        }

        public static void Register<TMessage>(this IMessenger messenger, object recipient, bool receiveInheritedMessagesToo, Action<TMessage> action)
        {
            messenger.Register<TMessage>(recipient: recipient, token: null, receiveInheritedMessages: receiveInheritedMessagesToo, action: action);
        }

        public static void Register<TMessage>(this IMessenger messenger, object recipient, object token, Action<TMessage> action)
        {
            messenger.Register<TMessage>(recipient: recipient, token: token, receiveInheritedMessages: true, action: action);
        }

        public static void Send<TMessage>(this IMessenger messenger, TMessage message)
        {
            messenger.Send<TMessage>(message, messageTargetType: null, token: null);
        }

        public static void Send<TMessage, TTarget>(this IMessenger messenger, TMessage message)
        {
            messenger.Send<TMessage>(message, messageTargetType: typeof(TTarget), token: null);
        }

        public static void Send<TMessage>(this IMessenger messenger, TMessage message, object token)
        {
            messenger.Send<TMessage>(message, messageTargetType: null, token: token);
        }

        public static void Unregister<TMessage>(this IMessenger messenger, object recipient)
        {
            messenger.Unregister<TMessage>(recipient: recipient);
        }

        public static void Unregister<TMessage>(this IMessenger messenger, object recipient, object token)
        {
            messenger.Unregister<TMessage>(recipient: recipient, token: token, action: null);
        }

        public static void Unregister<TMessage>(this IMessenger messenger, object recipient, Action<TMessage> action)
        {
            messenger.Unregister<TMessage>(recipient: recipient, token: null, action: action);
        }
    }
}
