using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BaggageApp.Models
{
    class BaggageDamageItem : INotifyPropertyChanged
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

        string _damageType;
        public string DamageType
        {
            get { return _damageType; }
            set
            {
                _damageType = value;
                OnPropertyChanged(nameof(DamageType));
            }
        }
        string _damageLocation;
        public string DamageLocation
        {
            get
            {
                return _damageLocation;
            }
            set
            {
                _damageLocation = value;
                OnPropertyChanged(nameof(DamageLocation));
            }
        }

        string _damageSeverity;
        public string DamageSeverity
        {
            get
            {
                return _damageSeverity;
            }
            set
            {
                _damageSeverity = value;
                OnPropertyChanged(nameof(DamageSeverity));
            }
        }

        private string _damageTypeCode;
        public string DamageTypeCode
        {
            get
            {
                return _damageTypeCode;
            }
            set
            {
                _damageTypeCode = value;
                OnPropertyChanged(nameof(DamageTypeCode));
            }
        }
    }
}
