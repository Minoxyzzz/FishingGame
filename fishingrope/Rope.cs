using Android.Content.Res;
using Android.Graphics;
using Android.Content;
using fishingrope;


public class Rope
{
    public float ropeX;
    public float ropeY;
    private float speed;
    private int maxY;
    private int minY;
    private Bitmap rope;
    private int ropeWidth = 30;
    private bool goingDown = false;
    public Bitmap RopeBitmap { get; private set; }

    public Rope(Resources res, int screenX, int screenY)
    {
        Bitmap originalRope = BitmapFactory.DecodeResource(res, Resource.Drawable.rope_image);
        RopeBitmap = Bitmap.CreateScaledBitmap(originalRope, 30, 60, false);

        maxY = screenY;
        minY = -RopeBitmap.Height;
        speed = 20;
        ropeX = screenX / 2 - RopeBitmap.Width / 2;
        ropeY = 0;
    }

    public void Update()
    {
        if (goingDown)
        {
            ropeY += speed;
            if (ropeY > maxY)
            {
                goingDown = false;
            }
        }
        else
        {
            ropeY -= speed;
            if (ropeY < 0)
            {
                ropeY = 0;
            }
        }
    }

    public void ScreenTapped()
    {
        goingDown = true;
    }
    public void resetPosition()
    {
        ropeY = 0;
        goingDown = false;
    }
}
