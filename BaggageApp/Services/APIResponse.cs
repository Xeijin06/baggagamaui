using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    [Serializable]
    public class APIResponse
    {
        private bool _successful;
        private string _data;

        public APIResponse()
        {
            this._data = string.Empty;
            this._successful = false;
        }

        public string Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public bool IsSuccessful
        {
            get
            {
                return this._successful;
            }
            set
            {
                this._successful = value;
            }
        }
    }
}
