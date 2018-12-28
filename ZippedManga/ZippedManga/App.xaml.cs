using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ZippedManga
{
    public partial class App : Application
    {
        public static List<DirectoryInfo> SelectedDirList { get; set; }
        public static List<string> KeyChain { get; set; }

        public App()
        {
            InitializeComponent();
            SelectedDirList = new List<DirectoryInfo>();
            SelectedDirList.Add(new DirectoryInfo("/sdcard/ZippedManagas"));
            SelectedDirList.ForEach(info =>
            {
                if(!info.Exists)
                {
                    info.Create();
                }
            });

            KeyChain = new List<string>();
            KeyChain.Add("ce");

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
