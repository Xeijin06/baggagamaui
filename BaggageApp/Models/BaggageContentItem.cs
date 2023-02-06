using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class BaggageContentItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        string _contentDescription;
        public string ContentDescription
        {
            get
            {
                return _contentDescription;
            }
            set
            {
                _contentDescription = value;
                OnPropertyChanged(nameof(ContentDescription));
            }
        }

        int _categoryWeight;
        public int CategoryWeight
        {
            get
            {
                return _categoryWeight;
            }
            set
            {
                _categoryWeight = value;
                OnPropertyChanged(nameof(CategoryWeight));
            }
        }

        string _categoryId;
        public string CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }
    }
}
