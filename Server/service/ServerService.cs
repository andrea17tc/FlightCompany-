using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Utils.service;
using System.Data;
using Server.repository;
using Utils.model;

namespace Server.service 
{
    public class ServerService :IService
    {
        FlightRepository flightRepository;
        PurchaseRepository purchaseRepository;
        TouristRepository touristRepository;
        TripRepository tripRepository;
        UserRepository userRepository;
        int touristID;

        Dictionary<string, IObserver> loggedClients = new Dictionary<string, IObserver>();
        private readonly object lockObject = new object();

        public ServerService(FlightRepository flightRepository, PurchaseRepository purchaseRepository,
        TouristRepository touristRepository, TripRepository tripRepository, UserRepository userRepository)
        {
            this.flightRepository = flightRepository;
            this.purchaseRepository = purchaseRepository;
            this.touristRepository = touristRepository;
            this.tripRepository = tripRepository;
            this.userRepository = userRepository;
        }

        public List<Flight> findAllAvailableFlights()
        {
            lock (lockObject)
            {
                List<Flight> flights = flightRepository.findAll().Where(flight => flight.NoTotalSeats > 0).ToList();
                return flights;
            }
        }
        public List<string> findAllFlightDestinations()
        {
            lock(lockObject) { 
                List<string> destinations = new List<string>();
                destinations = flightRepository.findAll().Select(flight => flight.Destination.Trim()).Distinct().ToList();
                return destinations;
            }
        }

        public IEnumerable<Flight> findFlightsByDestination(string destination, List<Flight> flights)
        {
            List<Flight> flightsToDestination = flights.Where(flight => flight.Destination.Trim() == destination).ToList();
            return flightsToDestination;
        }

        public IEnumerable<Flight> findFlightsByDate(DateTime date, List<Flight> flights)
        {
            List<Flight> flightsOfDate = flights.Where(flight => Equals(flight.Date.Date, date)).ToList();
            return flightsOfDate;
        }

        public List<Flight> findAllFlightsByDestinationAndDate(string destination, DateTime date)
        {
            lock(lockObject)
            {
                List<Flight> flights = findAllAvailableFlights().ToList();
                List<Flight> flightsByDate = findFlightsByDate(date, flights).ToList();
                List<Flight> flightsByDestinationAndDate = findFlightsByDestination(destination, flightsByDate).ToList();
                return flightsByDestinationAndDate;
            }

        }

        public int login(string username, string password, IObserver client)
        {
            int id = findUser(username, password,client);
            if (id != 0)
            {
                if (loggedClients.ContainsKey(username))
                {
                    throw new Exception("User already logged in.");
                }
                loggedClients[username] = client;
                return id;
            }
            else
            {
                throw new Exception("Authentication failed.");
            }
        }

        public void logout(int userID)
        {
            User user = userRepository.findOne(userID);
            if (loggedClients.ContainsKey(user.Username))
            {
                loggedClients.Remove(user.Username);
            }
            else
            {
                throw new Exception("User not logged in.");
            }

        }
        public int findUser(string username, string password, IObserver client)
        {
            lock (lockObject) { 
                User user = userRepository.findByUsername(username);
                if (user == null)
                {
                    return 0;
                }

                if (user.Password == password)
                {
                    return user.Id;
                }
                return 0;
            }
        }

        public int findNumberAvailableaSeatsForFlight(int flightID)
        {
            return flightRepository.findOne(flightID).NoTotalSeats;
        }
        public int findAvailableSeats(int flightID)
        {
            lock (lockObject)
            {
                Flight f = flightRepository.findOne(flightID);
                return f.NoTotalSeats;
            }
        }

        public void saveTourist(string touristName)
        {
            lock(lockObject)
            {
                try
                {
                    Tourist tourist = new Tourist(touristName);
                    if(touristRepository.findByName(touristName) == null)
                    {
                        touristRepository.save(tourist);
                        touristID = touristRepository.findByName(touristName).Id;
                    }
                    else
                    {
                        touristID = touristRepository.findByName(touristName).Id;
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               
            }
           
        }

        public void saveTourist(string touristName,int purchaseID)
        {
            saveTourist(touristName);
            saveTrip(purchaseID, touristID);
        }

        public void saveTrip(int purchaseID, int touristID)
        {
            lock (lockObject)
            {
                try
                {
                    Tourist t = touristRepository.findOne(touristID);
                    Purchase p = purchaseRepository.findOne(purchaseID);
                    Trip trip = new Trip(t, p);
                    tripRepository.save(trip);
                }
                catch(Exception e)
                {
                   Console.WriteLine(e.Message);
                }
            }
            
            
        }
        public int savePurchase(string clientName, string clientAdress, int userID, int flightID)
        {
            lock (lockObject) { 
                try
                {
                    saveTourist(clientName);
                    int touristID = touristRepository.findByName(clientName).Id;
                    Flight f = flightRepository.findOne(flightID);
                    User u = userRepository.findOne(userID);
                    Tourist t = touristRepository.findOne(touristID);
                    Purchase p = new Purchase(f, u, t, clientAdress);
                    purchaseRepository.save(p);
                    int purchaseID = purchaseRepository.findByClientAndFlight(flightID, touristID).Id;
                    return purchaseID;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }

        public void updateFlightSeats(int noSeats, int flightID)
        {
            lock (lockObject)
            {            
                try
                {
                    Flight f = flightRepository.findOne(flightID);
                    int nr = f.NoTotalSeats - noSeats;
                    flightRepository.updateNoSeats(flightID, nr);
                    notifyAll();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void notifyAll()
        {
            Task.Run(() =>
            {
                foreach (var client in loggedClients.Values)
                {
                    try
                    {
                        client.update();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            
            
        }


        public List<Flight> findAll()
        {
            throw new NotImplementedException();
        }

    }
}
