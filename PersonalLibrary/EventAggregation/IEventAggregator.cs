using System.Collections;

namespace EventAggregation
{
    /// <summary>
    /// Contract for the EventAggregator
    /// </summary>
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);
        void Unsubscribe(object subscriber);
        TMessage Publish<TMessage>(TMessage message);
    }
}