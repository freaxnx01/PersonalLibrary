namespace EventAggregation
{
    /// <summary>
    /// Marks the implementer as a message handler for the used type. Can 
    /// be implemented multiple times.
    /// </summary>
    /// <typeparam name="TMessage">The messagetype to handle.</typeparam>
    public interface IHandle<TMessage>
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The concrete message.</param>
        void Handle(TMessage message);
    }
}