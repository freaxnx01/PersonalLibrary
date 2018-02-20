using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Xunit;

namespace EventAggregation.Tests
{
    /// <summary>
    /// UnitTests für den EventAggregator
    /// </summary>
    public class EventAggregatorTests
    {
        [Fact]
        public void Given_an_object_when_subscribing_should_find_one_subscriber()
        {
            EventAggregator aggregator = new EventAggregator();
            aggregator.Subscribe(new Subject());

            Assert.True(aggregator.Subscribers.Count() == 1);
        }

        [Fact]
        public void Given_an_object_without_IHandleThe_when_subscribing_should_throw_ArgumentException()
        {
            EventAggregator aggregator = new EventAggregator();
            Assert.Throws<ArgumentException>(() => aggregator.Subscribe(new object()));
        }

        [Fact]
        public void Given_an_object_when_unsubscribed_should_remove_subscriber()
        {
            Subject subject = new Subject();
            EventAggregator aggregator = new EventAggregator();
            aggregator.Subscribe(subject);
            aggregator.Unsubscribe(subject);
            
            Assert.True(aggregator.Subscribers.Count() == 0);
        }

        [Fact]
        public void Given_many_subscribers_should_find_all_subscribers_for_a_specific_message()
        {
            Subject subject = new Subject(); 
            EventAggregator aggregator = new EventAggregator();
            aggregator.Subscribe(subject);

            var subscribersForMessageOne = aggregator.GetAllSubscribersFor<MessageOne>();
            var subscribersForMessageTwo = aggregator.GetAllSubscribersFor<MessageTwo>();

            Assert.True(subscribersForMessageOne.First().Equals(subject));
            Assert.True(subscribersForMessageTwo.Count() == 0);
        }

        [Fact]
        public void Given_a_subscriber_when_publishing_a_message_should_handle_it()
        {
            Subject subject = new Subject();
            var aggregator = new EventAggregator();
            aggregator.Subscribe(subject);
            MessageOne messageOne = aggregator.Publish(new MessageOne{ Content = "content"});
            
            Assert.True(messageOne.Content.Equals("Received"));
        }

        [Fact]
        public void Given_an_EventAggregator_with_a_Dispatcher_when_publishing_a_message_should_handle_it()
        {
            // pushes tasks in Dispatcher queue towards getting worked.
            DispatcherFrame frame = new DispatcherFrame();

            MessageOne messageOne = null;
            Subject subject = new Subject();
            EventAggregator aggregator = new EventAggregator(Dispatcher.CurrentDispatcher);
            aggregator.Subscribe(subject);

            ManualResetEvent manualEvent = new ManualResetEvent(false);
            ThreadStart threadStart = () =>
                                          {
                                              messageOne = aggregator.Publish(new MessageOne { Content = "Some Content" });
                                              frame.Continue = false;
                                              manualEvent.Set();
                                          };
            Thread backgroundThread = new Thread(threadStart);

            backgroundThread.Start();
            Dispatcher.PushFrame(frame);

            manualEvent.WaitOne(1000, false);

            Assert.True(messageOne.Content.Equals("Received"));
        }
    }

    #region Testing Support Infrastructure 

    public class MessageOne
    {
        public string Content { get; set; }
    }

    public class MessageTwo
    {
    }

    public class Subject : IHandle<MessageOne>
    {
        public void Handle(MessageOne message)
        {
            message.Content = "Received";
        }
    }

    #endregion
}
