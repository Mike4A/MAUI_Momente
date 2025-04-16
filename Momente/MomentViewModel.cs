using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente
{
    public class MomentViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    OnPropertyChanged(nameof(Id));
                    _id = value;
                }
            }
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                OnPropertyChanged(nameof(CreatedAt));
                _createdAt = value;
            }
        }

        private string _icon = "";
        public string Icon
        {
            get => _icon;
            set
            {
                OnPropertyChanged(nameof(Icon));
                _icon = value;
            }
        }

        private string _headline = "";
        public string Headline
        {
            get => _headline;
            set
            {
                OnPropertyChanged(nameof(Headline));
                _headline = value;
            }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                OnPropertyChanged(nameof(Description));
                _description = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
