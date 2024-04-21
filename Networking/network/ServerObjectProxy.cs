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
    public class ServerObjectProxy : IService
    {
        private string host;
        private int port;

        private IObserver client;

        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;

        public ServerObjectProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public virtual int login(string username, string password, IObserver client)
        {
            initializeConnection();
            User user = new User(username, password);
            Request request = new Request.Builder().type(RequestType.LOGIN).data(user).build();
            sendRequest(request);
            Response response = readResponse();
            if (response.Type() == ResponseType.OK)
            {
                this.client = client;
                return (int)response.Data();
            }
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                closeConnection();
                throw new Exception(err);
            }
            return 0;
        }

        public virtual void logout(int userID)
        {
            Request request = new Request.Builder().type(RequestType.LOGOUT).data(userID).build();
            sendRequest(request);
            Response response = readResponse();
            closeConnection();
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                throw new Exception(err);
            }
        }

        public virtual List<Flight> findAllAvailableFlights()
        {
            try
            {
                Request request = new Request.Builder().type(RequestType.FIND_ALL_AVAILABLE_FLIGHTS).build();
                sendRequest(request);
                Response response = readResponse();
                if(response.Type() == ResponseType.GET_AVAILABLE_FLIGHTS)
                {
                    return (List<Flight>)response.Data();
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return null;
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public virtual List<String> findAllFlightDestinations(){
            try
            {
                Request request = new Request.Builder().type(RequestType.FIND_ALL_FLIGHT_DESTINATIONS).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.GET_ALL_FLIGHT_DESTINATIONS)
                {
                    return (List<String>)response.Data();
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public virtual List<Flight> findAllFlightsByDestinationAndDate(string destination, DateTime date)
        {
            try
            {
                Object[] data = { destination, date.ToString() };
                Request request = new Request.Builder().type(RequestType.FIND_ALL_FLIGHTS_BY_DESTINATION_AND_DATE).data(data).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.GET_FLIGHTS_BY_DESTINATION_AND_DATE)
                {
                    return (List<Flight>)response.Data();
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public virtual List<Flight> findAll()
        {
            try
            {
                Request request = new Request.Builder().type(RequestType.FIND_ALL_FLIGHTS).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.GET_ALL_FLIGHTS)
                {
                    return (List<Flight>)response.Data();
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public virtual int findAvailableSeats(int flightID)
        {
            try
            {
                Request request = new Request.Builder().type(RequestType.FIND_AVAILABLE_SEATS).data(flightID).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.GET_AVAILABLE_SEATS)
                {
                    return (int)response.Data();
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return 0;
            }
        }

        public virtual void saveTourist(string touristName)
        {
            try
            {
                Request request = new Request.Builder().type(RequestType.SAVE_TOURIST).data(touristName).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.OK)
                {
                    return;
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void saveTourist(string touristName, int purchaseID)
        {
            try
            {
                Object[] data = { touristName, purchaseID };
                Request request = new Request.Builder().type(RequestType.SAVE_TOURIST).data(data).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.OK)
                {
                    return;
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void saveTrip(int purchaseID, int touristID)
        {
            try
            {
                Object[] data = { purchaseID, touristID };
                Request request = new Request.Builder().type(RequestType.SAVE_TRIP).data(data).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.OK)
                {
                    return;
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual int savePurchase(string clientName, string clientAddress, int userID, int flightID)
        {
            try
            {
                Object[] data = { clientName, clientAddress, userID, flightID };
                Request request = new Request.Builder().type(RequestType.SAVE_PURCHASE).data(data).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
                return (int) response.Data();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return 0;
            }
        }
        public virtual void updateFlightSeats(int noSeats, int flightID)
        {
            try
            {
                Object[] data = { noSeats, flightID };
                Request request = new Request.Builder().type(RequestType.UPDATE_FLIGHT_SEATS).data(data).build();
                sendRequest(request);
                Response response = readResponse();
                if (response.Type() == ResponseType.OK)
                {
                    return;
                }
                if (response.Type() == ResponseType.ERROR)
                {
                    String err = response.Data().ToString();
                    throw new Exception(err);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void handleUpdate(Response response)
        {
            if (response.Type() == ResponseType.UPDATE)
            {
                client.update();
            }
        }

        private Boolean isUpdate(Response response)
        {
            return response.Type() == ResponseType.UPDATE;
        }

        private void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (isUpdate((Response)response))
                    {
                        handleUpdate((Response)response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue((Response)response);
                        }
                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }

            }
        }
        private void sendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object " + e);
            }

        }

        private Response readResponse()
        {
            Response response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    //Monitor.Wait(responses); 
                    response = responses.Dequeue();

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }

        private void initializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                startReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private void startReader()
        {
            Thread tw = new Thread(run);
            tw.Start();
        }

        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();

                connection.Close();
                _waitHandle.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }   

        public int findUser(string username, string password, IObserver client)
        {
            throw new NotImplementedException();
        }

    }
}
