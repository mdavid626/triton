using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.ParameterEditor.Framework;
using Cadmus.Parametrizer;
using Cadmus.Parametrizer.Options;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ParameterViewModel : ViewModelBase
    {
        public Parameter Parameter { get; protected set; }

        public string Title => Parameter?.Title ?? Parameter?.Name;

        public string Description => Parameter?.Description;

        public EditorOptions Editor
        {
            get
            {
                if (Parameter == null || Parameter?.Editor == EditorOptions.NotSet)
                    return EditorOptions.Text;
                return Parameter.Editor;
            }
        }

        public bool IsVisible => true;

        public ParameterViewModel(Parameter parameter)
        {
            Parameter = parameter;
        }

        public DataTemplate GetTemplate()
        {
            var resourceName = Editor + "ParamEditorTemplate";
            return (DataTemplate)App.Current.Resources[resourceName];
        }
    }
}
