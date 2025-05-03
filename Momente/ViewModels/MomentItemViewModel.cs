using Momente.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.ViewModels
{
    public class MomentItemViewModel : INotifyPropertyChanged
    {
        public MomentItemViewModel(Moment moment)
        {
            _id = moment.Id;
            _createdAt = moment.CreatedAt;
            _icon = moment.Icon;
            _headline = moment.Headline;
            _description = moment.Description;
            _colorString = moment.ColorString;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private DateTime _createdAt;

        public string CreatedAtString
        {
            get => _createdAt.ToString(MauiProgram.DATE_FORMAT_STRING);
            set
            {
                if (_createdAt != DateTime.Parse(value))
                {
                    _createdAt = DateTime.Parse(value);
                    OnPropertyChanged(nameof(CreatedAtString));
                }
            }
        }

        private string _icon = "";

        public string Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        private string _headline = "";

        public string Headline
        {
            get => _headline;
            set
            {
                if (_headline != value)
                {
                    _headline = value;
                    OnPropertyChanged(nameof(Headline));
                }
            }
        }

        private string _description = "";

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string? HasDescriptonIcon { get => string.IsNullOrEmpty(Description) ? null : "📎"; }

        private string _colorString;

        public Color Color
        {
            get => Color.Parse(_colorString);
            set
            {
                if (_colorString != value.ToHex())
                {
                    _colorString = value.ToHex();
                    OnPropertyChanged(nameof(_colorString));
                }
            }
        }
    }
}
