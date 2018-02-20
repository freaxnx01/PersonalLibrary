using System;

namespace Commanding
{
    public class Command<T> : Command
    {
        private T target;
        private Action<T> command;

        public Command(T target, Action<T> command)
        {
            this.target = target;
            this.command = command;
        }

        public Command(T target, Action<T> command, bool canExecute)
            : this(target, command)
        {
            CanExecute = canExecute;
        }

        public override void Execute()
        {
            command(target);
        }
    }
}
