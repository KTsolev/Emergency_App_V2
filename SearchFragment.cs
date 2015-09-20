
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


namespace EmergencyApp_v2
{
	public class SearchFragment : Fragment
	{   private MapView MapView;
		private GoogleMap Map;
		private Marker NmtMarker;
		private TextView CurrentLocation;


		static readonly string Tag = "Coordinates";

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.SearchFragmentLayout, null);
			TextView CurrentLocation = (TextView) view.FindViewById(Resource.Layout.Main);
			MapView = view.FindViewById<MapView>(Resource.Id.mapView);
			if(CurrentLocation != null)
			if (!CurrentLocation.Text.Equals ("Your Location is:")) 
			{
				string[] coordinates = CurrentLocation.Text.Split (':');
				foreach(var item in coordinates)
				Log.Debug (Tag, item);
			}
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
				Map.MyLocationEnabled = true;

				var latLng = new LatLng(45.478692D, 9.229734D);

				var markerOptions = new MarkerOptions()
					.SetPosition(latLng)
					.Draggable(true);
				NmtMarker = Map.AddMarker(markerOptions);
				Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(latLng, 13));
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
			if(CurrentLocation != null)
			if (!CurrentLocation.Text.Equals ("Your Location is:")) 
			{
				string[] coordinates = CurrentLocation.Text.Split (':');
				foreach(var item in coordinates)
					Log.Debug (Tag, item);
			}
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
	}
}

