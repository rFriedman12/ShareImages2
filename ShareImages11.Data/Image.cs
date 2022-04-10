using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareImages11.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTimeUploaded { get; set; }
        public int Likes { get; set; }
    }
}
