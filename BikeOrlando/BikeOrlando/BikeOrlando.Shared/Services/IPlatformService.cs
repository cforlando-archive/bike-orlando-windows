using Microsoft.Maps.SpatialToolbox;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeOrlando.Services
{
    interface IPlatformService
    {
        SpatialDataSet LoadDataset();
    }
}
