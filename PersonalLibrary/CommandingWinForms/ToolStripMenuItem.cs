using UILibrary;
using Commanding;

namespace CommandingWinForms
{
    public class ToolStripMenuItem : BindableToolStripMenuItem, ICommandHolder
    {
        private ICommand command;

        #region IControlCommand Members

        public ICommand Command
        {
            get
            {
                return command;
            }
        }

        public void AssignCommand(ICommand command)
        {
            if (command == null)
            {
                return;
            }

            this.command = command;
            DataBindings.Add("Enabled", this.command, "CanExecute");
            Click += (sender, e) => this.command.Execute();
        }

        #endregion
    }
}
