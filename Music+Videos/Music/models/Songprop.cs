using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Music.models
{
    public class Songprop
    {
        public int ID { get; set; }
        public string songtitle { get; set; }
        public string songartist { get; set; }
        public string songalbum { get; set; }
        public string songname { get; set; }
        public string key { get; set; }
        public StorageFile songfile { get; set; }
        public BitmapImage albumcover;
        public StorageFolder folder { get; set; }
        public bool nextsong { get; set; }
        public bool selected { get; set; }
      


                


            }




        
    }

