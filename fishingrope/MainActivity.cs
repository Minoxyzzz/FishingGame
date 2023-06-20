using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Content;

namespace fishingrope
{
    [Activity(Label = "Fishing Game", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //xml linky a linky :D
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Button playButton = FindViewById<Button>(Resource.Id.playButton);
            playButton.Click += delegate
            {
                Intent gameActivity = new Intent(this, typeof(GameActivity));
                StartActivity(gameActivity);
            };

            Button aboutButton = FindViewById<Button>(Resource.Id.aboutButton);
            aboutButton.Click += delegate
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);

                // about alert
                alert.SetTitle("About");
                alert.SetMessage("Fishing Game v1.0 \nDotkni se obrazovky pro vypuštění háčku,\npři zásahu ryby dostaneš bod!");
                alert.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Back to menu", ToastLength.Short).Show();
                });

                // vyvolání alertu
                Android.App.AlertDialog dialog = alert.Create();
                dialog.Show();
            };
        }
        //permis update
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
