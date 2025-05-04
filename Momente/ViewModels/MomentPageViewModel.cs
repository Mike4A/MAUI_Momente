using Momente.Enums;
using Momente.Models;
using Momente.Resources.Localizations;
using Momente.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace Momente.ViewModels
{
    public class MomentPageViewModel : INotifyPropertyChanged
    {
        public MomentPageViewModel(MomentPage momentPage, MomentPageArgs args)
        {
            _momentPage = momentPage;
            _args = args;
            _id = args.Moment.Id;
            _createdAt = args.Moment.CreatedAt;
            _icon = args.Moment.Icon;
            _headline = args.Moment.Headline;
            _description = args.Moment.Description;
            _color = Color.Parse(args.Moment.ColorString);
            DeleteButtonCommand = new Command(async () => await DeleteButton_Clicked());
            CancelButtonCommand = new Command(async () => await CancelButton_Clicked());
            SaveButtonCommand = new Command(async () => await SaveButton_Clicked());
        }


        private MomentPage _momentPage;

        private MomentPageArgs _args;

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

        private Color _color;

        public Color Color
        {
            get => _color;
            set
            {
                if (ColorService.IsColorSimilar(_color, value))
                { return; }
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public ICommand DeleteButtonCommand { get; }

        private async Task DeleteButton_Clicked()
        {
            await Task.Delay(300);
            if (Id != 0)
            {
                if (await _momentPage.DisplayAlert("", AppResources.DeleteMomentQuestion, AppResources.Yes, AppResources.No))
                {
                    await DatabaseService.Instance.DeleteMomentAsync(Id);
                    _args.Action = MomentAction.Deleted;
                    await _momentPage.Navigation.PopModalAsync();
                }
            }
            else
            {
                _args.Action = MomentAction.None;
                await _momentPage.Navigation.PopModalAsync();
            }
        }

        public ICommand CancelButtonCommand { get; }

        private async Task CancelButton_Clicked()
        {
            await Task.Delay(300);
            _args.Action = MomentAction.None;
            //Already tried to do this on NavigatedFrom event too, but it fails due to not being able to cancel the navigation and DB sync props
            if (ChangesMadeToMoment() && await _momentPage.DisplayAlert("", AppResources.SaveMomentQuestion, AppResources.Yes, AppResources.No))
            {
                await SaveChangesAndPop();
            }
            else
            {
                await _momentPage.Navigation.PopModalAsync();
            }
        }

        private bool ChangesMadeToMoment()
        {
            return _args.Moment.Icon != Icon ||
                   _args.Moment.Headline != Headline ||
                   _args.Moment.Description != Description ||
                   !ColorService.IsColorSimilar(Color.Parse(_args.Moment.ColorString), Color);
        }

        public ICommand SaveButtonCommand { get; }

        private async Task SaveButton_Clicked()
        {
            await Task.Delay(300);
            await SaveChangesAndPop();
        }

        private async Task SaveChangesAndPop()
        {
            _args.Moment.Id = _id;
            _args.Moment.CreatedAt = _createdAt;
            _args.Moment.Icon = _icon;
            _args.Moment.Headline = _headline;
            _args.Moment.Description = _description;
            _args.Moment.ColorString = _color.ToHex();
            if (await DatabaseService.Instance.GetMomentByIdAsync(_args.Moment.Id) != null)
            {
                await DatabaseService.Instance.UpdateMomentAsync(_args.Moment);
                _args.Action = MomentAction.Updated;
            }
            else
            {
                await DatabaseService.Instance.AddMomentAsync(_args.Moment);
                _args.Action = MomentAction.Created;
            }
            await _momentPage.Navigation.PopModalAsync();
        }
    }
}
