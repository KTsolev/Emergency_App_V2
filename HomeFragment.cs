using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EmergencyApp_v2
{
	public class HomeFragment : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View view = inflater.Inflate(Resource.Layout.HomeFragmentLayout, container, false);
			ImageButton button112 = view.FindViewById<ImageButton>(Resource.Id.imageButton112);
			ImageButton button115 = view.FindViewById<ImageButton>(Resource.Id.imageButton115);
			ImageButton button1515 = view.FindViewById<ImageButton>(Resource.Id.imageButton1515);
			ImageButton button118 = view.FindViewById<ImageButton>(Resource.Id.imageButton118);	
		
			button112.Click+= delegate {
				ClickButton("112");
			};
			button115.Click+= delegate {
				ClickButton("115");
			};
			button1515.Click+= delegate {
				ClickButton("1515");
			};
			button118.Click+= delegate {
				ClickButton("118");
			};

			return view;
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			FragmentManager.BeginTransaction().Remove(this).Commit();
		}

		protected void ClickButton(String num)
		{
			var uri = Android.Net.Uri.Parse ("tel:" + num);
			var intent = new Intent (Intent.ActionDial, uri);
			StartActivity (intent);
		}
	}
}

