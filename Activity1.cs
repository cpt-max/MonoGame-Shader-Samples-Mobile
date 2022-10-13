using Android.App;
using Android.Content.PM;
using Android.Media.TV;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Microsoft.Xna.Framework;

namespace ShaderSample
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        ShaderGame game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            game = new ShaderGame();

            var gameView = game.Services.GetService(typeof(View)) as View;
            SetContentView(gameView);

            game.Run();
        }
    }
}
