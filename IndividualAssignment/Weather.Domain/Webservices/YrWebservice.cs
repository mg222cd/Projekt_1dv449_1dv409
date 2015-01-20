﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Weather.Domain.Webservices
{
    public class YrWebservice : IYrWebservice
    {
        public IEnumerable<Forecast> GetForecast(Geoname geoname)
        {
            string requestUriString = String.Format("http://www.yr.no/sted/{0}/{1}/{2}/forecast.xml", geoname.countryName, geoname.adminName1, geoname.name);
            XDocument xDoc = XDocument.Load(requestUriString);
            var _lastUpdate = xDoc.Element("weatherdata").Element("meta").Element("lastupdate").Value;
            var _nextUpdate = xDoc.Element("weatherdata").Element("meta").Element("nextupdate").Value;
            geoname.nextUpdate = ConvertToDateTime(_nextUpdate);

            var groupedForcasts = xDoc.Descendants("time")
                .Take(20)
                .ToList();
            var forecast = new List<Forecast>();

            foreach (var item in groupedForcasts)
            {
                DateTime lastUpdate = ConvertToDateTime(_lastUpdate);
                DateTime nextUpdate = ConvertToDateTime(_nextUpdate);
                DateTime timefrom = ConvertToDateTime((string)item.Attribute("from"));
                DateTime timeto = ConvertToDateTime((string)item.Attribute("to"));
                int timeperiod = (int)item.Attribute("period");
                int temperature = (int)item.Element("temperature").Attribute("value");
                string symbolId = (string)item.Element("symbol").Attribute("var");
                forecast.Add(new Forecast(lastUpdate, nextUpdate, timefrom, timeto, timeperiod, temperature, symbolId, geoname));
            }
            return forecast;
        }

        private DateTime ConvertToDateTime(string stringToConvert)
        {
            string[] splittedString = stringToConvert.Split('T');
            string dateTimeString;
            dateTimeString = splittedString[0] + " " + splittedString[1];
            DateTime convertedString = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture);
            return convertedString;
        }
    }
}