using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BikeOrlando
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		Geolocator geo = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

			GoToCurrentLocation();
        }

		private async void GoToCurrentLocation()
		{
			if (geo == null)
			{
				geo = new Geolocator();
			}
			Geoposition pos = await geo.GetGeopositionAsync();

			MyMap.SetView(pos.Coordinate.Point.Position, 11);
		}

		private void GoToMyLocationBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			GoToCurrentLocation();
		}

		private void ToggleTrafficBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var toggle = sender as AppBarToggleButton;

			MyMap.ShowTraffic = toggle.IsChecked.HasValue && toggle.IsChecked.Value;
		}

		private void AddPushpinsBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var locs = GetSamplePoints();

			for (int i = 0; i < locs.Count; i++)
			{
				MyMap.AddPushpin(locs[i], (i + 1).ToString());
			}
		}

		private void AddPolylineBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var locs = GetSamplePoints();
			MyMap.AddPolyline(locs, GetRandomColor(), 5);
		}

		private void AddPolygonBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var locs = GetSamplePoints();
			MyMap.AddPolygon(locs, GetRandomColor(), GetRandomColor(), 2);
		}

		private void ClearMapBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			MyMap.ClearMap();
		}

		private List<BasicGeoposition> GetSamplePoints()
		{
			var center = MyMap.Center.Position;

			var rand = new Random();
			center.Latitude += rand.NextDouble() * 0.05 - 0.025;
			center.Longitude += rand.NextDouble() * 0.05 - 0.025;

			var locs = new List<BasicGeoposition>();
			locs.Add(new BasicGeoposition() { Latitude = center.Latitude - 0.05, Longitude = center.Longitude - 0.05 });
			locs.Add(new BasicGeoposition() { Latitude = center.Latitude - 0.05, Longitude = center.Longitude + 0.05 });
			locs.Add(new BasicGeoposition() { Latitude = center.Latitude + 0.05, Longitude = center.Longitude + 0.05 });
			locs.Add(new BasicGeoposition() { Latitude = center.Latitude + 0.05, Longitude = center.Longitude - 0.05 });
			return locs;
		}

		private Color GetRandomColor()
		{
			var rand = new Random();

			byte[] bytes = new byte[3];
			rand.NextBytes(bytes);

			return Color.FromArgb(150, bytes[0], bytes[1], bytes[2]);
		}
    }
}
