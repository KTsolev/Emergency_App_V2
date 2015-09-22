using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using EmergencyApp_v2;
//##################//
using Android;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;
using Android.App;

namespace EmergencyApp_v2
{
	[Activity (Label = "EmergencyApp_v2", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/Theme.Example")]
	public class MainActivity : Activity, ILocationListener, ActionBar.ITabListener
	{

		private Location currentLocation;
		private LocationManager locationManager;
		private TextView locationText;
		private String locationProvider;
		private String addressText;
		private Fragment  home;
		private Fragment  search;
		private Fragment mCurrentFragment;
		private Stack<Fragment> mStackFragments;


		static readonly string Tag = "ActionBarTabsSupport";

		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate(bundle);
			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			ActionBar.SetDisplayShowHomeEnabled (false);
			ActionBar.SetDisplayShowTitleEnabled (false);
			SetContentView(Resource.Layout.Main);

			locationText = FindViewById<TextView>(Resource.Id.location);

			addressText = String.Empty;
			home = new HomeFragment ();
			search = new SearchFragment ();

			mCurrentFragment = home;
			mStackFragments = new Stack<Fragment>();

			var homeTab = ActionBar.NewTab();
			homeTab.SetText(Resources.GetString(Resource.String.tab1_text));
			homeTab.SetIcon(Resource.Drawable.home_icon);
			homeTab.SetTabListener (this);
			ActionBar.AddTab(homeTab);

			var searchTab = ActionBar.NewTab();
			searchTab.SetText(Resources.GetString(Resource.String.tab2_text));
			searchTab.SetIcon(Resource.Drawable.search_icon);
			searchTab.SetTabListener (this);
			ActionBar.AddTab(searchTab);

			var trans = FragmentManager.BeginTransaction ();
			trans.Add (Resource.Id.fragmentContainer, home, "Home");
			trans.Add (Resource.Id.fragmentContainer, search, "Search");
			trans.Hide (search);
			trans.Commit ();
			InitializeLocationManager ();
		}
			
		public void OnLocationChanged(Location location)
		{
			currentLocation = location;
			if (currentLocation == null)
			{
				//GPS Provider can not find current location uses network provider for finding device location
				string Provider = LocationManager.NetworkProvider;

				if(locationManager.IsProviderEnabled(Provider))
				{
					locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
				} 
			}
			else
			{
				GetCurrentAddress ();
			}
		}


		protected override void OnResume()
		{
			base.OnResume();
			if(locationProvider != null)
				locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			if(locationManager != null)
				locationManager.RemoveUpdates(this);
		}


		public void InitializeLocationManager()
		{
			locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = String.Empty;
			}
		}

		public async void GetCurrentAddress()
		{
			if (currentLocation == null)
			{
				addressText = "Can't determine the current address.";
				return;
			}

			Geocoder geocoder = new Geocoder(this);
			IList<Address> addressList = await geocoder.GetFromLocationAsync(currentLocation.Latitude, currentLocation.Longitude, 10);

			Address address = addressList.FirstOrDefault();
			if (address != null)
			{
				StringBuilder deviceAddress = new StringBuilder();
				for (int i = 0; i < address.MaxAddressLineIndex; i++)
				{
					deviceAddress.Append(address.GetAddressLine(i))
						.AppendLine(",");
				}
				addressText = deviceAddress.ToString();
				if(currentLocation != null)
				{
					Pois.MyLocation = currentLocation;
					locationText.Text = "Latitude: " + currentLocation.Latitude + ", Longitude: " + currentLocation.Longitude + ", Address: " + addressText + "";
				}
			}
			else
			{
				addressText = "Unable to determine the address.";
			}
		}


		private void ShowFragment (Fragment fragment)
		{
			if (fragment.IsVisible)
			{
				return;
			}

			if (fragment.View != null && mCurrentFragment.View != null) 
			{
				var trans = FragmentManager.BeginTransaction ();
		
				trans.SetCustomAnimations(Resource.Drawable.slide_in, Resource.Drawable.slide_out, Resource.Drawable.slide_in, Resource.Drawable.slide_out);
				fragment.View.BringToFront ();
				mCurrentFragment.View.BringToFront ();
				trans.Hide (mCurrentFragment);
				trans.Show (fragment);

				trans.AddToBackStack (null);
				mStackFragments.Push (mCurrentFragment);

				trans.Commit ();

				mCurrentFragment = fragment;
			}
			else 
			{
				return;
			}

		}
			
		public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			// Optionally refresh/update the displayed tab.
			Log.Debug(Tag, "The tab {0} was re-selected.", tab.Text);
		}
		 	
		public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			// Display the fragment the user should see
			switch(tab.Position)
			{
			case 0:
					ShowFragment (home);
				break;
			case 1:
					ShowFragment (search);
				break;
			default:
				break;
			}

			Log.Debug(Tag, "The tab {0} has been selected.", tab.Text);
		}

		public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			// Save any state in the displayed fragment.
			/*if (mCurrentFragment != null) {
				ft.Detach (mCurrentFragment);
			}*/
			Log.Debug(Tag, "The tab {0} as been unselected.", tab.Text);
		}

		public override void OnBackPressed ()
		{

			if (FragmentManager.BackStackEntryCount > 0)
			{
				FragmentManager.PopBackStack();
				mCurrentFragment = mStackFragments.Pop();
			}
			else
			{
				FinishAffinity ();
			}				
		}
	}
}


