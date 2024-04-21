using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.network
{
    public enum RequestType
    { 
        LOGIN, LOGOUT, FIND_ALL_AVAILABLE_FLIGHTS, FIND_ALL_FLIGHT_DESTINATIONS, FIND_ALL_FLIGHTS_BY_DESTINATION_AND_DATE,
        FIND_ALL_FLIGHTS, FIND_AVAILABLE_SEATS, SAVE_TOURIST, SAVE_TRIP, SAVE_PURCHASE, UPDATE_FLIGHT_SEATS
    }
}
