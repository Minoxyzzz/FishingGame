using Android.Content.Res;
using Android.Graphics;
using Android.Content;
using fishingrope;
using System;

public class Fish
{
    public float fishX;
    public float fishY;
    private int speed;
    private Bitmap fish;
    private int fishWidth = 30;
    private int maxX;
    private int minY;
    private bool goingRight = true;
    public Bitmap FishBitmap { get; private set; }

    public Fish(Resources res, int screenX, int screenY)
    {
        Bitmap originalFish = BitmapFactory.DecodeResource(res, Resource.Drawable.fish);
        FishBitmap = Bitmap.CreateScaledBitmap(originalFish, fishWidth, (originalFish.Height * fishWidth) / originalFish.Width, false);

        speed = 10;
        fishX = new Random().Next(screenX - FishBitmap.Width);
        fishY = screenY - FishBitmap.Height;
        maxX = screenX - FishBitmap.Width;
    }

    public void Update()
    {
        if (goingRight)
        {
            fishX += speed;
            if (fishX >= maxX)
            {
                goingRight = false;
            }
        }
        else
        {
            fishX -= speed;
            if (fishX <= 0)
            {
                goingRight = true;
            }
        }
    }

    public void ResetPosition()
    {
        fishY = minY;
    }
}
