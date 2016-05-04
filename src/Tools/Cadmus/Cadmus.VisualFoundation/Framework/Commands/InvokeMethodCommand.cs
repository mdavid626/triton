using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework.Commands
{
    public class InvokeMethodCommand : CommandBase
    {
        public INotifyPropertyChanged Target { get; set; }

        public MethodInfo Method { get; set; }

        public object[] Parameters { get; set; }

        public string CanExecutePropertyName { get; protected set; }

        public PropertyInfo CanExecuteProperty { get; protected set; }

        public InvokeMethodCommand(INotifyPropertyChanged target, string method, params object[] parameters)
            : this(target, target.GetType().GetMethod(method), parameters)
        {

        }

        public InvokeMethodCommand(INotifyPropertyChanged target, MethodInfo method, params object[] parameters)
        {
            if (method == null)
                throw new ArgumentException($"Method on {target} not found!");

            Target = target;
            Method = method;
            Parameters = parameters;
            target.PropertyChanged += Target_PropertyChanged;
            CanExecutePropertyName = $"Can{method.Name}";
            CanExecuteProperty = Target.GetType().GetProperty(CanExecutePropertyName);
        }

        protected virtual void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CanExecutePropertyName)
            {
                OnPropertyChanged(nameof(CanExecute));
            }
        }

        public override bool CanExecute => true.Equals(CanExecuteProperty?.GetValue(Target));

        public override void Execute()
        {
            Method.Invoke(Target, Parameters);
        }
    }
}
