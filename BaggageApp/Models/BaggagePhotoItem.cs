using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class BaggagePhotoItem
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string LocalFile { get; set; }
        public string RemoteFile { get; set; }

    }
}
