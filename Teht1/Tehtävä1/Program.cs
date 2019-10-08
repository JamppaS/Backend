using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Tehtävä1
{
    class Program
    {
        async static Task Main(string[] args)
        {
            int amountOfBikes = 0;
            string network = "";
            string location = "";

            // Check if there are any arguments
            if (args.Length > 0)
            {
                location = args[0];
                if (args.Length>1)
                {
                    network = args[1];
                }
            }
            // If no arguments ask for the inputs
            else
            {
                Console.WriteLine("Write the location of the station.");
                location = Console.ReadLine();
                Console.WriteLine("Is the query OffLine or Online?");
                network = Console.ReadLine();
            }
            
            try
            {
                if (network.ToLower() == "offline")
                {
                    OfflineCityBikeDataFetcher Fetcher = new OfflineCityBikeDataFetcher();


                    Task<int> bikeAmount = Fetcher.GetBikeCountInStation(location);

                    
                    amountOfBikes = bikeAmount.Result;

                }
                //Default to online
                else
                {
                    RealTimeCityBikeDataFetcher Fetcher = new RealTimeCityBikeDataFetcher();


                    int bikeAmount = await Fetcher.GetBikeCountInStation(location);
                    
                    amountOfBikes = bikeAmount;

                }
                Console.WriteLine("The station has " + amountOfBikes + " bikes.");
            }

            catch (ArgumentException e)
            {
                Console.WriteLine("Invalid argument: " + e);
            }

            catch (NotFoundException e)
            {
                Console.WriteLine("Not found: " + e);
            }


            Console.WriteLine("Exit");

        }


    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {

        public async Task<int> GetBikeCountInStation(string stationName)
        {
            
            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException();
            }

            BikeRentalStationList station = new BikeRentalStationList();            
            System.Net.Http.HttpClient _HttpClient = new System.Net.Http.HttpClient();
            string jsonObj = await _HttpClient.GetStringAsync("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");
            station = JsonConvert.DeserializeObject<BikeRentalStationList>(jsonObj);

            foreach (StationValues item in station.stations)
            {
                if (item.name.ToLower() == stationName.ToLower())
                {
                    return item.bikesAvailable;
                }
            }

            throw new NotFoundException();
        }
    }

    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public Task<int> GetBikeCountInStation(string stationName)
        {

            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException();
            }

            string[] readFile = File.ReadAllLines("bikedata.txt", Encoding.Default);

            foreach (string station in readFile)
            {
                if (station.ToLower().Contains(stationName.ToLower()))
                {
                    return Task.FromResult(int.Parse(System.Text.RegularExpressions.Regex.Match(station, @"\d+").Value));
                }
            }

            throw new NotFoundException();
        }
    }

    public class BikeRentalStationList
    {
        public IList<StationValues> stations;
    }

    public class StationValues
    {
        public string id;
        public string name;
        public float x;
        public float y;
        public int bikesAvailable;
        public int spacesAvailable;
        public bool allowDropoff;
        public bool isFloatingBike;
        public bool isCarStation;
        public string state;
        public IList<string> networks;
        public string realTimeData;
    }

    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }    

    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Error no station found!")
        {
            
        }
    }
}
