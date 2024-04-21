using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Server.utils;
using Utils.model;

namespace Server.repository
{
    public class TouristRepository : IRepository<int, Tourist>
    {
        private static readonly ILog log = LogManager.GetLogger("Tourist Repository");

        IDictionary<String, string> props;

        public TouristRepository(IDictionary<String, string> props)
        {
            log.Info("Creating TouristRepository ");
            this.props = props;
        }


        public Tourist findOne(int id)
        {
            log.InfoFormat("Finding Tourist by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM tourist WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string name = dataR.GetString(1);
                        Tourist tourist = new Tourist(name);
                        tourist.Id = id;
                        log.InfoFormat("Found Tourist: {0}", tourist);
                        return tourist;

                    }
                }
            }
            log.InfoFormat("Tourist not found with ID: {0}", id);
            return null;
        }

        public Tourist findByName(string touristName)
        {
            log.InfoFormat("Finding Tourist by Name: {0}", touristName);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM tourist WHERE name=@name; ";
                IDbDataParameter name = comm.CreateParameter();
                name.ParameterName = "@name";
                name.Value = touristName;
                comm.Parameters.Add(name);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string n = dataR.GetString(1);
                        Tourist tourist = new Tourist(n);
                        tourist.Id = id;
                        log.InfoFormat("Found Tourist: {0}", tourist);
                        return tourist;

                    }
                }
            }
            log.InfoFormat("Tourist not found with name: {0}", touristName);
            return null;
        }

        public IEnumerable<Tourist> findAll()
        {
            log.InfoFormat("Finding All Tourists");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Tourist> tourists = new List<Tourist>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM tourist; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string name = dataR.GetString(1);
                        Tourist tourist = new Tourist(name);
                        tourist.Id = id;
                        tourists.Add(tourist);

                    }
                }
            }
            return tourists;
        }

        public void save(Tourist entity)
        {
            log.InfoFormat("Saving Tourist: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO tourist (name) VALUES (@name);";

                var name = command.CreateParameter();
                name.ParameterName = "@name";
                name.Value = entity.TouristName;
                command.Parameters.Add(name);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Tourist {0} NOT saved", entity);
                    throw new Exception("No tourist added!");

                }
                log.InfoFormat("Tourist {0} saved", entity);

            }
        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting Tourist with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM tourist WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Tourist with ID {0} NOT deleted", id);
                    throw new Exception("No deleted tourist!");
                }
            }

        }

        public void update(Tourist entity)
        {
            log.InfoFormat("Updating Flight with ID: {0}", entity.Id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE tourist SET name=@name WHERE id=@id;";
                IDbDataParameter name = command.CreateParameter();
                name.ParameterName = "@name";
                name.Value = entity.TouristName;
                command.Parameters.Add(name);

                IDbDataParameter id = command.CreateParameter();
                id.ParameterName = "@id";
                id.Value = entity.Id;
                command.Parameters.Add(id);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Tourist {0} NOT updated", entity);
                    throw new Exception("Tourist NOT updated");
                }
            }
        }
    }
}