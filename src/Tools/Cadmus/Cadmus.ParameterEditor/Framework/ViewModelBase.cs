using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;
using Cadmus.Foundation.Metadata;
using Cadmus.VisualFoundation;
using Cadmus.VisualFoundation.Framework;
using Cadmus.VisualFoundation.Framework.Commands;
using Caliburn.Micro;

namespace Cadmus.ParameterEditor.Framework
{
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        public virtual BindableCollection<CommandViewModel> CreateCoroutineOperations()
        {
            var cust = new BindableCollection<CommandViewModel>();
            foreach (var m in GetType().GetMethods())
            {
                var op = m.GetCustomAttribute<OperationAttribute>();
                if (op != null)
                {
                    var com = new CommandViewModel(new InvokeMethodCommand(this, m))
                    {
                        Title = op.Title,
                        Description = op.Description
                    };
                    cust.Add(com);
                }
            }
            return cust;
        }
    }
}
