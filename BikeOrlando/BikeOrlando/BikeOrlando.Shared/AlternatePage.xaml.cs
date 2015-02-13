using Microsoft.Maps.SpatialToolbox.Bing;
using Microsoft.Maps.SpatialToolbox;
using Microsoft.Maps.SpatialToolbox.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;

namespace BikeOrlando
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlternatePage : Page
    {
        public AlternatePage()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Load("racks");
        }

        private void ToggleTrafficBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var toggle = sender as AppBarToggleButton;
            MyMap.ShowTraffic = toggle.IsChecked.HasValue && toggle.IsChecked.Value;
        }

        private void GoToMyLocationBtn_Clicked(object sender, RoutedEventArgs e)
        {
            GoToCurrentLocation();
        }
        private void ClearMapBtn_Clicked(object sender, RoutedEventArgs e)
        {
            MyMap.ClearMap();
        }
        async void LoadLanesButton_Clicked(object sender, RoutedEventArgs e)
        {
            await Load("lanes");
        }

        async void LoadRacksButton_Clicked(object sender, RoutedEventArgs e)
        {
            await Load("racks");
        }

        async void LoadParkingButton_Clicked(object sender, RoutedEventArgs e)
        {
            await Load("parking");  // will throw an exception until we get some data
        }


        private async Task Load(string viewName)
        {
            string errorMessage = null;
            try
            {
                var uri = new System.Uri(string.Format("ms-appx:///data/bike_{0}.json",viewName));
                var data = await LoadDatasetFromResource(uri);
                await LoadMap(data);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
                await new MessageDialog(errorMessage).ShowAsync();

        }


        private async void GeometryTapped(object sender, RoutedEventArgs e)
        {
            var elem = sender as FrameworkElement;
            var metadata = elem == null ? null : elem.Tag as ShapeMetadata;
            var message = metadata == null ? "no metadata" : metadata.Properties["description"] as string;
            var dlg = new MessageDialog(message);
            await dlg.ShowAsync();
        }

        void CenterView(SpatialDataSet data)
        {
            if (data.BoundingBox != null)
            {
#if WINDOWS_APP
                MyMap.SetView(data.BoundingBox.Center.ToBMGeometry().ToGeopoint().Position, 10);
#elif WINDOWS_PHONE_APP
                MyMap.SetView(data.BoundingBox.Center.ToBMGeometry(), 10);
#endif
            }
        }

        public async Task<SpatialDataSet> LoadDatasetFromResource(Uri uri)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);

            if (file != null)
            {
                using (var fileStream = await file.OpenStreamForReadAsync())
                {
                    //Read the spatial data file
                    var reader = new GeoJsonFeed();
                    return await reader.ReadAsync(fileStream);
                }
            }
            return null;
        }


        async Task LoadMap(SpatialDataSet data)
        {
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.Error))
                    await new MessageDialog(data.Error).ShowAsync();
                else
                {
                    MyMap.ClearMap();
                    MyMap.LoadSpatialData(data, GeometryTapped);
                    CenterView(data);
                }
            }
        }


        Geolocator geo = null;
        private async void GoToCurrentLocation()
        {
            if (geo == null)
            {
                geo = new Geolocator();
            }
            Geoposition pos = await geo.GetGeopositionAsync();

            MyMap.SetView(pos.Coordinate.Point.Position, 11);
        }


        /// <summary>
        /// Not currently used
        /// </summary>
        /// <returns></returns>
        public async Task<SpatialDataSet> LoadDatasetFromFileSystem()
        {
            //Create a FileOpenPicker to allow the user to select which file to import
            var openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            openPicker.FileTypeFilter.Add(".json");

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (var fileStream = await file.OpenStreamForReadAsync())
                {
                    //Read the spatial data file
                    var reader = new GeoJsonFeed();
                    return await reader.ReadAsync(fileStream);
                }
            }
            return null;
        }
    }
}
