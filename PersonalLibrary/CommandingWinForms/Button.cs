using Commanding;

namespace CommandingWinForms
{
    public class Button : System.Windows.Forms.Button, ICommandHolder
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
            this.command = command;
            DataBindings.Add("Enabled", this.command, "CanExecute");
            Click += (sender, e) => this.command.Execute();
        }

        #endregion
    }
}
