using System.ComponentModel;
using System.Windows;

namespace Trebuchet
{
    public partial class InstallerRunningWindow : Window
    {
        private bool _canClose = false;

        public InstallerRunningWindow(string stepMessage)
        {
            InitializeComponent();
            StepMessageText.Text = stepMessage;
        }

        public void AllowClose() => _canClose = true;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_canClose)
                e.Cancel = true;

            base.OnClosing(e);
        }
    }
}
