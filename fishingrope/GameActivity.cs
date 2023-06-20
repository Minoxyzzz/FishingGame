using Android.Animation;
using Android.App;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Diagnostics;
using System.Timers;
using static Android.OS.Vibrator;

namespace fishingrope
{
    [Activity(Label = "Chyť rybu!", Theme = "@style/AppTheme")]
    public class GameActivity : AppCompatActivity
    {
        private int score = 0;
        private TextView scoreView;
        private TextView timeView;
        private ImageView rope;
        private ImageView fish;
        private Timer gameTimer;
        private bool isRopeFalling = false;
        private bool isFishGoingRight = true;
        private int gameTimeInSeconds = 60; // timer odpocet
        private int currentTimeInSeconds;
        private Stopwatch stopwatch;
        private ImageView backgroundImage;
        private ImageView waveImage;
        private Vibrator vibrator;
        private MediaPlayer backgroundMusic;
        private MediaPlayer scoreSound;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_game);

            scoreView = FindViewById<TextView>(Resource.Id.score);
            timeView = FindViewById<TextView>(Resource.Id.time);
            rope = FindViewById<ImageView>(Resource.Id.rope);
            fish = FindViewById<ImageView>(Resource.Id.fish);
            backgroundImage = FindViewById<ImageView>(Resource.Id.background_image);
            waveImage = FindViewById<ImageView>(Resource.Id.wave_image);
            vibrator = (Vibrator)GetSystemService(VibratorService);

            var gameLayout = FindViewById<ViewGroup>(Resource.Id.game_layout);
            gameLayout.Click += (s, e) => isRopeFalling = true;

            // layout 
            gameLayout.Post(() =>
            {
                // pozice rope
                rope.SetX((gameLayout.Width / 2f) - (rope.Width / 2f));
                rope.SetY(0);
                // pozice ryba
                fish.SetX((gameLayout.Width / 2f) - (fish.Width / 2f)); // center horiz. ryba
                fish.SetY(gameLayout.Height - fish.Height);
            });

            stopwatch = new Stopwatch();

            // start animace vlny
            StartWaveAnimation();

            // load backgroundu 
            backgroundImage.SetImageResource(Resource.Drawable.im1);
            waveImage.SetImageResource(Resource.Drawable.im2);

            // play background music
            backgroundMusic = MediaPlayer.Create(this, Resource.Raw.moe);
            backgroundMusic.Looping = true;
            backgroundMusic.Start();

            // load score sound
            scoreSound = MediaPlayer.Create(this, Resource.Raw.col);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartGameTimer();
            StartMovementUpdates();
        }

        protected override void OnPause()
        {
            base.OnPause();
            StopGameTimer();
            StopMovementUpdates();
            backgroundMusic?.Pause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            backgroundMusic?.Release();
            scoreSound?.Release();
            backgroundMusic = null;
            scoreSound = null;
        }

        private void StartGameTimer()
        {
            gameTimer = new Timer
            {
                Interval = 1000, // timer interval
                AutoReset = true,
                Enabled = true
            };

            gameTimer.Elapsed += GameTimer_Elapsed;

            stopwatch.Start();
        }

        private void StopGameTimer()
        {
            gameTimer.Stop();
            gameTimer.Elapsed -= GameTimer_Elapsed;

            stopwatch.Stop();
            stopwatch.Reset();
        }

        // casovac_end
        private void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentTimeInSeconds++;

            RunOnUiThread(() =>
            {
                timeView.Text = $"Time: {gameTimeInSeconds - currentTimeInSeconds}";

                if (currentTimeInSeconds >= gameTimeInSeconds)
                {
                    StopGameTimer();
                    ShowGameOverAlert();
                }
            });
        }

        private void ShowGameOverAlert()
        {
            var dialog = new Android.App.AlertDialog.Builder(this);
            dialog.SetTitle("Game Over");
            dialog.SetMessage($"Your score: {score}");
            dialog.SetCancelable(false);
            dialog.SetPositiveButton("Try Again", (sender, args) =>
            {
                ResetGame();
            });
            dialog.SetNegativeButton("Back to Main Menu", (sender, args) =>
            {
                Finish();
            });
            dialog.Show();
        }

        // funkce reset
        private void ResetGame()
        {
            score = 0;
            currentTimeInSeconds = 0;
            scoreView.Text = $"Score: {score}";
            timeView.Text = $"Time: {gameTimeInSeconds}";
            StartGameTimer();
        }

        // movement stuff 
        private void StartMovementUpdates()
        {
            var movementTimer = new Timer
            {
                Interval = 10, // 10 ms
                AutoReset = true,
                Enabled = true
            };

            movementTimer.Elapsed += MovementTimer_Elapsed;
        }

        private void StopMovementUpdates()
        {
            // zastavuje animace
        }

        private void MovementTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                var gameLayoutWidth = FindViewById<ViewGroup>(Resource.Id.game_layout).Width;

                // tickrate pohybu
                if (isFishGoingRight)
                {
                    fish.SetX(fish.GetX() + (float)(1000 * elapsedSeconds));
                    if (fish.GetX() + fish.Width > gameLayoutWidth)
                    {
                        isFishGoingRight = false;
                    }
                }
                else
                {
                    fish.SetX(fish.GetX() - (float)(1000 * elapsedSeconds));
                    if (fish.GetX() < 0)
                    {
                        isFishGoingRight = true;
                    }
                }

                // movement rope (touch)
                if (isRopeFalling)
                {
                    rope.SetY(rope.GetY() + (float)(3000 * elapsedSeconds));
                }
                else
                {
                    rope.SetY(0);
                }

                // score counter a stuff
                if (rope.GetY() + rope.Height >= fish.GetY() &&
                    rope.GetX() + rope.Width >= fish.GetX() && fish.GetX() + fish.Width >= rope.GetX())
                {
                    score++;
                    scoreView.Text = $"Score: {score}";

                    // vibrace
                    vibrator.Vibrate(100);

                    // zvuk
                    scoreSound?.Start();

                    // reset animace rope
                    isRopeFalling = false;
                }

                // rope+bottom reset
                if (rope.GetY() + rope.Height >= FindViewById<ViewGroup>(Resource.Id.game_layout).Height)
                {
                    isRopeFalling = false;
                }

                stopwatch.Restart();
            });
        }

        // vlna
        private void StartWaveAnimation()
        {
            float screenHeight = Resources.DisplayMetrics.HeightPixels;
            float waveImageHeight = waveImage.Height;

            ValueAnimator waveAnimator = ValueAnimator.OfFloat(0, 0.4f * screenHeight);
            waveAnimator.SetDuration(2000);
            waveAnimator.RepeatCount = ValueAnimator.Infinite;
            waveAnimator.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            waveAnimator.Update += (sender, e) =>
            {
                float animatedValue = (float)e.Animation.AnimatedValue;
                waveImage.SetY(screenHeight - waveImageHeight - animatedValue);
            };

            waveAnimator.Start();
        }
    }
}
