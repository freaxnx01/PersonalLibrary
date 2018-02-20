using Commanding;

namespace Commanding
{
    public interface ICommandHolder
    {
        ICommand Command { get; }
        void AssignCommand(ICommand command);
    }
}
