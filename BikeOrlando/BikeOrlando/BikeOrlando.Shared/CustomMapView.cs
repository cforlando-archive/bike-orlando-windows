using BikeOrlando.Services;
using Microsoft.Maps.SpatialToolbox;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace BikeOrlando
{
    public class CustomMapView : MapView
    {
        public void LoadSpatialData(SpatialDataSet data, TappedEventHandler geometryTappedEvent)
        {
            MapTools.LoadGeometries(data, PinLayer,  ShapeLayer, DefaultStyle, geometryTappedEvent);
        }


        #region privates
        private ShapeStyle DefaultStyle = new ShapeStyle()
        {
            FillColor = StyleColor.FromArgb(150, 0, 0, 255),
            StrokeColor = StyleColor.FromArgb(150, 0, 0, 0),
            StrokeThickness = 4
        };

        private Dictionary<string, string> DataSourceUrls = new Dictionary<string, string>()
        {
            { "kml", "http://www.bing.com/maps/GeoCommunity.aspx?action=export&format=kml&mkt=en-us&cid=D35222484A76A01!2835" }
        };
        #endregion
    }
}
