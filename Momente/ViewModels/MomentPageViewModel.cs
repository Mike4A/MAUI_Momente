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
            _id = _args.Moment.Id;
            _createdAtString = args.Moment.CreatedAtString;
            _icon = args.Moment.Icon;
            _headline = args.Moment.Headline;
            _description = args.Moment.Description;
            _color = args.Moment.Color;
            DeleteButtonCommand = new Command(async () => await DeleteButton_Clicked());
            DeleteButtonCommand = new Command(async () => await CancelButton_Clicked());
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

        public ICommand SaverButtonCommand { get; }

        private async Task SaveButton_Clicked()
        {
            await SaveChangesAndPop();
        }
        private async Task SaveChangesAndPop()
        {
            _args.Moment.Id = Id;
            _args.Moment.CreatedAtString = CreatedAtString;
            _args.Moment.Icon = Icon;
            _args.Moment.Headline = Headline;
            _args.Moment.Description = Description;
            _args.Moment.Color = Color;
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
