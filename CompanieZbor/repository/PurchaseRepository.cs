﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Ro.Mpp2024.Model;

namespace Ro.Mpp2024.Repository
{
    public class PurchaseRepository : IRepository<int, Purchase>
    {
        private readonly FlightRepository flightRepository;
        private readonly UserRepository userRepository;
        private readonly TouristRepository touristRepository;
        private readonly JdbcUtils dbUtils;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PurchaseRepository(SqlConnectionStringBuilder props, FlightRepository flightRepository, UserRepository userRepository,
                              TouristRepository touristRepository)
        {
            logger.Info("Initializing PurchaseRepository with properties: {0}", props.ConnectionString);
            dbUtils = new JdbcUtils(props);
            this.flightRepository = flightRepository;
            this.userRepository = userRepository;
            this.touristRepository = touristRepository;
        }

        public Optional<Purchase> FindOne(int id)
        {
            logger.Trace("Finding Purchase by ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "SELECT * FROM purchase WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int flightID = reader.GetInt32("flightID");
                            int userID = reader.GetInt32("userID");
                            int touristID = reader.GetInt32("touristID");
                            string clientAddress = reader.GetString("clientAdress");
                            Flight flight = flightRepository.FindOne(flightID).Value;
                            User user = userRepository.FindOne(userID).Value;
                            Tourist tourist = touristRepository.FindOne(touristID).Value;
                            Purchase purchase = new Purchase(flight, user, tourist, clientAddress);
                            purchase.Id = id;
                            logger.Trace("Found Purchase: {0}", purchase);
                            return Optional.Of(purchase);
                        }
                    }
                }
            }
            logger.Trace("Purchase not found with ID: {0}", id);
            return Optional.Empty<Purchase>();
        }

        public IEnumerable<Purchase> FindAll()
        {
            logger.Trace("Finding all Purchases");
            List<Purchase> purchases = new List<Purchase>();
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "SELECT * FROM purchase;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            int flightID = reader.GetInt32("flightID");
                            int userID = reader.GetInt32("userID");
                            int touristID = reader.GetInt32("touristID");
                            string clientAddress = reader.GetString("clientAdress");
                            Flight flight = flightRepository.FindOne(flightID).Value;
                            User user = userRepository.FindOne(userID).Value;
                            Tourist tourist = touristRepository.FindOne(touristID).Value;
                            Purchase purchase = new Purchase(flight, user, tourist, clientAddress);
                            purchase.Id = id;
                            purchases.Add(purchase);
                        }
                    }
                }
            }
            logger.Trace("Found {0} Purchases", purchases.Count);
            return purchases;
        }

        public Optional<Purchase> Save(Purchase entity)
        {
            logger.Trace("Saving Purchase: {0}", entity);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "INSERT INTO purchase (flightID, userID, touristID, clientAdress) VALUES (@flightID, @userID, @touristID, @clientAddress);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@flightID", entity.Flight.Id);
                    command.Parameters.AddWithValue("@userID", entity.User.Id);
                    command.Parameters.AddWithValue("@touristID", entity.Tourist.Id);
                    command.Parameters.AddWithValue("@clientAddress", entity.ClientAddress);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Saved {0} instances", rowsAffected);
                }
            }
            return Optional.Empty<Purchase>();
        }

        public Optional<Purchase> Delete(int id)
        {
            logger.Trace("Deleting Purchase with ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "DELETE FROM purchase WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Deleted {0} instances", rowsAffected);
                }
            }
            return Optional.Empty<Purchase>();
        }

        public Optional<Purchase> Update(int id, Purchase entity)
        {
            logger.Trace("Updating Purchase with ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "UPDATE purchase SET clientAdress=@clientAddress WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientAddress", entity.ClientAddress);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Updated {0} instances", rowsAffected);
                }
            }
            return Optional.Empty<Purchase>();
        }
    }
}