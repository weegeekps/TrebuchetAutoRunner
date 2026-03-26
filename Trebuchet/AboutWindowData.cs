using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Trebuchet
{
    public class AboutWindowData : INotifyPropertyChanged
    {
        // We take most everything from the assembly info, so we don't have to hardcode it here.

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Copyright
        {
            get;
            set
            {
                if (field != value)
                {
                    // Convert any commas in the copyright statement to newlines.
                    field = value.Replace(", ", Environment.NewLine);
                    OnPropertyChanged();
                }
            }
        } = string.Empty;

        public string RepositoryUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = string.Empty;

        public string Version
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = string.Empty;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
