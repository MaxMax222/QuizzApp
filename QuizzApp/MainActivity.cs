using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using System;
using Android.Content;

namespace QuizzApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button next;
        TextView score; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Init();
            AddClicks();
        }

        private void AddClicks()
        {
            next.Click += Next_Click;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Questions));
            StartActivity(intent);
        }

        void Init()
        {
            next = FindViewById<Button>(Resource.Id.next);
            score = FindViewById<TextView>(Resource.Id.score);

            score.Text = $"Previous Score: {Intent.GetStringExtra("score")}/9";
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
