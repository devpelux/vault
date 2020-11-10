using System;
using System.Windows;

namespace Vault.CustomControls
{
    public class DialogWindow
    {
        private readonly Window window = null;


        public DialogWindow(Window window)
            => this.window = window is IDialogWindow ? window : throw new InvalidCastException($"{window} deve implementare IDialogWindow.");

        public string Show()
        {
            _ = window.ShowDialog();
            return ((IDialogWindow)window).GetResult();
        }
    }
}
