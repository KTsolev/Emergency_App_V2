/*using System;
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
using Android.Net;

namespace EmergencyApp_v2
{

	public class LocationServiceBinder : Binder
	{
		public LocationService Service
		{
			get { return this.service; }
		} 

		protected LocationService service;
		public bool IsBound { get; set; }            
		public LocationServiceBinder (LocationService service) { this.service = service; }
	}

	[Service]
	public class LocationService : Service, ILocationListener
	{
		protected IBinder binder;
		protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService ("location") as LocationManager;

		public override IBinder OnBind (Intent intent)
		{
			binder = new LocationServiceBinder (this);
			return binder;
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.Sticky;
		}

		public void StartLocationUpdates () {       
			var locationCriteria = new Criteria();                    
			locationCriteria.Accuracy = Accuracy.NoRequirement;        
			locationCriteria.PowerRequirement = Power.NoRequirement;                    
			var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
			LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);
		}

		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public EventLogTags logTag = new EventLogTags ();
		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

		public void OnLocationChanged (Location location)
		{
			this.LocationChanged (this, new LocationChangedEventArgs (location));
			Log.Debug (logTag, String.Format ("Latitude is {0}", location.Latitude));
			Log.Debug (logTag, String.Format ("Longitude is {0}", location.Longitude));
			Log.Debug (logTag, String.Format ("Altitude is {0}", location.Altitude));
			Log.Debug (logTag, String.Format ("Speed is {0}", location.Speed));
			Log.Debug (logTag, String.Format ("Accuracy is {0}", location.Accuracy));
			Log.Debug (logTag, String.Format ("Bearing is {0}", location.Bearing));
		}

		public void OnLocationChanged (Location location)
		{
			Log.Debug (logTag, String.Format ("Latitude is {0}", location.Latitude));
			Log.Debug (logTag, String.Format ("Longitude is {0}", location.Longitude));

		}
	}

	public class LocationServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public LocationServiceConnection (LocationServiceBinder binder)
		{
			if (binder != null) {
				this.binder = binder;
			}
		}

		public void OnServiceConnected (ComponentName name, IBinder service)
		{
			LocationServiceBinder serviceBinder = service as 
				LocationServiceBinder;

			if (serviceBinder != null) {
				this.binder = serviceBinder;
				this.binder.IsBound = true;

				// raise the service bound event
				this.ServiceConnected(this, new 
					ServiceConnectedEventArgs () { Binder = service } );

				// begin updating the location in the Service
				serviceBinder.Service.StartLocationUpdates();
			}
		}

		public void OnServiceDisconnected (ComponentName name) { this.binder.IsBound = false; }
	}
}
*/
