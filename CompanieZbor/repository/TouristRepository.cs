﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Ro.Mpp2024.Model;

namespace Ro.Mpp2024.Repository
{
    public class TouristRepository : IRepository<int, Tourist>
    {
        private readonly JdbcUtils dbUtils;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public TouristRepository(SqlConnectionStringBuilder props)
        {
            logger.Info("Initializing TouristRepository with properties: {0}", props.ConnectionString);
            dbUtils = new JdbcUtils(props);
        }

        public Optional<Tourist> FindOne(int id)
        {
            logger.Trace("Finding Tourist by ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "SELECT * FROM tourist WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader.GetString("name");
                            Tourist tourist = new Tourist(name);
                            tourist.Id = id;
                            logger.Trace("Found Tourist: {0}", tourist);
                            return Optional.Of(tourist);
                        }
                    }
                }
            }
            logger.Trace("Tourist not found with ID: {0}", id);
            return Optional.Empty<Tourist>();
        }

        public IEnumerable<Tourist> FindAll()
        {
            logger.Trace("Finding all Tourists");
            List<Tourist> tourists = new List<Tourist>();
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "SELECT * FROM tourist;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int touristId = reader.GetInt32("id");
                            string name = reader.GetString("name");
                            Tourist tourist = new Tourist(name);
                            tourist.Id = touristId;
                            tourists.Add(tourist);
                        }
                    }
                }
            }
            logger.Trace("Found {0} Tourists", tourists.Count);
            return tourists;
        }

        public Optional<Tourist> Save(Tourist entity)
        {
            logger.Trace("Saving Tourist: {0}", entity);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "INSERT INTO tourist (name) VALUES (@name);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", entity.Name);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Saved {0} instances", rowsAffected);
                }
            }
            return Optional.Empty<Tourist>();
        }

        public Optional<Tourist> Delete(int id)
        {
            logger.Trace("Deleting Tourist with ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "DELETE FROM tourist WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Deleted {0} instances", rowsAffected);
                }
            }
            return Optional.Empty<Tourist>();
        }

        public Optional<Tourist> Update(int id, Tourist entity)
        {
            logger.Trace("Updating Tourist with ID: {0}", id);
            using (SqlConnection connection = dbUtils.GetConnection())
            {
                string query = "UPDATE tourist SET name=@name WHERE id=@id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", entity.Name);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    logger.Trace("Updated {0} instances", rowsAffected);