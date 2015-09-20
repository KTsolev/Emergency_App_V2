using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//#######################
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EmergencyApp_v2
{
	[Activity (Label = "SearchActivity")]			
	public class SearchActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.SearchFragmentLayout);
			//ExpandableListView options = FindViewById<ExpandableListView>(Resource.Id.expandableListView1);
			TextView textView = FindViewById<TextView>(Resource.Id.textView1);

			string text = Intent.GetStringExtra ("MyLocation") ?? "Data not available";
			textView.Text = "This is the My Search tab "+" MyLocation: "+text;
			String[] data = {"police", "firedepartment", "hospital", "atm"};
			//options.Adapter = new ArrayAdapter (this, Resource.Layout.Search, data);
		
		}
	}
}

