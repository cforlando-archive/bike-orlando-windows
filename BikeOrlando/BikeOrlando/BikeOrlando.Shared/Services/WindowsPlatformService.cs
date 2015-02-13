using Microsoft.Maps.SpatialToolbox;
using Microsoft.Maps.SpatialToolbox.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Pickers;

namespace BikeOrlando.Services
{
    class WindowsPlatformService : IPlatformService
    {
        public  SpatialDataSet LoadDataset()
        {
            return null;
            ////Create a FileOpenPicker to allow the user to select which file to import
            //var openPicker = new FileOpenPicker()
            //{
            //    ViewMode = PickerViewMode.List,
            //    SuggestedStartLocation = PickerLocationId.Desktop
            //};
            //openPicker.FileTypeFilter.Add(".json");



            //var file = await openPicker.PickSingleFileAsync();

            //if (file != null)
            //{
            //    using (var fileStream = await file.OpenStreamForReadAsync())
            //    {
            //        //Read the spatial data file
            //        var reader = new GeoJsonFeed();
            //        return await reader.ReadAsync(fileStream);
            //    }
            //}



        }
    }
}
