using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Server.utils;
using Utils.model;

namespace Server.repository
{
    public class UserRepository : IRepository<int, User>
    {
        private static readonly ILog log = LogManager.GetLogger("User Repository");
        IDictionary<String, string> props;

        public UserRepository(IDictionary<String, string> props)
        {
            log.Info("Creating UserRepository ");
            this.props = props;
        }

        public User findOne(int id)
        {
            log.InfoFormat("Finding User by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM user WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        User u = new User(username, password);
                        u.Id = id;
                        log.InfoFormat("Found User: {0}", u );
                        return u;

                    }
                }
            }
            log.InfoFormat("User not found with ID: {0}", id);
            return null;
        }

        public User findByUsername(string name)
        {
            log.InfoFormat("Finding User by Username: {0}", name);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM user WHERE username=@username; ";
                IDbDataParameter usernameParam = comm.CreateParameter();
                usernameParam.ParameterName = "@username";
                usernameParam.Value = name;
                comm.Parameters.Add(usernameParam);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        User u = new User(username, password);
                        u.Id = id;
                        log.InfoFormat("Found User: {0}", u);
                        return u;

                    }
                }
            }
            log.InfoFormat("User not found with username: {0}", name);
            return null;
        }

        public IEnumerable<User> findAll()
        {
            log.InfoFormat("Finding All Users");
            IDbConnection con = DBUtils.getConnection(props);
            IList<User> users = new List<User>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM user; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        User u = new User(username, password);
                        u.Id = id;
                        users.Add(u);
                    }
                }
            }
            return users;
        }

        public void save(User entity)
        {
            log.InfoFormat("Saving User: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO user (username, password) VALUES (@username, @password);";

                var username = command.CreateParameter();
                username.ParameterName = "@username";
                username.Value = entity.Username;
                command.Parameters.Add(username);

                var password= command.CreateParameter();
                password.ParameterName = "@password";
                password.Value = entity.Password;
                command.Parameters.Add(password);
           
                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("User {0} NOT saved", entity);
                    throw new Exception("No user added!");

                }
                log.InfoFormat("User{0} saved", entity);

            }
        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting User with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM user WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("User with ID {0} NOT deleted", id);
                    throw new Exception("No deleted user!");
                }
            }
        }

        public void update(User entity)
        {
       
        }
    }

}
