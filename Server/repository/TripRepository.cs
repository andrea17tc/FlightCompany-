using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Server.utils;
using Utils.model;

namespace Server.repository
{
    public class TripRepository : IRepository<int, Trip>
    {
        private readonly TouristRepository touristRepository;
        private readonly PurchaseRepository purchaseRepository;
        private static readonly ILog log = LogManager.GetLogger("Trip Repository");
        IDictionary<String, string> props;

        public TripRepository(IDictionary<String, string> props, TouristRepository touristRepository, PurchaseRepository purchaseRepository)
        {
            log.Info("Creating UserRepository ");
            this.props = props;
            this.touristRepository = touristRepository;
            this.purchaseRepository = purchaseRepository;
        }


        public Trip findOne(int id)
        {
            log.InfoFormat("Finding Trip by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM trip WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int touristID = dataR.GetInt32(1);
                        int purchaseID = dataR.GetInt32(2);
                    
                        Tourist t = touristRepository.findOne(touristID);
                        Purchase p = purchaseRepository.findOne(purchaseID);

                        Trip trip = new Trip(t, p);
                        trip.Id = id;

                        log.InfoFormat("Found Trip: {0}", trip);
                        return trip;

                    }
                }
            }
            log.InfoFormat("Trip not found with ID: {0}", id);
            return null;
        }

        public IEnumerable<Trip> findAll()
        {
            log.InfoFormat("Finding All Trips");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Trip> trips = new List<Trip>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM trip; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id= dataR.GetInt32(0);
                        int touristID = dataR.GetInt32(1);
                        int purchaseID = dataR.GetInt32(2);

                        Tourist t = touristRepository.findOne(touristID);
                        Purchase p = purchaseRepository.findOne(purchaseID);

                        Trip trip = new Trip(t,p);
                        trip.Id = id;

                        trips.Add(trip);    

                    }
                }
            }
            return trips;
        }

        public void save(Trip entity)
        {
            log.InfoFormat("Saving Trip: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO trip(touristID, purchaseID) VALUES (@touristID, @purchaseID);";

                var touristID = command.CreateParameter();
                touristID.ParameterName = "@touristID";
                touristID.Value = entity.Tourist.Id;
                command.Parameters.Add(touristID);

                var purchaseID = command.CreateParameter();
                purchaseID.ParameterName = "@purchaseID";
                purchaseID.Value = entity.Purchase.Id;
                command.Parameters.Add(purchaseID);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("trip {0} NOT saved", entity);
                    throw new Exception("No trip added!");

                }
                log.InfoFormat("Trip {0} saved", entity);

            }

        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting trip with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM trip WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Trip with ID {0} NOT deleted", id);
                    throw new Exception("No deleted trip!");
                }
            }
        }

        public void update( Trip entity)
        {

        }
    }

}
