using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cadmus.ParameterEditor.ViewModels;
using Cadmus.Parametrizer;
using Cadmus.Parametrizer.Options;

namespace Cadmus.ParameterEditor.Views
{
    public class ParameterEditorSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return ((ParameterViewModel)item)?.GetTemplate();
        }
    }
}
