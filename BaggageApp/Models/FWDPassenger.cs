using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class FWDPassenger : ObservableObject
    {
        private bool _isLast;
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Sequential { get; set; }
        public bool IsLast
        {
            get
            {
                return _isLast;
            }
            set
            {
                SetProperty(ref _isLast, value);
            }
        }
        public string FullName
        {
            get
            {
                return GetFullName();
            }
        }
        private string GetFullName()
        {
            return string.Format("{0}, {1}", LastName, Name);
        }
    }
}
