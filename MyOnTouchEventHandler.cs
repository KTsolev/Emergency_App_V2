using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EmergencyApp_v2
{
	public class MyOnTouchEventHandler: Java.Lang.Object, View.IOnTouchListener
	{
		private float lastPositionX;

		public bool OnTouch(View v, MotionEvent e)
		{
			switch (e.Action) 
			{
			case MotionEventActions.Down: 
				lastPositionX = e.GetX ();
				Log.Debug ("Touch position: " , lastPositionX+"");
				return true;
			case MotionEventActions.Move:
				var currentPositionX = e.GetX ();
				Log.Debug ("Touch position: " , currentPositionX+"");
				var delta = currentPositionX - lastPositionX;
				Log.Debug ("Touch delta: " , delta+"");
				var translation = v.TranslationX;
				translation -= delta;
				if (translation < 0)
					translation = 0;
				v.TranslationX = translation;
				return true;
			default:
				return v.OnTouchEvent (e);
			}
		}

	}
}

