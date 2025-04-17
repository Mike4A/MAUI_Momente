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
    public class MainViewModel //: INotifyPropertyChanged
    {
        public ObservableCollection<Moment>? Moments { get; } = new ObservableCollection<Moment>();
    }
}
