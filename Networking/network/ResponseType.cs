using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.network
{
    public enum ResponseType
    {
        OK, ERROR, GET_AVAILABLE_FLIGHTS, GET_ALL_FLIGHT_DESTINATIONS, GET_FLIGHTS_BY_DESTINATION_AND_DATE,
        GET_ALL_FLIGHTS, GET_AVAILABLE_SEATS, SAVE_TOURIST, SAVE_TRIP, UPDATE
    }
}
