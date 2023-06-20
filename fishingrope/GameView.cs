using Android.Content;
using Android.Graphics;
using Android.Views;
using System;

public class GameView : View
{
    private Paint paint;
    private Fish fish;
    private Rope rope;
    private int score = 0;

    public GameView(Context context, int screenX, int screenY) : base(context)
    {
        paint = new Paint();
        fish = new Fish(Resources, screenX, screenY);
        rope = new Rope(Resources, screenX, screenY);
    }

    protected override void OnDraw(Canvas canvas)
    {
        base.OnDraw(canvas);

        fish.Update();
        rope.Update();

        if (CheckCollision())
        {
            rope.resetPosition();
            fish.ResetPosition();
            score++;
        }

        canvas.DrawBitmap(fish.FishBitmap, fish.fishX, fish.fishY, paint);
        canvas.DrawBitmap(rope.RopeBitmap, rope.ropeX, rope.ropeY, paint);
        paint.TextSize = 70;
        canvas.DrawText("Score : " + score, 100, 100, paint);

        // triggeruje the OnDraw metodu
        Invalidate();
    }

    public bool CheckCollision()
    {
        if (Math.Abs(fish.fishX - rope.ropeX) < rope.RopeBitmap.Width && Math.Abs(fish.fishY - rope.ropeY) < rope.RopeBitmap.Height)
        {
            return true;
        }
        return false;
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down)
        {
            rope.ScreenTapped();
        }
        return true;
    }
}
