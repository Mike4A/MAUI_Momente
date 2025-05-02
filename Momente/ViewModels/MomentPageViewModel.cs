using System.ComponentModel;

namespace Momente.ViewModels
{
    public class MomentPageViewModel : INotifyPropertyChanged
    {
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
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
            }
        }

        private string? _createdAtString;
        public string CreatedAtString
        {
            get => _createdAtString!;
            set
            {
                _createdAtString = value;
                OnPropertyChanged(nameof(CreatedAtString));
            }
        }
        private string _icon = "";
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        private string _headline = "";
        public string Headline
        {
            get => _headline;
            set
            {
                _headline = value;
                OnPropertyChanged(nameof(Headline));
            }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private Color _color = MauiProgram.MOMENT_DEFAULT_COLOR;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
                OnPropertyChanged(nameof(GlowColor));
            }
        }
        public Color GlowColor
        {
            get => Color.WithLuminosity(Color.GetLuminosity() + MauiProgram.MOMENT_LUMINOSITY_GLOW);
        }
        public Color ShadowColor
        {
            get => Color.WithLuminosity(Color.GetLuminosity() + MauiProgram.MOMENT_LUMINOSITY_SHADOW);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
