using Momente.Enums;
using Momente.Models;
using Momente.Resources.Localizations;
using Momente.Services;
using SQLite;
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
            _id = _args.Moment.Id;
            _createdAt = _args.Moment.CreatedAt;
            _icon = args.Moment.Icon;
            _headline = args.Moment.Headline;
            _description = args.Moment.Description;
            _colorString = args.Moment.ColorString;
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
                    await _momentPage.Navigation.PopAsync();
                }
            }
            else
            {
                _args.Action = MomentAction.None;
                await _momentPage.Navigation.PopAsync();
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
                await _momentPage.Navigation.PopAsync();
            }
        }

        private bool ChangesMadeToMoment()
        {
            if (_args.Moment.Icon != Icon)
            { return true; }
            if (_args.Moment.Headline != Headline)
            { return true; }
            if (_args.Moment.Description != Description)
            { return true; }
            if (_args.Moment.ColorString != Color.ToHex())
            { return true; }
            return false;
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
            _args.Moment.ColorString =  _colorString;
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
            await _momentPage.Navigation.PopAsync();
        }
    }
}
