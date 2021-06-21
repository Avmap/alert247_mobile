using Android.App;
using Android.Content;
using AlertApp.Services;
using Plugin.CurrentActivity;
using Android.Views.InputMethods;

[assembly: Xamarin.Forms.Dependency(typeof(AlertApp.Droid.DroidKeyboardHelper))]
namespace AlertApp.Droid
{
    public class DroidKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            var context = CrossCurrentActivity.Current.Activity;
            
            var inputMethodManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}