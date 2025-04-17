using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Momente
{
    public class MomentViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SaveMomentCommand { get; }
        public ICommand DeleteMomentCommand { get; }

        public MomentViewModel()
        {
            SaveMomentCommand = new Command(async () => await SaveMomentAsync());
            DeleteMomentCommand = new Command(async () => await DeleteMomentAsync());
        }

        private async Task SaveMomentAsync()
        {
            Moment moment = new Moment
            {
                Id = this.Id,
                CreatedAt = this.CreatedAt,
                Icon = this.Icon,
                Headline = this.Headline,
                Description = this.Description
            };

            if (DatabaseService.Instance.GetMomentByIdAsync(Id) != null)
            {
                await DatabaseService.Instance.UpdateMomentAsync(moment);
            }
            else
            {
                await DatabaseService.Instance.AddMomentAsync(moment);
            }

            await Debugger.WriteMomentEntries();
        }
        private async Task DeleteMomentAsync()
        {
            //await Debugger.WriteMomentEntries();
        }

        /// <param name="id">0 means "load default"</param>
        public async Task<bool> TryLoadFromIdOrDefaultAsync(int id)
        {
            Moment? moment;
            if (id == 0)
            {
                moment = new Moment();
            }
            else
            {
                moment = await DatabaseService.Instance.GetMomentByIdAsync(id);
            }

            if (moment == null)
            {
                return false;
            }

            Id = moment.Id;
            CreatedAt = moment.CreatedAt;
            Icon = moment.Icon;
            Headline = moment.Headline;
            Description = moment.Description;

            return true;
        }
    }
}
