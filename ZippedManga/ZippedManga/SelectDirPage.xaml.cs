using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZippedManga
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectDirPage : ContentPage
	{
        string CurrentDir;

        public SelectDirPage()
        {
            CurrentDir = "/sdcard";
            InitializeComponent();

            Refresh();
        }

        public SelectDirPage(string startDir)
        {
            CurrentDir = startDir;
            InitializeComponent();

            Refresh();
        }

        async void Refresh()
        {
            var fileItems = new List<DirectoryInfo>();

            await Task.Run(() =>
            {
                var info = new DirectoryInfo(CurrentDir);

                if (CurrentDir != "/")
                    fileItems.Add(info.Parent);

                foreach (var dir in info.GetDirectories())
                    fileItems.Add(dir);
            });

            dirList.ItemsSource = fileItems;
        }

        private void DirList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(dirList.SelectedItem is DirectoryInfo dirInfo)
            {
                { 
                    CurrentDir = dirInfo.FullName;
                }

                var dirs = dirList.ItemsSource as List<DirectoryInfo>;
                dirs.Clear();
                Refresh();
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var dir = new DirectoryInfo(CurrentDir);
            App.SelectedDirList.Add(dir);
            await Navigation.PopAsync();
            MainPage._instanse.UpdateCards();
        }
    }
}