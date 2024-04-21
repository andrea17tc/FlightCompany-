using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Server.utils;
using Utils.model;

namespace Server.repository
{
    public class PurchaseRepository : IRepository<int, Purchase>
    {
        private readonly FlightRepository flightRepository;
        private readonly UserRepository userRepository;
        private readonly TouristRepository touristRepository;
        private static readonly ILog log = LogManager.GetLogger("Purchase Repository");
        IDictionary<String, string> props;

        public PurchaseRepository(IDictionary<String, string> props, FlightRepository flightRepository, UserRepository userRepository,
                              TouristRepository touristRepository)
        {
            log.Info("Creating PurchaseRepository ");
            this.props = props;
            this.flightRepository = flightRepository;
            this.userRepository = userRepository;
            this.touristRepository = touristRepository;
        }

        public Purchase findOne(int id)
        {
            log.InfoFormat("Finding Purchase by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM purchase WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int flightID = dataR.GetInt32(1);
                        int userID = dataR.GetInt32(2);
                        int touristID = dataR.GetInt32(3);
                        string clientAdress = dataR.GetString(4);

                        Flight f = flightRepository.findOne(flightID);
                        User u = userRepository.findOne(userID);
                        Tourist t = touristRepository.findOne(touristID);

                        Purchase p = new Purchase(f, u, t, clientAdress);

                        p.Id = id;
                  
                        log.InfoFormat("Found Purchase: {0}", p);
                        return p;

                    }
                }
            }
            log.InfoFormat("Purchase not found with ID: {0}", id);
            return null;
        }

        public Purchase findByClientAndFlight(int flightID,int touristID)
        {
            log.InfoFormat("Finding Purchase by flightID and touristID, {0},{1}", flightID,touristID);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM purchase WHERE flightID=@flightID and touristID=@touristID; ";
                IDbDataParameter paramFId = comm.CreateParameter();
                paramFId.ParameterName = "@flightID";
                paramFId.Value = flightID;
                comm.Parameters.Add(paramFId);

                IDbDataParameter paramTId = comm.CreateParameter();
                paramTId.ParameterName = "@touristID";
                paramTId.Value = touristID;
                comm.Parameters.Add(paramTId);


                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        int userID = dataR.GetInt32(2);
                        string clientAdress = dataR.GetString(4);

                        Flight f = flightRepository.findOne(flightID);
                        User u = userRepository.findOne(userID);
                        Tourist t = touristRepository.findOne(touristID);

                        Purchase p = new Purchase(f, u, t, clientAdress);

                        p.Id = id;
                        log.InfoFormat("Found Purchase: {0}", p);
                        return p;

                    }
                }
            }
            log.InfoFormat("Purchase not found with IDs");
            return null;
        }

        public IEnumerable<Purchase> findAll()
        {
            log.InfoFormat("Finding All Purchases");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Purchase> purchases = new List<Purchase>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM purchase; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int flightID = dataR.GetInt32(1);
                        int userID = dataR.GetInt32(2);
                        int touristID = dataR.GetInt32(3);
                        string clientAdress = dataR.GetString(4);

                        Flight f = flightRepository.findOne(flightID);
                        User u = userRepository.findOne(userID);
                        Tourist t = touristRepository.findOne(touristID);

                        Purchase p = new Purchase(f, u, t, clientAdress);
                        purchases.Add(p);

                    }
                }
            }
            return purchases;
        }

        public void save(Purchase entity)
        {
            log.InfoFormat("Saving Purchase: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO purchase (flightID, userID, touristID, clientAdress) VALUES (@flightID, @userID, @touristID, @clientAdress);";

                var flightID = command.CreateParameter();
                flightID.ParameterName = "@flightID";
                flightID.Value = entity.Flight.Id;
                command.Parameters.Add(flightID);

                var userID = command.CreateParameter();
                userID.ParameterName = "@userID";
                userID.Value = entity.User.Id;
                command.Parameters.Add(userID);

                var touristID = command.CreateParameter();
                touristID.ParameterName = "@touristID";
                touristID.Value = entity.Tourist.Id;
                command.Parameters.Add(touristID);

                var clientAdress = command.CreateParameter();
                clientAdress.ParameterName = "@clientAdress";
                clientAdress.Value = entity.ClientAddress;
                command.Parameters.Add(clientAdress);


                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Purchase {0} NOT saved", entity);
                    throw new Exception("No purchase added!");

                }
                log.InfoFormat("Purchase {0} saved", entity);

            }
        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting Purchase with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM purchase WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Purchase with ID {0} NOT deleted", id);
                    throw new Exception("No deleted purchase!");
                }
            }
        }

        public void update(Purchase entity)
        {

            log.InfoFormat("Updating Purchase with ID: {0}", entity.Id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE purchase SET clientAdress=@clientAdress WHERE id=@id;";
                IDbDataParameter clientAdress = command.CreateParameter();
                clientAdress.ParameterName = "@clientAdress";
                clientAdress.Value = entity.ClientAddress;
                command.Parameters.Add(clientAdress);

                IDbDataParameter id = command.CreateParameter();
                id.ParameterName = "@id";
                id.Value = entity.Id;
                command.Parameters.Add(id);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Purchase {0} NOT updated", entity);
                    throw new Exception("Purchase NOT updated");
                }
            }
        }
    }

}
