using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Trebuchet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateEntryButtons();
        }

        private void PopulateEntryButtons()
        {
            IEnumerable<IConfigurationSection> entries = App.Configuration
                .GetSection("entries")
                .GetChildren();

            int insertIndex = 2;

            foreach (IConfigurationSection entry in entries)
            {
                string? title = entry["title"];
                string? startStep = entry["startStep"];

                Button button = new() { Content = title, Tag = startStep };

                button.Click += EntryButton_Click;
                ButtonList.Children.Insert(insertIndex++, button);
            }
        }

        private async void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not Button { Tag: string startStepStr }) return;
                if (!int.TryParse(startStepStr, out int stepIndex)) return;

                Hide();
                await RunStepsAsync(stepIndex);
                Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unknown error occurred.\n\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RunStepsAsync(int startStepIndex)
        {
            List<IConfigurationSection> steps = App.Configuration
                .GetSection("steps")
                .GetChildren()
                .ToList();

            int? currentIndex = startStepIndex;

            while (currentIndex.HasValue)
            {
                if (currentIndex.Value < 0 || currentIndex.Value >= steps.Count) break;

                IConfigurationSection step = steps[currentIndex.Value];

                string stepMessage = step["stepMessage"] ?? "Installation in progress...";
                string? relativePath = step["path"];
                int startDelay = int.TryParse(step["startDelay"], out int d) ? d : 0;
                int? nextStep = int.TryParse(step["nextStep"], out int n) ? n : (int?)null;

                if (startDelay > 0)
                    await Task.Delay(startDelay);

                string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath ?? string.Empty);

                InstallerRunningWindow runningDialog = new(stepMessage);

                try
                {
                    Process? process = Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });

                    runningDialog.Show();

                    if (process is not null)
                        await process.WaitForExitAsync();
                }
                catch (Exception ex)
                {
                    runningDialog.AllowClose();
                    runningDialog.Close();
                    MessageBox.Show($"Failed to start installer:\n{ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }

                runningDialog.AllowClose();
                runningDialog.Close();

                if (nextStep.HasValue)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Was the installation successful?",
                        "Installation Complete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result != MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Cannot continue the installation process.",
                            "Installation Stopped",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        break;
                    }
                }

                currentIndex = nextStep;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenAbout_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
