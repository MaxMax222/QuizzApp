using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace QuizzApp
{
    [Activity(Label = "Card Quizz", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LogIn : Activity
    {
        Button login, erase;
        CheckBox checkRemember;
        EditText name, password;
        ISharedPreferences preferences;
        ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the layout for this activity
            SetContentView(Resource.Layout.Login);

            // Initialize the UI elements
            Init();

            // Add event handlers
            AddClicks();
        }

        void Init()
        {
            login = FindViewById<Button>(Resource.Id.logBtn);
            erase = FindViewById<Button>(Resource.Id.erase);
            checkRemember = FindViewById<CheckBox>(Resource.Id.checkRemeber);
            name = FindViewById<EditText>(Resource.Id.usernameEdit);
            password = FindViewById<EditText>(Resource.Id.passwordEdit);

            // Get the shared preferences and editor
            preferences = GetSharedPreferences("User_Details", FileCreationMode.Private);
            editor = preferences.Edit();

            // If "Remember Me" is checked, fill in the fields
            if (preferences.GetBoolean("Remember", false))
            {
                name.Text = preferences.GetString("Name", null);
                password.Text = preferences.GetString("Password", null);
                checkRemember.Checked = true;
            }
        }

        void AddClicks()
        {
            login.Click += Login_Click;
            erase.Click += Erase_Click;
        }

        private void Erase_Click(object sender, EventArgs e)
        {
            editor.Clear();
            editor.Apply();
            Toast.MakeText(this, "User information deleted", ToastLength.Short).Show();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            // Validate that both fields are filled
            if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                Toast.MakeText(this, "Please fill in both fields", ToastLength.Long).Show();
                return;
            }

            // Check if the user exists
            if (CheckUserExists())
            {
                // Validate the user credentials
                if (ValidateUser())
                {
                    // If "Remember Me" is checked, save the login details
                    if (checkRemember.Checked)
                    {
                        SaveUserPreferences();
                    }
                    else
                    {
                        editor.PutBoolean("Remember", false);
                        editor.Apply();
                    }

                    // Navigate to the main activity
                    var intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("bestScore", preferences.GetString("HighScore", "0"));  
                    StartActivityForResult(intent, 0);
                }
                else
                {
                    Toast.MakeText(this, "Invalid username or password, try again", ToastLength.Long).Show();
                }
            }
            else
            {
                // If the user doesn't exist, create a new one
                SaveUserPreferences();
                Toast.MakeText(this, "New user created. Welcome!", ToastLength.Long).Show();

                if (checkRemember.Checked)
                {
                    SaveUserPreferences();
                }

                // Navigate to the main activity
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("bestScore", preferences.GetString("HighScore", "0"));
                StartActivityForResult(intent, 0);
            }
        }

        private bool ValidateUser()
        {
            return name.Text == preferences.GetString("Name", null) && password.Text == preferences.GetString("Password", null);
        }

        private bool CheckUserExists()
        {
            string username = preferences.GetString("Name", null);
            string userPassword = preferences.GetString("Password", null);
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userPassword);
        }

        private void SaveUserPreferences()
        {
            editor.PutString("Name", name.Text);
            editor.PutString("Password", password.Text);
            editor.PutBoolean("Remember", checkRemember.Checked);
            if (preferences.GetString("HighScore", null) == null)
            {
                editor.PutString("HighScore", "0");
            }
            editor.Apply(); // Apply changes
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if(requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    name.Text = "";
                    password.Text = "";
                    checkRemember.Checked = false;
                }
            }
        }
    }
}
