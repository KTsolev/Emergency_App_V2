
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


namespace EmergencyApp_v2
{
	public class SearchFragment : Fragment
	{   
		private MapView MapView;
		private GoogleMap Map;
		private Location MyLocation;
		private Location MyLastLocation;
		private Marker NmtMarker;
		private LatLng Lat;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.SearchFragmentLayout, null);
			MapView = view.FindViewById<MapView>(Resource.Id.mapView);
			MapView.OnCreate(savedInstanceState);
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

			if (Map != null)
			{
				Map.MyLocationEnabled = false;
				MyLocation = null;
				MyLastLocation = null;
				Lat = new LatLng (45.469303, 9.188106);
				MyLastLocation = MyLocation;
				OnLocationChanged (Lat);

				float[] distance= new float[2];
				float[] newDistance= new float[2];

				Map.MyLocationChange += (s, e) => {

					MyLastLocation = MyLocation;
					MyLocation = e.Location;
					if(MyLocation != null && MyLastLocation != null)
					{
						Location.DistanceBetween(MyLocation.Latitude,MyLastLocation.Latitude,MyLocation.Longitude,MyLocation.Latitude,distance);
						Log.Debug (@"My Location is: ",distance.FirstOrDefault()+":"+distance.LastOrDefault());

						if(Math.Abs(newDistance[0] - distance[0]) >= 1F && Math.Abs(newDistance[1] - distance[1]) >= 1F)
						{	
							Lat = new LatLng(MyLocation.Latitude, MyLocation.Longitude);
							OnLocationChanged (Lat);
							distance=newDistance;
						}
					}
				};
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

