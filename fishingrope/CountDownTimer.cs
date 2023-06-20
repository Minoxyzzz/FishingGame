using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fishingrope
{
    //timer
    public class CountDownTimer : Android.OS.CountDownTimer
    {
        public Action<long> OnTickAction { get; set; }
        public Action OnFinishAction { get; set; }

        public CountDownTimer(long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
        {
        }

        public override void OnFinish()
        {
            OnFinishAction();
        }

        public override void OnTick(long millisUntilFinished)
        {
            OnTickAction(millisUntilFinished);
        }
    }

}