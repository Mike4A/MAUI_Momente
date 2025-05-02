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
            _icon = moment.Icon;
            _createdAtString = moment.CreatedAtString;
            _headline = moment.Headline;
            _description = moment.Description;
            _color = moment.Color;
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

        private string? _icon;
        public string? Icon
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

        private string? _createdAtString;
        public string? CreatedAtString
        {
            get => _createdAtString;
            set
            {
                if (_createdAtString != value)
                {
                    _createdAtString = value;
                    OnPropertyChanged(nameof(CreatedAtString));
                }
            }
        }

        private string? _headline;
        public string? Headline
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

        private string? _description;
        public string? Description
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

        private string? _hasDescriptionIcon;
        public string? HasDescriptionIcon
        {
            get => _hasDescriptionIcon;
            set
            {
                if (_hasDescriptionIcon != value)
                {
                    _hasDescriptionIcon = value;
                    OnPropertyChanged(nameof(HasDescriptionIcon));
                }
            }
        }

        private Color? _color;

        public Color? Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }
    }
}
