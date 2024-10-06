using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using System;
using Android.Content;

namespace QuizzApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        Button next, logOut;
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
            logOut.Click += LogOut_Click;
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            SetResult(Result.Ok, intent);
            Finish();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Questions));
            StartActivityForResult(intent,0);
        }

        void Init()
        {
            next = FindViewById<Button>(Resource.Id.next);
            logOut = FindViewById<Button>(Resource.Id.logOut);
            score = FindViewById<TextView>(Resource.Id.score);
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    score.Text = $"Previous score is : {data.GetIntExtra("score",0)}/9";
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
