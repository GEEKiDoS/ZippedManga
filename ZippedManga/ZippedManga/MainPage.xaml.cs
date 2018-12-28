using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SharpCompress.Common;
using SharpCompress.IO;
using SharpCompress.Readers;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;

namespace ZippedManga
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static MainPage _instanse = null;

        public MainPage()
        {
            InitializeComponent();
            _instanse = this;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            UpdateCards();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectDirPage());
        }

        public CachedImage AddCard(string Title, string Desc, Action<object> clickAction = null)
        {
            var layout = new RelativeLayout();

            var img = new CachedImage
            {
                LoadingPlaceholder = "placeholder.jpg",
                DownsampleUseDipUnits = true,
                DownsampleWidth = 400,
                FadeAnimationEnabled = true,
            };

            layout.Children.Add(img, Constraint.Constant(0), Constraint.Constant(0));

            layout.Children.Add(new Label { Text = Title, TextColor = Color.Black, FontSize = 24 },
                                Constraint.Constant(10),
                                Constraint.RelativeToParent((parent) => { return parent.Height - 10 - 18 - 24; }));

            layout.Children.Add(new Label { Text = Desc, TextColor = Color.FromRgba(0, 0, 0, 200) },
                                Constraint.Constant(10),
                                Constraint.RelativeToParent((parent) => { return parent.Height - 10 - 16; }));

            var card = new Frame
            {
                Content = layout,
                BorderColor = Color.White,
                HasShadow = true,
                Margin = new Thickness { Left = 5, Right = 5, Top = 5, Bottom = 5 },
                Padding = new Thickness(0, 0, 0, 0),
                WidthRequest = 315
            };

            if (clickAction != null)
            {
                card.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(clickAction) });
            }

            rootLayout.Children.Add(card);

            return img;
        }

        public void UpdateCards()
        {
            rootLayout.Children.Clear();

            var infos = new List<DirectoryInfo>();

            App.SelectedDirList.ForEach(dir =>
            {
                foreach (var file in dir.GetFiles())
                {
                    if (file.Extension == ".7z")
                    {
                        var img = AddCard(file.Name.Replace(file.Extension, ""), file.FullName, o =>
                        {
                            DependencyService.Get<SnackInterface>().Show($"You Clicked {file.Name.Replace(file.Extension, "")}");
                        });

                        var stream = new MemoryStream(ZippedMangaHelper.GetMangaPreview(file.FullName));
                        img.Source = ImageSource.FromStream(() => stream);
                    }
                }
            });
        }
    }
}
