using PropertyChangedNotification;

namespace Commanding
{
    public abstract class Command : NotifyPropertyChangedBase, ICommand
    {
        private bool canExecute;
        private string displayName;
        private string key;

        public Command()
        {
            canExecute = true;
        }

        public string Key
        {
            get
            {
                return key;
            }

            protected set
            {
                key = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                if (displayName != value)
                {
                    displayName = value;
                    RaisePropertyChanged(() => DisplayName);
                }
            }
        }

        #region ICommand Members

        public bool CanExecute
        {
            get
            {
                return canExecute;
            }

            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    RaisePropertyChanged("CanExecute");

                    //TODO: Probleme mit Typ bool. String funktioniert.
                    //RaisePropertyChanged(() => CanExecute);
                }
            }
        }

        public abstract void Execute();

        #endregion
    }
}
