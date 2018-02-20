using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace EventAggregation
{
    /// <summary>
    /// Event Aggregation Implementation. Offers Publish/Subscribe and messaging 
    /// to other components.
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        #region Members 
        
        // list to store all subscribers for ALL messages
        private List<WeakReference> _Subscribers = new List<WeakReference>();
        // A Dispatcher for marshalling calls to the UI-Thread (update databound lists, etc.)
        private Dispatcher _Dispatcher;
        // Thread-Synchronization locking object for access to Subscribers list
        private object _Lock = new object();

        #endregion

        #region Constructors

        public EventAggregator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        /// <param name="dispatcher">Dispatcher. Comes from App.xaml.cs</param>
        public EventAggregator(Dispatcher dispatcher)
        {
            _Dispatcher = dispatcher;

            // just one rough  possibility to purge dead subscriptions
            // runs every minute - needs to be adjusted for other apps.
            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            dispatcherTimer.Tick += RemoveDeadReferences;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }

        #endregion

        #region IEventAggregator Implementations 

        /// <summary>
        /// Subscribes an object to the event aggregator.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Subscribe(object subscriber)
        {
            RunLocked(() =>
            {
                if (IsNotNull(subscriber) &&
                    IsNotYetInCollection(subscriber)&&
                    IsHandlerInterfaceImplemented(subscriber))
                {
                    _Subscribers.Add(new WeakReference(subscriber));
                }
                else
                {
                    throw new ArgumentException("Subscriber must implement IHandle<TMessage>.", "subscriber");
                } 
            });
        }
        
        /// <summary>
        /// Unsubscribes an object from the event aggregator if registered.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe(object subscriber)
        {
            RunLocked(() =>
            {
                var weakRefToRemove = (from item in _Subscribers
                                        where item.Target == subscriber
                                        select item)
                        .FirstOrDefault();

                _Subscribers.Remove(weakRefToRemove);
            });
        }

        /// <summary>
        /// Publishes a new message to all subsribers of this messagetype
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message containing data.</param>
        /// <returns></returns>
        public TMessage Publish<TMessage>(TMessage message)
        {
            RunLocked(() =>
            {
                foreach (var subscriber in GetAllSubscribersFor<TMessage>())
                {
                    if (_Dispatcher != null)
                        _Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => subscriber.Handle(message)));
                    else
                        subscriber.Handle(message);
                }
            });

            return message;
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Gets the subscribers. For UnitTesting.
        /// </summary>
        /// <value>The subscribers.</value>
        internal IEnumerable<WeakReference> Subscribers
        {
            get
            {
                return _Subscribers;
            }
        }

        #endregion

        #region Internals and Privates

        /// <summary>
        /// Gets all subscribers for a specific messagetype.
        /// </summary>
        /// <typeparam name="TMessage">The messagetype to search registered implementers for.</typeparam>
        /// <returns></returns>
        internal IEnumerable<IHandle<TMessage>> GetAllSubscribersFor<TMessage>()
        {
            return (from subscriber in _Subscribers
                    let handler = subscriber.Target as IHandle<TMessage>
                    where handler != null
                    select handler).ToList();
        }

        /// <summary>
        /// This is just to find wether an object subscribing is implementing the open generic version of IHandle<> 
        /// with any TMessage. It does not matter which TMessage exactly.
        /// </summary>
        /// <param name="subscriber">The subscribing object.</param>
        /// <returns>
        /// 	<c>true</c> if <see cref="IHandle{TMessage}<>"/>  is implemented; otherwise, <c>false</c>.
        /// </returns>
        private bool IsHandlerInterfaceImplemented(object subscriber)
        {
            Type[] interfaces = subscriber.GetType()
                    .FindInterfaces((type, criteria) =>
                        {
                            if (type.IsGenericType && 
                                type.GetGenericTypeDefinition() == typeof(IHandle<>))
                                return true;

                            return false;
                        }, null);

            return interfaces.Length > 0 ? true : false;
        }

        /// <summary>
        /// Runs all actions within a lock for thread-safe access to the subscribers list.
        /// ReaderWriterLock is a better choice than simple locking but it is more complex. 
        /// </summary>
        /// <param name="action">The action to execute within a lock.</param>
        private void RunLocked(Action action)
        {
            lock (_Lock)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Determines whether the subscriber is null
        /// </summary>
        /// <param name="subscriber">The subscriber to check.</param>
        /// <returns>
        /// 	<c>true</c> if the subscriber is not null; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNotNull(object subscriber)
        {
            return subscriber != null ? true : false;
        }

        /// <summary>
        /// Determines whether the object is already in the collection
        /// </summary>
        /// <param name="subscriber">The subscriber to check for.</param>
        /// <returns>
        /// 	<c>true</c> if the subscriber is not registered yet; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNotYetInCollection(object subscriber)
        {
            var element = _Subscribers.Select(o => o).Where(o => o.Target == subscriber).FirstOrDefault();
            return element == null ? true : false;
        }

        /// <summary>
        /// Removes registrations of diposed subscribers
        /// </summary>
        private void RemoveDeadReferences(object sender, EventArgs eventArgs)
        {
            RunLocked(() =>
            {
                var weakReferences = (from item in _Subscribers
                                      where !item.IsAlive
                                      select item).ToList();

                weakReferences.ForEach(reference => _Subscribers.Remove(reference));
            });
        }

        #endregion
    }
}