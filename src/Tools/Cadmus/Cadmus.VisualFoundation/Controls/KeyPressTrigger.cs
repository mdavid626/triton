using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Cadmus.VisualFoundation.Controls
{
    public enum KeyEventAction
    {
        KeyUp,
        KeyDown
    }

    public class KeyPressTrigger : TriggerBase<FrameworkElement>
    {
        public static readonly DependencyProperty TriggerValueProperty = DependencyProperty.Register("TriggerValue", typeof(Key), typeof(KeyPressTrigger), new PropertyMetadata(null));
        public static readonly DependencyProperty KeyActionProperty = DependencyProperty.Register("KeyAction", typeof(KeyEventAction), typeof(KeyPressTrigger), new PropertyMetadata(null));
        public static readonly DependencyProperty ModifierKeysProperty = DependencyProperty.Register("ModifierKeys", typeof(ModifierKeys), typeof(KeyPressTrigger), new PropertyMetadata());

        [Category("KeyPress Properties")]
        public Key TriggerValue
        {
            get { return (Key)GetValue(TriggerValueProperty); }
            set { SetValue(TriggerValueProperty, value); }
        }

        [Category("KeyPress Properties")]
        public KeyEventAction KeyEvent
        {
            get { return (KeyEventAction)GetValue(KeyActionProperty); }
            set { SetValue(KeyActionProperty, value); }
        }

        public ModifierKeys ModifierKeys
        {
            get { return (ModifierKeys)GetValue(ModifierKeysProperty); }
            set { SetValue(ModifierKeysProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (KeyEvent == KeyEventAction.KeyUp)
            {
                AssociatedObject.KeyUp += OnKeyPress;
            }
            else if (KeyEvent == KeyEventAction.KeyDown)
            {
                AssociatedObject.KeyDown += OnKeyPress;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (KeyEvent == KeyEventAction.KeyUp)
            {
                AssociatedObject.KeyUp -= OnKeyPress;
            }
            else if (KeyEvent == KeyEventAction.KeyDown)
            {
                AssociatedObject.KeyDown -= OnKeyPress;
            }
        }

        private void OnKeyPress(object sender, KeyEventArgs args)
        {
            if (Keyboard.Modifiers == ModifierKeys && args.Key.Equals(TriggerValue))
            {
                InvokeActions(null);
                args.Handled = true;
            }
        }
    }
}
