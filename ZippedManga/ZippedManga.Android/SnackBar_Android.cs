using Android.Support.Design.Widget;
using Plugin.CurrentActivity;
using ZippedManga.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SnackBar_Android))]

namespace ZippedManga.Droid
{
    class SnackBar_Android : SnackInterface
    {
        public void Show(string message)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            var view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(view, message, Snackbar.LengthLong).Show();
        }
    }
}