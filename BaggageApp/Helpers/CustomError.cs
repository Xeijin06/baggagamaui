using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    [Serializable]
    public class CustomError
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public CustomError()
        {
            this.TimeStamp = DateTime.Now;
        }

        public CustomError(string Message) : this()
        {
            this.Message = Message;
        }

        public CustomError(System.Exception ex) : this(ex.Message)
        {
            this.StackTrace = ex.StackTrace;
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}
