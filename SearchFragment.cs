using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Json;


namespace EmergencyApp_v2
{
	public class SearchFragment : Fragment
	{   
		private MapView MapView;
		private GoogleMap Map;
		private Location MyLocation;
		private Location MyLastLocation;
		private Marker NmtMarker;
		private Marker PoisMarkers;
		private LatLng Lat;
		private Button ShowPois;
		private Fragment PoisFragment;
		private FrameLayout FragmentContainer;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.SearchFragmentLayout, null);
			MapView = view.FindViewById<MapView>(Resource.Id.mapView);
			ShowPois = view.FindViewById<Button>(Resource.Id.getAllPois);
			FragmentContainer= view.FindViewById<FrameLayout>(Resource.Id.poisContainer);
			MapView.OnCreate(savedInstanceState);

			ShowPois.Click += async (object sender, EventArgs e) => {
				if(Lat != null)
				{
				 	JsonValue json = await Pois.FetchPoisrAsync(Lat.Latitude.ToString(),Lat.Longitude.ToString());
					Pois.ParseJsonData(json);	
				}

				if(Pois.NearByPois.Count > 0 && Map != null)
				{
					AddMarkersToMap();
				}
				else
				{				
					Toast.MakeText(this.Activity.BaseContext, "Sorry there is no fetched data yet!Please try again 4-5 seconds later :)!", ToastLength.Short).Show();;	
				}
			};
				
			return view;
		} 

		public override void OnActivityCreated(Bundle p0)
		{
			base.OnActivityCreated(p0);
			MapsInitializer.Initialize(Activity);
		}

		public override void OnStart()
		{
			base.OnStart();
			InitializeMapAndHandlers();
		}

		private void InitializeMapAndHandlers()
		{
			SetUpMapIfNeeded();

			if (Map != null) {
				Map.MyLocationEnabled = false;
				MyLocation = null;
				MyLastLocation = null;
				Lat = new LatLng (45.469303, 9.188106);
				MyLastLocation = MyLocation;
				OnLocationChanged (Lat);

				float[] distance = new float[2];
				float[] newDistance = new float[2];

				Map.MyLocationChange += (s, e) => {

					MyLastLocation = MyLocation;
					MyLocation = e.Location;
					if (MyLocation != null && MyLastLocation != null) {
						Location.DistanceBetween (MyLocation.Latitude, MyLastLocation.Latitude, MyLocation.Longitude, MyLocation.Latitude, distance);

						if (Math.Abs (newDistance [0] - distance [0]) >= 1F && Math.Abs (newDistance [1] - distance [1]) >= 1F) {	
							Lat = new LatLng (MyLocation.Latitude, MyLocation.Longitude);
							OnLocationChanged (Lat);
							distance = newDistance;
						}
					}
				};

			}
			else
			{
				SetUpMapIfNeeded ();
			}
		}

	
		public override void OnDestroyView()
		{
			base.OnDestroyView();
			MapView.OnDestroy();
		}

		public override void OnResume()
		{
			base.OnResume();
			SetUpMapIfNeeded();
			MapView.OnResume();
		}

		public override void OnPause()
		{
			base.OnPause();
			MapView.OnPause();
		}

		public override void OnLowMemory()
		{
			base.OnLowMemory();
			MapView.OnLowMemory();
		}

		private void SetUpMapIfNeeded()
		{
			if(null == Map)
			{
				Map = View.FindViewById<MapView>(Resource.Id.mapView).Map;
			}
		}

		public void AddMarkersToMap()
		{
			foreach(Pois item in Pois.NearByPois)
			{
				LatLng lat = new LatLng (item.Lat, item.Lng);
				int iconId = 0;
				switch (item.Type) 
				{
					case "\"hospital\"":
						iconId = Resource.Drawable.hospital_icon;
					break;
					case "\"pharmacy\"":
						iconId = Resource.Drawable.parmacy_icon;
					break;
					case "\"police\"":
						iconId = Resource.Drawable.police_icon;
					break;
					case "\"fire_station\"":
						iconId = Resource.Drawable.fire_station_icon;
					break;
					case  "\"car_repair\"":
						iconId = Resource.Drawable.car_repair_icon;
					break;
					case "\"embassy\"":
						iconId = Resource.Drawable.embassy_icon;
					break;
					default:
						iconId = Resource.Drawable.marker_icon;
					break;
				}

				var markerOptions = new MarkerOptions ()
					.SetPosition (lat)
					.SetTitle (item.Name)
					.SetSnippet(item.Address)
					.InvokeIcon(BitmapDescriptorFactory.FromResource(iconId))
					.Draggable(true);

				PoisMarkers = Map.AddMarker(markerOptions);
			}
		}

		public void OnLocationChanged (LatLng location)
		{
			if (NmtMarker != null) 
			{
				NmtMarker.Remove ();
			}

			Map.MyLocationEnabled = true;
			MyLocation = null;
			MyLastLocation = null;
			Lat = location;
			MyLastLocation = MyLocation;
			var markerOptions = new MarkerOptions ()
				.SetPosition (Lat)
				.SetTitle ("You are here!")
				.Draggable(true);

			NmtMarker = Map.AddMarker(markerOptions);
			Map.MoveCamera (CameraUpdateFactory.NewLatLngZoom(Lat, 10));
			Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(Lat, 10));

		}
	}
}

