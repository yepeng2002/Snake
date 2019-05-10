using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snake.App.Controls.Mvvm
{
    /// <summary>
    /// Provides loosely-coupled messaging between various colleague objects.  All references to objects are stored weakly, to prevent memory leaks.
    /// 提供松散耦合的消息通知机制，为防止内存泄漏，所有对象都使用了弱引用（WeakReference）
    /// </summary>
    public class Messenger : IMessenger
    {
        #region Static Field

        private static IMessenger s_Instance;
        private static readonly object s_lock = new object();

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IMessenger Default
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_lock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new Messenger();
                        }
                    }
                }

                return s_Instance;
            }
        }        
        #endregion
        

        #region Constructor

        public Messenger()
        {
        }

        #endregion // Constructor

        #region Register

        /// <summary>
        /// Registers a callback method, with no parameter, to be invoked when a specific message is broadcasted.
        /// 注册消息监听
        /// </summary>
        /// <param name="key">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcasted.</param>
        public void Register(object recipient, string key, Action callback)
        {
            this.Register(recipient, key, callback, null);
        }

        /// <summary>
        /// Registers a callback method, with a parameter, to be invoked when a specific message is broadcasted.
        /// 注册消息监听
        /// </summary>
        /// <param name="key">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcasted.</param>
        public void Register<TMessage>(object recipient, string key, Action<TMessage> callback)
        {
            this.Register(recipient, key, callback, typeof(TMessage));
        }

        void Register(object recipient, string key, Delegate callback, Type parameterType)
        {
            if (recipient == null)
                throw new ArgumentNullException("recipient");

            if (String.IsNullOrEmpty(key))
                throw new ArgumentException("'message' cannot be null or empty.");

            if (callback == null)
                throw new ArgumentNullException("callback");

            this.VerifyParameterType(key, parameterType);

            _messageToActionsMap.AddAction(recipient, key, callback.Target, callback.Method, parameterType);
        }

        [Conditional("DEBUG")]
        void VerifyParameterType(string key, Type parameterType)
        {
            Type previouslyRegisteredParameterType = null;
            if (_messageToActionsMap.TryGetParameterType(key, out previouslyRegisteredParameterType))
            {
                if (previouslyRegisteredParameterType != null && parameterType != null)
                {
                    if (!previouslyRegisteredParameterType.Equals(parameterType))
                        throw new InvalidOperationException(string.Format(
                            "The registered action's parameter type is inconsistent with the previously registered actions for message '{0}'.\nExpected: {1}\nAdding: {2}",
                            key,
                            previouslyRegisteredParameterType.FullName,
                            parameterType.FullName));
                }
                else
                {
                    // One, or both, of previouslyRegisteredParameterType or callbackParameterType are null.
                    if (previouslyRegisteredParameterType != parameterType)   // not both null?
                    {
                        throw new TargetParameterCountException(string.Format(
                            "The registered action has a number of parameters inconsistent with the previously registered actions for message \"{0}\".\nExpected: {1}\nAdding: {2}",
                            key,
                            previouslyRegisteredParameterType == null ? 0 : 1,
                            parameterType == null ? 0 : 1));
                    }
                }
            }
        }

        #endregion // Register

        #region Notify

        /// <summary>
        /// Notifies all registered parties that a message is being broadcasted.
        /// 发送消息通知，触发监听执行
        /// </summary>
        /// <param name="key">The message to broadcast.</param>
        /// <param name="parameter">The parameter to pass together with the message.</param>
        public void Notify(Type messageTargetType, string key, object parameter)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentException("'message' cannot be null or empty.");

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(key, out registeredParameterType))
            {
                if (registeredParameterType == null)
                    throw new TargetParameterCountException(string.Format("Cannot pass a parameter with message '{0}'. Registered action(s) expect no parameter.", key));
            }

            var actions = _messageToActionsMap.GetActions(messageTargetType, key);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke(parameter));
        }

        /// <summary>
        /// Notifies all registered parties that a message is being broadcasted.
        /// 发送消息通知，触发监听执行
        /// </summary>
        /// <param name="key">The message to broadcast.</param>
        public void Notify(Type messageTargetType, string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentException("'message' cannot be null or empty.");

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(key, out registeredParameterType))
            {
                if (registeredParameterType != null)
                    throw new TargetParameterCountException(string.Format("Must pass a parameter of type {0} with this message. Registered action(s) expect it.", registeredParameterType.FullName));
            }

            var actions = _messageToActionsMap.GetActions(messageTargetType, key);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke());
        }

        #endregion // NotifyColleauges

        #region IMessenger
        public void Register<TMessage>(object recipient, object token, bool receiveInheritedMessages, Action<TMessage> action)
        {
            string key = string.Empty;
            if (token == null)
                key = typeof(TMessage).ToString();
            else
                key = token.ToString();

            Register(recipient, key, action);
        }

        public void Send<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            string key = string.Empty;
            if (token == null)
                key = typeof(TMessage).ToString();
            else
                key = token.ToString();

            if (message != null)
                Notify(messageTargetType, key, message);
            else
                Notify(messageTargetType, key);
        }

        public void Unregister(object recipient)
        {
            _messageToActionsMap.RemoveAction(recipient);
        }

        public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            string key = string.Empty;
            if (token == null)
                key = typeof(TMessage).ToString();
            else
                key = token.ToString();

            if (String.IsNullOrEmpty(key))
                throw new ArgumentException("'key' cannot be null or empty.");

            if (action == null)
                throw new ArgumentNullException("action");

            this.VerifyParameterType(key, typeof(TMessage));

            _messageToActionsMap.RemoveAction(recipient, key, action.Target, action.Method, typeof(TMessage));
        }
        #endregion

        #region MessageToActionsMap [nested class]

        /// <summary>
        /// This class is an implementation detail of the Messenger class.
        /// </summary>
        private class MessageToActionsMap
        {
            #region Constructor

            internal MessageToActionsMap()
            {
            }

            #endregion // Constructor

            #region AddAction

            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="key">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            /// <param name="method">The method to invoke.</param>
            /// <param name="actionType">The type of the Action delegate.</param>
            internal void AddAction(object recipient, string key, object target, MethodInfo method, Type actionType)
            {
                if (key == null)
                    throw new ArgumentNullException("message");

                if (method == null)
                    throw new ArgumentNullException("method");

                lock (_map)
                {
                    if (!_map.ContainsKey(key))
                        _map[key] = new List<WeakAction>();

                    _map[key].Add(new WeakAction(recipient, target, method, actionType));
                }
            }

            #endregion // AddAction

            #region GetActions

            /// <summary>
            /// Gets the list of actions to be invoked for the specified message
            /// </summary>
            /// <param name="key">The message to get the actions for</param>
            /// <returns>Returns a list of actions that are registered to the specified message</returns>
            internal List<Delegate> GetActions(Type messageTargetType, string key)
            {
                if (key == null)
                    throw new ArgumentNullException("message");

                List<Delegate> actions;
                lock (_map)
                {
                    if (!_map.ContainsKey(key))
                        return null;

                    List<WeakAction> weakActions = _map[key];
                    actions = new List<Delegate>(weakActions.Count);
                    for (int i = weakActions.Count - 1; i > -1; --i)
                    {
                        WeakAction weakAction = weakActions[i];
                        if (weakAction == null)
                            continue;
                        if (weakAction.Recipient.GetType() == messageTargetType)
                            continue;

                        Delegate action = weakAction.CreateAction();
                        if (action != null)
                        {
                            actions.Add(action);
                        }
                        else
                        {
                            // The target object is dead, so get rid of the weak action.
                            weakActions.Remove(weakAction);
                        }
                    }

                    // Delete the list from the map if it is now empty.
                    if (weakActions.Count == 0)
                        _map.Remove(key);
                }

                // Reverse the list to ensure the callbacks are invoked in the order they were registered.
                actions.Reverse();

                return actions;
            }

            #endregion // GetActions

            #region RemoveAction

            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="key">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            internal void RemoveAction(object recipient)
            {
                lock (_map)
                {
                    foreach (var keyValue in _map)
                    {
                        List<WeakAction> delList = new List<WeakAction>();
                        foreach (var item in keyValue.Value)
                        {
                            if (object.ReferenceEquals(item.Recipient, recipient))
                            {
                                delList.Add(item);
                            }
                        }
                        foreach (var item in delList)
                        {
                            keyValue.Value.Remove(item);
                        }
                        if (keyValue.Value.Count == 0)
                            _map.Remove(keyValue.Key);
                        delList.Clear();
                    }
                }
            }

            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="recipient"></param>
            /// <param name="key">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            /// <param name="method">The method to invoke.</param>
            /// <param name="actionType">The type of the Action delegate.</param>
            internal void RemoveAction(object recipient, string key, object target, MethodInfo method, Type actionType)
            {
                if (key == null)
                    throw new ArgumentNullException("message");

                if (method == null)
                    throw new ArgumentNullException("method");

                lock (_map)
                {
                    if (_map.ContainsKey(key) && _map[key] != null)
                    {
                        List<WeakAction> delList = new List<WeakAction>();
                        foreach(var item in _map[key])
                        {
                            if (object.ReferenceEquals(item.TargetRef.Target, target) && object.ReferenceEquals(item.Recipient, recipient)
                                && item.Method.Name == method.Name && item.ParameterType == actionType)
                            {
                                delList.Add(item);
                            }
                        }
                        foreach (var item in delList)
                        {
                            _map[key].Remove(item);
                        }
                        if (_map[key].Count == 0)
                            _map.Remove(key);
                        delList.Clear();
                    }
                }
            }

            #endregion // RemoveAction

            #region TryGetParameterType

            /// <summary>
            /// Get the parameter type of the actions registered for the specified message.
            /// </summary>
            /// <param name="messageTargetType"></param>
            /// <param name="key">The message to check for actions.</param>
            /// <param name="parameterType">
            /// When this method returns, contains the type for parameters 
            /// for the registered actions associated with the specified message, if any; otherwise, null.
            /// This will also be null if the registered actions have no parameters.
            /// This parameter is passed uninitialized.
            /// </param>
            /// <returns>true if any actions were registered for the message</returns>
            internal bool TryGetParameterType(string key, out Type parameterType)
            {
                if (key == null)
                    throw new ArgumentNullException("message key");

                parameterType = null;
                List<WeakAction> weakActions;
                lock (_map)
                {
                    if (!_map.TryGetValue(key, out weakActions) || weakActions.Count == 0)
                        return false;
                }
                parameterType = weakActions[0].ParameterType;
                return true;
            }

            #endregion // TryGetParameterType

            #region Fields

            // Stores a hash where the key is the message and the value is the list of callbacks to invoke.
            readonly Dictionary<string, List<WeakAction>> _map = new Dictionary<string, List<WeakAction>>();

            #endregion // Fields
        }

        #endregion // MessageToActionsMap [nested class]

        #region WeakAction [nested class]

        /// <summary>
        /// This class is an implementation detail of the MessageToActionsMap class.
        /// </summary>
        private class WeakAction
        {
            #region Constructor

            /// <summary>
            /// Constructs a WeakAction.
            /// </summary>
            /// <param name="recipient"></param>
            /// <param name="target">The object on which the target method is invoked, or null if the method is static.</param>
            /// <param name="method">The MethodInfo used to create the Action.</param>
            /// <param name="parameterType">The type of parameter to be passed to the action. Pass null if there is no parameter.</param>
            internal WeakAction(object recipient, object target, MethodInfo method, Type parameterType)
            {
                _recipient = recipient;

                if (target == null)
                {
                    _targetRef = null;
                }
                else
                {
                    _targetRef = new WeakReference(target);
                }

                _method = method;

                this.ParameterType = parameterType;

                if (parameterType == null)
                {
                    _delegateType = typeof(Action);
                }
                else
                {
                    _delegateType = typeof(Action<>).MakeGenericType(parameterType);
                }
            }

            #endregion // Constructor

            #region CreateAction

            /// <summary>
            /// Creates a "throw away" delegate to invoke the method on the target, or null if the target object is dead.
            /// </summary>
            internal Delegate CreateAction()
            {
                // Rehydrate into a real Action object, so that the method can be invoked.
                if (_targetRef == null)
                {
                    return Delegate.CreateDelegate(_delegateType, _method);
                }
                else
                {
                    try
                    {
                        object target = _targetRef.Target;
                        if (target != null)
                            return Delegate.CreateDelegate(_delegateType, target, _method);
                    }
                    catch
                    {
                    }
                }

                return null;
            }

            #endregion // CreateAction

            #region Fields

            internal readonly Type ParameterType;

            readonly object _recipient;
            readonly Type _delegateType;
            readonly MethodInfo _method;
            readonly WeakReference _targetRef;

            public object Recipient
            {
                get { return _recipient; }
            }
            public Type DelegateType
            {
                get { return _delegateType; }
            }
            public MethodInfo Method
            {
                get { return _method; }
            }
            public WeakReference TargetRef
            {
                get { return _targetRef; }
            }

            #endregion // Fields
        }

        #endregion // WeakAction [nested class]

        #region Fields

        readonly MessageToActionsMap _messageToActionsMap = new MessageToActionsMap();

        #endregion // Fields

    }
}
