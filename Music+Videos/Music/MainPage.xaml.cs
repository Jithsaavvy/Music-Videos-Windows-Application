using Music.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Search;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;
using System.Collections;
using Windows.UI.Notifications;
using Windows.UI.Core;
using Windows.UI;
using NotificationsExtensions.Tiles;
using Windows.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Music
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Songprop> Songs;
        private ObservableCollection<Videoprop> Videos;
        private ObservableCollection<StorageFile> allData;
        private ObservableCollection<StorageFile> allviddata;      
        int id = 0;

        public MainPage()
        {
            this.InitializeComponent();

            Songs = new ObservableCollection<Songprop>();
            Videos = new ObservableCollection<Videoprop>();
            backbtn.Visibility = Visibility.Collapsed;
            
        }

        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            mysplitview.IsPaneOpen = !mysplitview.IsPaneOpen;
           
        }

        //for videos

        private async Task retreivevideodata(ObservableCollection<StorageFile> myvideos, StorageFolder parentfolder)
        {
            foreach (var item in await parentfolder.GetFilesAsync())
            {
                if (item.FileType == ".mp4" || item.FileType == ".avi" || item.FileType == ".vob")
                {
                    myvideos.Add(item);
                }
            }

            foreach (var item in await parentfolder.GetFoldersAsync())
            {
                await retreivevideodata(myvideos, item);
            }
        }

        private async Task displayvideos(ObservableCollection<StorageFile> dispvideo)
        {
            foreach (var file in dispvideo)
            {

                VideoProperties property = await file.Properties.GetVideoPropertiesAsync();
                StorageItemThumbnail thumbb = await file.GetThumbnailAsync(ThumbnailMode.VideosView, 230, ThumbnailOptions.ResizeThumbnail);
                var videoname = file.Name;
                var coverimage = new BitmapImage();
                coverimage.SetSource(thumbb);
                var conuttt = dispvideo.Count();

                var video = new Videoprop();
                totalvideono.Text = string.Format("Total Videos: {0}", conuttt);
                video.ID = id;
                video.videoname = videoname;
                video.videotitle = property.Title;
                video.videoartist = property.Subtitle;
                video.videoalbum = property.Publisher;
                video.albumcover = coverimage;
                video.videofile = file;
                Videos.Add(video);
                id++;
            }
        }

        public async Task<ObservableCollection<StorageFile>> setvideofiles()
        {
            StorageFolder folder = KnownFolders.VideosLibrary;
            var alldata = new ObservableCollection<StorageFile>();
            await retreivevideodata(alldata, folder);

            return alldata;
        }

        private async Task displayv()
        {
            await displayvideos(allviddata);

        }


        //For songs
        private async Task retreivedata(ObservableCollection<StorageFile> mysongs, StorageFolder parentfolder)
        {
            foreach (var item in await parentfolder.GetFilesAsync())
            {
                if (item.FileType == ".mp3" || item.FileType==".wav")
                {
                    mysongs.Add(item);
                }

            }

            foreach (var item in await parentfolder.GetFoldersAsync())
            {
                await retreivedata(mysongs, item);
            }
        }

        private async Task displaysongs(ObservableCollection<StorageFile> dispsongs)
        {
            foreach (var file in dispsongs)
            {
                MusicProperties property = await file.Properties.GetMusicPropertiesAsync();
                StorageItemThumbnail thumb = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 230, ThumbnailOptions.ResizeThumbnail);
                var name = file.Name;
                var countt = dispsongs.Count();
                var coverimage = new BitmapImage();
                coverimage.SetSource(thumb);
               
                totalsongsno.Text = string.Format("Total Songs: {0}",countt);
                var song = new Songprop();
                song.ID = id;

                song.songname = name;
                song.songtitle = property.Title;
                song.songartist = property.Artist;
                song.songalbum = property.Album;
                song.albumcover = coverimage;
                song.songfile = file;
                Songs.Add(song);
                id++;
            }
        }

        public async Task<ObservableCollection<StorageFile>> setsongsfiles()
        {
            StorageFolder folder = KnownFolders.MusicLibrary;
            var alldata = new ObservableCollection<StorageFile>();
            await retreivedata(alldata, folder);

            return alldata;
        }


        private async Task displayy()
        {
            await displaysongs(allData);

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Allsongs.IsSelected)
            {
                mytitleblock.Text = "All songs";
                musicxbox.Visibility = Visibility.Visible;
                musicpanel.Visibility = Visibility.Visible;
                myscroll.Visibility = Visibility.Visible;
                myframe.Visibility = Visibility.Collapsed;
                videogrid.Visibility = Visibility.Collapsed;
                videoscroll.Visibility = Visibility.Collapsed;
                videopanel.Visibility = Visibility.Collapsed;
                backbtn.Visibility = Visibility.Visible;

            }
            else if (video.IsSelected)
            {
                musicxbox.Visibility = Visibility.Collapsed;
                myscroll.Visibility = Visibility.Collapsed;
                musicpanel.Visibility = Visibility.Collapsed;
                videogrid.Visibility = Visibility.Visible;
                videoscroll.Visibility = Visibility.Visible;
                videopanel.Visibility = Visibility.Visible;

                mytitleblock.Text = "Videos";

                backbtn.Visibility = Visibility.Visible;
                myframe.Visibility = Visibility.Collapsed;
            }

            else if (about.IsSelected)
            {
                myframe.Navigate(typeof(AboutPage));
                musicxbox.Visibility = Visibility.Collapsed;
                myscroll.Visibility = Visibility.Collapsed;
                musicpanel.Visibility = Visibility.Collapsed;
                videogrid.Visibility = Visibility.Collapsed;
                videoscroll.Visibility = Visibility.Collapsed;
                videopanel.Visibility = Visibility.Collapsed;
                myframe.Visibility = Visibility.Visible;
                mytitleblock.Text = "About Me";
                backbtn.Visibility = Visibility.Visible;
            }
        }      

        private void goback()
        {
            mytitleblock.Text = "Home";
            videogrid.Visibility = Visibility.Collapsed;
            musicxbox.Visibility = Visibility.Visible;
            musicpanel.Visibility = Visibility.Visible;
            myscroll.Visibility = Visibility.Visible;
            myframe.Visibility = Visibility.Collapsed;

            backbtn.Visibility = Visibility.Collapsed;
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            goback();
          
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            myprogressring.IsActive = true;
            pro2.IsActive = true;
            mytitleblock.Text = "Loading,Please wait";
            myprogressring.Visibility = Visibility.Visible;
            allData = await setsongsfiles();
           

            myprogressring.IsActive = false;
            await displayy();
            pro2.IsActive = false;
            myprogressring.Visibility = Visibility.Collapsed;
            pro2.Visibility = Visibility.Collapsed;
            mytitleblock.Text = "Welcome";

        } 
    
        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            var song = new Songprop();
            var playesouns = (Songprop)e.ClickedItem;
            var images = playesouns.albumcover;                        
            var name = playesouns.songartist;
            var names = playesouns.songname;
            newelements.AutoPlay = true;
            newelements.PosterSource = images;


            mygridview.ScrollIntoView(playesouns);
            mygridview.SelectedItem = true;

            newelements.TransportControls.IsStopEnabled = true;
            newelements.TransportControls.IsStopButtonVisible = true;
            newelements.TransportControls.IsFastForwardButtonVisible = true;
            newelements.TransportControls.IsFastForwardEnabled = true;
            newelements.TransportControls.IsFastRewindButtonVisible = true;
            newelements.TransportControls.IsFastRewindEnabled = true;
            newelements.TransportControls.IsPlaybackRateEnabled = true;
            newelements.TransportControls.IsPlaybackRateButtonVisible = true;
         
            newelements.SetSource(await playesouns.songfile.OpenAsync(FileAccessMode.Read), playesouns.songfile.ContentType);
            newelements.Play();
            remaintxt.Text = string.Format("Now Playing: {0}", names);

            resultetxtblock.Text = names;
            tiletxt.Text = name;


            var tilexml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText02);
           
            var tileattr = tilexml.GetElementsByTagName("text");
            tileattr[0].AppendChild(tilexml.CreateTextNode(resultetxtblock.Text));
            tileattr[1].AppendChild(tilexml.CreateTextNode(tiletxt.Text));
            var tilenotify = new TileNotification(tilexml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotify);

            var tiles = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150IconWithBadgeAndText);
            var tileatr = tiles.GetElementsByTagName("text");
            tileatr[0].AppendChild(tiles.CreateTextNode(resultetxtblock.Text));
            tileatr[1].AppendChild(tiles.CreateTextNode(tiletxt.Text));
            var tilenotifyy = new TileNotification(tiles);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotifyy);

            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            xml.DocumentElement.SetAttribute("launch", "Args");
            var text = xml.CreateTextNode(resultetxtblock.Text);
            var elements = xml.GetElementsByTagName("text");
            elements[0].AppendChild(text);
            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);


        }

        public void playmusic()
        {
            
            newelements.Play();


        }

        public void pausemusic()
        {
            newelements.Pause();
        }

        public void stopmusic()
        {
            newelements.Stop();
        }
        private void play_Click(object sender, RoutedEventArgs e)
        {
           
            newelements.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            newelements.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            newelements.Stop();

        }

       
        private Songprop picknextsong()
        {
            var currentindex = -1;       
            var nextsong = Songs.Where(p => p.nextsong == false);
            currentindex = mygridview.SelectedIndex + 1;
            var randsong = nextsong.ElementAt(currentindex);
            randsong.selected = true;
            var songname = randsong.songtitle;
            remaintxt.Text = string.Format("Now Playing: {0}", songname);
            return randsong;
        }       

        private async void newelements_MediaEnded(object sender, RoutedEventArgs e)
        {
            var song = picknextsong();            
            newelements.SetSource(await song.songfile.OpenAsync(FileAccessMode.Read),song.songfile.ContentType);
            newelements.Play();
        }
            
            

        private void newelements_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Drag and Drop inside to play the media";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private  async void newelements_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();

                if (items.Any())
                {
                    var storagefile = items[0] as StorageFile;
                    var contenttype = storagefile.ContentType;
                    var name = storagefile.Name;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    remaintxt.Text = string.Format("Now Playing: {0}",name);
                    


                    if (contenttype == "audio/mpeg" || contenttype == "audio/wav" || contenttype=="video/mp4" || contenttype=="video/avi" || contenttype=="video/vob")
                    {
                        newelements.AutoPlay = true;
                        StorageFile newfile = await storagefile.CopyAsync(folder, storagefile.Name, NameCollisionOption.GenerateUniqueName);
                        newelements.SetSource(await storagefile.OpenAsync(FileAccessMode.Read), contenttype);
                        newelements.Play();           
                                    
                        var tilexml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text02);
                        var tileattr = tilexml.GetElementsByTagName("text");
                        tileattr[0].AppendChild(tilexml.CreateTextNode(resultetxtblock.Text));
                        tileattr[1].AppendChild(tilexml.CreateTextNode(tiletxt.Text));
                        var tilenotify = new TileNotification(tilexml);
                        TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotify);

                        var template = ToastTemplateType.ToastText01;
                        var xml = ToastNotificationManager.GetTemplateContent(template);
                        xml.DocumentElement.SetAttribute("launch", "Args");
                        var text = xml.CreateTextNode(resultetxtblock.Text);
                        var elements = xml.GetElementsByTagName("text");
                        elements[0].AppendChild(text);
                        var toast = new ToastNotification(xml);
                        var notifier = ToastNotificationManager.CreateToastNotifier();
                        notifier.Show(toast);



                    }
                }
            }
        }

        private async void myimport_Click(object sender, RoutedEventArgs e)
        {
            var fileopen = new FileOpenPicker();
            fileopen.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            fileopen.FileTypeFilter.Add(".mp3");
            fileopen.FileTypeFilter.Add(".wav");
            fileopen.FileTypeFilter.Add(".mp4");
            fileopen.FileTypeFilter.Add(".avi");
            fileopen.FileTypeFilter.Add(".vob");

            newelements.AutoPlay = true;
            var file = await fileopen.PickSingleFileAsync();
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var filename = file.Name;
            var filetype = file.FileType;
            if(filetype==".mp4" || filetype==".avi" || filetype==".mp3" || filetype==".wav" || filetype==".vob")
            {
                newelements.SetSource(stream, file.ContentType);
                newelements.IsFullWindow = !newelements.IsFullWindow;
                newelements.TransportControls.IsStopEnabled = true;
                newelements.TransportControls.IsStopButtonVisible = true;
                newelements.TransportControls.IsFastForwardButtonVisible = true;
                newelements.TransportControls.IsFastForwardEnabled = true;
                newelements.TransportControls.IsFastRewindButtonVisible = true;
                newelements.TransportControls.IsFastRewindEnabled = true;
            }

            newelements.SetSource(stream, file.ContentType);
            newelements.TransportControls.IsStopEnabled = true;
            newelements.TransportControls.IsStopButtonVisible = true;
            newelements.TransportControls.IsFastForwardButtonVisible = true;
            newelements.TransportControls.IsFastForwardEnabled = true;
            newelements.TransportControls.IsFastRewindButtonVisible = true;
            newelements.TransportControls.IsFastRewindEnabled = true;

            newelements.Play();
            remaintxt.Text = string.Format("Now Playing: {0}",filename);
            resultetxtblock.Text = filename;
            presstxt.Visibility = Visibility.Collapsed;
           

            var tilexml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150IconWithBadgeAndText);
            var tileattr = tilexml.GetElementsByTagName("text");
            tileattr[0].AppendChild(tilexml.CreateTextNode(resultetxtblock.Text));
            tileattr[1].AppendChild(tilexml.CreateTextNode(tiletxt.Text));
            var tilenotify = new TileNotification(tilexml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotify);

            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            xml.DocumentElement.SetAttribute("launch", "Args");
            var text = xml.CreateTextNode(remaintxt.Text);
            var elements = xml.GetElementsByTagName("text");
            elements[0].AppendChild(text);
            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

        }        

        private async void videogrid_Loaded(object sender, RoutedEventArgs e)
        {
            videoprogressring.IsActive = true;
            pro2.IsActive = true;
            mytitleblock.Text = "Loading,Please Wait";

            videoprogressring.Visibility = Visibility.Visible;
            allviddata = await setvideofiles();

            videoprogressring.IsActive = false;
            await displayv();
            pro2.IsActive = false;
            myprogressring.Visibility = Visibility.Collapsed;
            pro2.Visibility = Visibility.Collapsed;
            mytitleblock.Text = "Welcome";
        }

        private async void videogridview_ItemClick(object sender, ItemClickEventArgs e)
        {

            var video = new Videoprop();
            var playesouns = (Videoprop)e.ClickedItem;
            var name = playesouns.videoartist;
            var names = playesouns.videoname;
            newelements.AutoPlay = true;

            newelements.SetSource(await playesouns.videofile.OpenAsync(FileAccessMode.Read), playesouns.videofile.ContentType);
            newelements.TransportControls.IsStopEnabled = true;
           
            newelements.TransportControls.IsStopButtonVisible = true;
            newelements.TransportControls.IsFastForwardButtonVisible = true;
            newelements.TransportControls.IsFastForwardEnabled = true;
            newelements.TransportControls.IsFastRewindButtonVisible = true;
            newelements.TransportControls.IsFastRewindEnabled = true;
            newelements.TransportControls.IsPlaybackRateEnabled = true;
            newelements.TransportControls.IsPlaybackRateButtonVisible = true;
            newelements.Play();
            // newelements.Height = 300;
            newelements.IsFullWindow = !newelements.IsFullWindow;
            remaintxt.Text = string.Format("Now Playing: {0}", names);

            resultetxtblock.Text = names;
            tiletxt.Text = name;

            var tilexml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150IconWithBadgeAndText);

            var tileattr = tilexml.GetElementsByTagName("text");
            tileattr[0].AppendChild(tilexml.CreateTextNode(resultetxtblock.Text));
            tileattr[1].AppendChild(tilexml.CreateTextNode(tiletxt.Text));
            var tilenotify = new TileNotification(tilexml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tilenotify);
            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            xml.DocumentElement.SetAttribute("launch", "Args");
            var text = xml.CreateTextNode(resultetxtblock.Text);
            var elements = xml.GetElementsByTagName("text");
            elements[0].AppendChild(text);
            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        private Songprop pickprevsong()
        {
            var currentindex = -1;
            var prevsong = Songs.Where(p => p.nextsong == false);
            currentindex = mygridview.SelectedIndex - 1 ;
            var randsong = prevsong.ElementAt(currentindex);
            randsong.selected = true;
            var songname = randsong.songtitle;
            remaintxt.Text = string.Format("Now Playing: {0}", songname);
            return randsong;
        }

        private async void nextbtn_Click(object sender, RoutedEventArgs e)
        {
            var next = picknextsong();
            newelements.SetSource(await next.songfile.OpenAsync(FileAccessMode.Read),next.songfile.ContentType);
            newelements.Play();
        }

        private async void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            var prev = pickprevsong();
            newelements.SetSource(await prev.songfile.OpenAsync(FileAccessMode.Read), prev.songfile.ContentType);
            newelements.Play();
        }

        private Videoprop picknextvideo()
        {
            var currentind = -1;
            var nextvid = Videos.Where(p => p.nextvideo == false);
            currentind = videogridview.SelectedIndex + 1;
            var randvideo = nextvid.ElementAt(currentind);
            randvideo.selected = true;
            var videoname = randvideo.videoname;
            remaintxt.Text = string.Format("Now Playing: {0}", videoname);
            return randvideo;
        }

        private Videoprop pickprevvideo()
        {
            var currentind = -1;
            var nextvid = Videos.Where(p => p.nextvideo == false);
            currentind = videogridview.SelectedIndex - 1;
            var randvideo = nextvid.ElementAt(currentind);
            randvideo.selected = true;
            var videoname = randvideo.videoname;
            remaintxt.Text = string.Format("Now Playing: {0}", videoname);
            return randvideo;
        }

        private async void previbtn_Click(object sender, RoutedEventArgs e)
        {
            var previ = pickprevvideo();

            newelements.SetSource(await previ.videofile.OpenAsync(FileAccessMode.Read),previ.videofile.ContentType);
            newelements.Play();
        }

        private async void nextibtna_Click(object sender, RoutedEventArgs e)
        {
            var nexttt = picknextvideo();
            newelements.SetSource(await nexttt.videofile.OpenAsync(FileAccessMode.Read), nexttt.videofile.ContentType);
            newelements.Play();
        }
    }

        
    }

    

    
