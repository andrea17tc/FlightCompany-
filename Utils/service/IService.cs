using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.model;

namespace Utils.service
{
    public interface IService
    {
        int login(string username, string password, IObserver client);
        void logout(int userID);

        List<Flight> findAllAvailableFlights();
        List<string> findAllFlightDestinations();
        List<Flight> findAllFlightsByDestinationAndDate(string destination, DateTime date);
        List<Flight> findAll();
        int findUser(string username, string password, IObserver client);

        int findAvailableSeats(int flightID);
        void saveTourist(string touristName);
        void saveTourist(string touristName, int purchaseID);
        void saveTrip(int touristID, int purchaseID);
        int savePurchase(string clientName, string clientAddress, int userID, int flightID);
        void updateFlightSeats(int noSeats, int flightID);
    }
}
