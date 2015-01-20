﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Domain.Repositories
{
    public interface IWeatherRepository : IDisposable
    {
        //Geoname
        IEnumerable<Geoname> FindAllGeonames();
        Geoname FindGeonameById(int id);
        Geoname FindGeonameByGeonameId(string geonameid);
        void AddGeoname(Geoname geoname);
        void UpdateGeoname(Geoname geoname);
        void RemoveGeoname(int id);

        //Forecast
        IEnumerable<Forecast> FindAllForecasts();
        Forecast FindForecastById(int id);
        void AddForecast(Forecast forecast);
        void UpdateForecast(Forecast forecast);
        void RemoveForecast(int id);

        //Save
        void Save();
    }
}