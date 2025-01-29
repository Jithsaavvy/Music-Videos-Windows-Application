// Class to store the video properities

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Music
{
    public class Videoprop
    {
        public int ID { get; set; }
        public string videotitle { get; set; }
        public string videoartist { get; set; }
        public string videoalbum { get; set; }
        public string videoname { get; set; }
        public bool nextvideo { get; set; }
        public StorageFile Name { get; set; }
        public StorageFile videofile { get; set; }
        public BitmapImage albumcover;
        public StorageFolder folder { get; set; }
        public bool selected { get; set; }
    }

}
