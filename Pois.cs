﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Json;
using Android.Util;
using Android.Locations;
using Newtonsoft.Json;

namespace EmergencyApp_v2
{
	

	public class Pois
	{

		public float Lat { get; private set; }
		public float Lng { get; private set; }
		public string Icon { get; private set; }
		public string Name { get; private set; }
		public string Rate { get; private set; }
		public string Type { get; private set; }
		public bool IsOpen { get; private set; }
		public string Address { get; private set; }
		public float[] Distance { get; private set; }

		public static Location MyLocation { get; set;}
		public static List<Pois> NearByPois = new List<Pois>();

		public Pois ()
		{
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("Lat: " + this.Lat);
			sb.AppendLine ("Lng: " + this.Lng);
			sb.AppendLine ("Icon: " + this.Icon);
			sb.AppendLine ("IsOpen: " + this.IsOpen);
			sb.AppendLine ("Name: " + this.Name);
			sb.AppendLine ("Rate: " + this.Rate);
			sb.AppendLine ("Type: " + this.Type);
			sb.AppendLine ("Address: " + this.Address);
			foreach (var i in this.Distance) 
			{
				sb.AppendLine ("Distance: " + i);
			}
			return sb.ToString();
		}	

		public static async Task<JsonValue> FetchPoisrAsync (string latitude, string longitude, string type)
		{
			// Create an HTTP web request using the URL:
			string key="AIzaSyCeQuNmu89FN2_CvB_sPw5Glmf8L3ritTg";
			int radius = 5000;
			string url =  "https://maps.googleapis.com/maps/api/place/search/json?location=" + latitude + "," + longitude + "&radius=" + radius + "&types=" + type + "&place_id=ChIJn8qyiKzGhkcRbtP7f16_hsw&key=" + key + "";;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";
			WebResponse response = null;
			Stream stream = null;
			JsonValue jsonDoc = null;
			// Send the request to the server and wait for the response:
			using (response = await request.GetResponseAsync ())
			{
				// Get a stream representation of the HTTP web response:
				using (stream = response.GetResponseStream ())
				{
					// Use this stream to build a JSON document object:
					jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					//Log.Debug("Response: {0}", jsonDoc.ToString ());
					// Return the JSON document:
					return jsonDoc;
				}
				stream.Close ();
			}
		}

		public static void ParseJsonData(JsonValue jsonDoc)
		{
			JsonValue json = jsonDoc ["results"];
			foreach(JsonValue val in json as JsonArray)
			{
				Pois newPoi = new Pois ();
				newPoi.Distance = new float[2];
				newPoi.Lat = float.Parse(val["geometry"]["location"]["lat"].ToString());	
				newPoi.Lng = float.Parse (val ["geometry"] ["location"] ["lng"].ToString ());
				newPoi.Icon = val ["icon"];
				newPoi.Name = val ["name"];
				newPoi.Address = val ["vicinity"];	
				newPoi.Type =val["types"][0].ToString();
				if (val.ContainsKey ("rating")) 
				{
					newPoi.Rate = val ["rating"].ToString();
				}
				if (val.ContainsKey ("opening_hours")) 
				{
					newPoi.IsOpen = bool.Parse(val ["opening_hours"] ["open_now"].ToString ());
				}
				Location.DistanceBetween (newPoi.Lat, MyLocation.Latitude, newPoi.Lng, MyLocation.Longitude, newPoi.Distance);
				NearByPois.Add (newPoi);
			}
			foreach (var poi in NearByPois) 
			{
				Log.Debug ("Poi", poi.ToString ());
			}
		}
	}
}

