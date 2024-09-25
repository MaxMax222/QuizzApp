using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace QuizzApp
{
    [Activity(Label = "Questions")]
    public class Questions : Activity
    {
        RadioGroup options;
        Button next;
        LinearLayout container;
        Random rnd = new Random();
        ImageView[] images;
        int[] radioButtonIds;
        int currentQuestion, score = 0, amount_of_questions = 10;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.questions);
            Init();
            AddClicks();
        }

        private void AddClicks()
        {
            next.Click += Next_Click;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (currentQuestion < amount_of_questions - 1)
            {
                // Check if the selected answer is correct
                if (options.CheckedRadioButtonId == radioButtonIds[int.Parse(images[currentQuestion].Tag.ToString())])
                {
                    score++;
                }

                currentQuestion++;
                GenerateQuestion();
            }
            else
            {
                // End of quiz, go to main activity
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("score", score.ToString());
                StartActivity(intent);
            }
        }

        private void Init()
        {
            options = FindViewById<RadioGroup>(Resource.Id.select_options);
            next = FindViewById<Button>(Resource.Id.next);
            container = FindViewById<LinearLayout>(Resource.Id.imageContainer);
            currentQuestion = 0;

            images = GenerateImages();
            GenerateQuestion();
            next.Enabled = false;

            options.CheckedChange += (sender, e) =>
            {
                next.Enabled = e.CheckedId != -1; // Enable next button if any option is selected
            };

            // Initialize array to store RadioButton IDs
            radioButtonIds = new int[options.ChildCount];

            // Iterate through the RadioGroup and get all the RadioButton IDs
            for (int i = 0; i < options.ChildCount; i++)
            {
                if (options.GetChildAt(i) is RadioButton radioButton)
                {
                    radioButtonIds[i] = radioButton.Id;
                }
            }
        }

        void GenerateQuestion()
        {
            next.Enabled = false; // Disable next button until an option is selected
            options.ClearCheck(); // Clear any previously selected option
            container.RemoveAllViews(); // Clear the previous image
            container.AddView(images[currentQuestion]); // Add the current image
        }

        ImageView[] GenerateImages()
        {
            char[] types = { 's', 'c', 'h', 'd' };
            var images = new ImageView[amount_of_questions];
            int tagInt = 0, num;
            char type;
            var typeToTagMap = new Dictionary<char, int>
            {
                { 'd', 0 },
                { 'h', 1 },
                { 'c', 2 },
                { 's', 3 }
            };


            for (int i = 0; i < amount_of_questions; i++)
            {
                images[i] = new ImageView(this);
                num = rnd.Next(1, 14);
                type = types[rnd.Next(4)];
                images[i].SetImageResource(base.Resources.GetIdentifier($"img{num}{type}", "drawable", this.PackageName));

                tagInt = typeToTagMap.TryGetValue(type, out var tag) ? tag : -1;
                images[i].Tag = tagInt; // Store the tag value to identify the correct answer later
            }
            return images;
        }
    }
}
