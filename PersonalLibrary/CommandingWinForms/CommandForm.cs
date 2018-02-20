using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

using Extensions;
using Commanding;
using ObjectGraphML;

namespace CommandingWinForms
{
    public class CommandForm : Form
    {
        private Dictionary<string, ICommand> commands = null;

        public CommandForm()
        {
            MethodInfo mi = typeof(CommandForm).GetMethod("RegisterCommands", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            mi = mi.MakeGenericMethod(this.GetType());
            mi.Invoke(this, new object[] { this });
        }

        public Dictionary<string, ICommand> Commands
        {
            get
            {
                return commands;
            }
        }

        private void RegisterCommands<T>(T instance)
        {
            var commandMethods = from m in instance.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                 where m.GetCustomAttributes(typeof(CommandAttribute), false).Count() > 0
                                 orderby m.Name
                                 select new { Method = m, Attribute = (CommandAttribute)m.GetCustomAttributes(typeof(CommandAttribute), false)[0] };

            commands = new Dictionary<string, ICommand>();

            foreach (var item in commandMethods)
            {
                string commandKey = item.Method.Name;
                if (!item.Attribute.Name.IsNullOrEmpty())
                {
                    commandKey = item.Attribute.Name;
                }

                Type genericType = typeof(Action<>).MakeGenericType(typeof(T));
                Action<T> command = (Action<T>)Delegate.CreateDelegate(genericType, item.Method);
                commands.Add(commandKey, new Command<T>(instance, a => ExecuteCommand<T>(instance, command)));
            }
        }

        // Für manuelle Registrierung von Commands
        //protected void RegisterCommand<T>(string commandKey, Action<T> command, T target)
        //{
        //    commands.Add(commandKey, new Command<T>(this, a => ExecuteCommand<T>(target, command)));
        //}

        private void ExecuteCommand<T>(T target, Action<T> command)
        {
            command(target);
            SetCommandEnablement();
        }

        protected virtual void SetCommandEnablement() {}

        protected void RenderControl(Control containerControl, string definitionXmlFile)
        {
            Engine engine = new Engine(Commands);
            engine.RenderControl(containerControl, definitionXmlFile, this);
            SetCommandEnablement();
        }
    }
}
