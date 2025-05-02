using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Momente
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Moment>? Moments { get; } = [];

        private Color _testColor = Colors.Red;
        public Color TestColor
        {
            get => _testColor;
            set
            {
                if (_testColor != value)
                {
                    _testColor = value;
                    OnPropertyChanged(nameof(TestColor));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
