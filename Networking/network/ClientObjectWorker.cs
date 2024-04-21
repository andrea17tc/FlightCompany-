using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.model;
using Utils.service;

namespace Networking.network
{
    public class ClientObjectWorker : IObserver
    {
        private IService server;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public ClientObjectWorker(IService server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {

                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void updateFlight()
        {
            throw new NotImplementedException();
        }

        private Response HandleRequest(Request request)
        {
            Response response = null;
            if(request.Type() == RequestType.LOGIN)
            {
                Console.WriteLine("Login request ...");
                User user = (User)request.Data();
                try
                {
                    int id = server.login(user.Username, user.Password, this);
                    return new Response.Builder().Type(ResponseType.OK).Data(id).Build();
                }
                catch(Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
                        
            }
            if (request.Type() == RequestType.LOGOUT)
            {
                Console.WriteLine("Logout request ...");
                int userID = (int)request.Data();
                try
                {
                    server.logout(userID);
                    connected = false;
                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }
            if(request.Type() == RequestType.FIND_ALL_AVAILABLE_FLIGHTS)
            {
                Console.WriteLine("Find all available flights request ...");
                try
                {
                    IEnumerable<Flight> flights = server.findAllAvailableFlights();
                    return new Response.Builder().Type(ResponseType.GET_AVAILABLE_FLIGHTS).Data(flights).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if(request.Type() == RequestType.FIND_ALL_FLIGHTS_BY_DESTINATION_AND_DATE)
            {
                Console.WriteLine("Find all flights by destination and date request ...");
                Object[] data = (Object[])request.Data();
                string destination = (string)data[0];
                String date = (string)data[1];
                try
                {
                    List<Flight> flights = server.findAllFlightsByDestinationAndDate(destination, DateTime.Parse(date));
                    return new Response.Builder().Type(ResponseType.GET_FLIGHTS_BY_DESTINATION_AND_DATE).Data(flights).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if(request.Type() == RequestType.FIND_ALL_FLIGHTS)
            {
                Console.WriteLine("Find all flights request ...");
                try
                {
                    IEnumerable<Flight> flights = server.findAll();
                    return new Response.Builder().Type(ResponseType.GET_ALL_FLIGHTS).Data(flights).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if(request.Type() == RequestType.FIND_ALL_FLIGHT_DESTINATIONS)
            {
                Console.WriteLine("Find all flight destinations request ...");
                try
                {
                    IEnumerable<String> destinations = server.findAllFlightDestinations();
                    return new Response.Builder().Type(ResponseType.GET_ALL_FLIGHT_DESTINATIONS).Data(destinations).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }
            if(request.Type() == RequestType.FIND_AVAILABLE_SEATS)
            {
                Console.WriteLine("Find available seats request ...");
                int flightId = (int)request.Data();
                try
                {
                    int seats = server.findAvailableSeats(flightId);
                    return new Response.Builder().Type(ResponseType.GET_AVAILABLE_SEATS).Data(seats).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }
            if(request.Type() == RequestType.SAVE_TOURIST)
            {
                Console.WriteLine("Save tourist request ...");
                Object[] data = (Object[])request.Data();
                string name = (string)data[0];
                int purchaseID = (int)data[1];
                try
                {
                    server.saveTourist(name, purchaseID);
                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }

            }

            if (request.Type() == RequestType.SAVE_TRIP)
            {
                Console.WriteLine("Save Trip Request...");
                Object[] data = (Object[])request.Data();
                int flightID = (int)data[0];
                int touristID = (int)data[1];
                try
                {
                    server.saveTrip(flightID, touristID);
                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if(request.Type() == RequestType.SAVE_PURCHASE)
            {
                Console.WriteLine("Save Purchase request ...");
                Object[] data = (Object[])request.Data();
                string clientName = (string)data[0];
                string clientAdress = (string)data[1];
                int userID = (int)data[2];
                int flightID = (int)data[3];
                try
                {
                    int id=server.savePurchase(clientName, clientAdress, userID, flightID);
                    return new Response.Builder().Type(ResponseType.OK).Data(id).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if(request.Type() == RequestType.UPDATE_FLIGHT_SEATS)
            {
                Console.WriteLine("Update flight seats request ...");
                Object[] data = (Object[])request.Data();
                int noSeats = (int)data[0];
                int flightID = (int)data[1];
                try
                {
                    server.updateFlightSeats(noSeats, flightID);
                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch(Exception e)
                {
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }
            return response;
        }
        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object response = HandleRequest((Request)request);
                    if (response != null)
                    {
                        sendResponse((Response)response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }

        }

        public void update()
        {
            try
            {
                sendResponse(new Response.Builder().Type(ResponseType.UPDATE).Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

