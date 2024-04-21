using Server.repository;
using Server.service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.utils;
using System.Net.Sockets;
using System.Threading;
using Networking.network;
using Utils.service;

namespace Server
{
    public class StartServer
    {
        private static int DEFAULT_PORT = 55555;
        private static String DEFAULT_IP = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.WriteLine("Reading properties from app.config ...");
            int port = DEFAULT_PORT;
            String ip = DEFAULT_IP;
            String portS = ConfigurationManager.AppSettings["port"];
            if (portS == null)
            {
                Console.WriteLine("Port property not set. Using default value " + DEFAULT_PORT);
            }
            else
            {
                bool result = Int32.TryParse(portS, out port);
                if (!result)
                {
                    Console.WriteLine("Port property not a number. Using default value " + DEFAULT_PORT);
                    port = DEFAULT_PORT;
                    Console.WriteLine("Portul " + port);
                }
            }
            String ipS = ConfigurationManager.AppSettings["ip"];
            if (ipS == null)
            {
                Console.WriteLine("Port property not set. Using default value " + DEFAULT_IP);
            }
            IDictionary<String, string> serverProps = new SortedList<String, String>();
            serverProps.Add("ConnectionString", GetConnectionStringByName("companieZbor"));

            UserRepository userRepo = new UserRepository(serverProps);
            FlightRepository flightRepository = new FlightRepository(serverProps);
            TouristRepository touristRepository = new TouristRepository(serverProps);
            PurchaseRepository purchaseRepository = new PurchaseRepository(serverProps, flightRepository, userRepo, touristRepository);
            TripRepository tripRepository = new TripRepository(serverProps, touristRepository, purchaseRepository);
            ServerService service = new ServerService(flightRepository, purchaseRepository, touristRepository, tripRepository, userRepo);

            Console.WriteLine("Starting server on IP {0} and port {1}", ip, port);
            SerialServer server = new SerialServer(ip, port, service);
            server.Start();
        }
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
        public class SerialServer : ConcurrentServer
        {
            private IService server;
            private ClientObjectWorker worker;
            public SerialServer(string host, int port, IService server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialChatServer...");
            }
            protected override Thread createWorker(TcpClient client)
            {
                worker = new ClientObjectWorker(server, client);
                return new Thread(new ThreadStart(worker.run));
            }
        }
    }
}
