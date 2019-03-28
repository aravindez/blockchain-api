using System;
using MySql.Data.MySqlClient;
using blockchainapi.Models;
using System.Collections.Generic;

namespace blockchainapi.Services
{
    public class UserService
    {
        private readonly String connString;

        public UserService()
        {
            connString = "server=127.0.0.1;uid=blockchain;pwd=blockchain;database=blockchain;pooling=false";
        }

        public UserItem GetUser(String email, String pword)
        {
            UserItem user = new UserItem();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT id, first_name, last_name, admin FROM users WHERE email='"+email+"' AND password='"+pword+"';";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.email = email;
                    user.id = (int)reader[0];
                    user.fname = (String)reader[1];
                    user.lname = (String)reader[2];
                    user.name = user.fname + " " + user.lname;
                    ulong temp = (ulong)reader[3];
                    user.admin = Convert.ToInt32(temp);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return user;
        }

        public List<UserItem> GetUsers(int user_id)
        {
            List<UserItem> users = new List<UserItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT id, email, first_name, last_name FROM users WHERE admin=0 and id!=" + user_id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserItem user = new UserItem();
                    user.id = (int)reader[0];
                    user.email = (String)reader[1];
                    user.fname = (String)reader[2];
                    user.lname = (String)reader[3];
                    user.name = user.fname + " " + user.lname;
                    users.Add(user);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return users;
        }

        public List<GroupItem> GetGroups(int user_id)
        {
            List<GroupItem> groups = new List<GroupItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT g.id, g.name FROM `groups` g " +
                	            "INNER JOIN `users_groups` ug " +
                	            "ON ug.group_id=g.id " +
                	            "WHERE ug.user_id=" + user_id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GroupItem group = new GroupItem();
                    group.id = (int)reader[0];
                    group.name = (string)reader[1];
                    groups.Add(group);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return groups;
        }

        public List<GroupItem> GetGroups()
        {
            List<GroupItem> groups = new List<GroupItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT g.id, g.name FROM `groups` g;";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GroupItem group = new GroupItem();
                    group.id = (int)reader[0];
                    group.name = (string)reader[1];
                    groups.Add(group);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return groups;
        }

        public List<UserItem> GetGroupUsers(int group_id, bool notIn)
        {
            List<UserItem> users = new List<UserItem>();
            List<UserItem> notUsers = new List<UserItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.id, u.email, u.first_name, u.last_name FROM `users` u " +
                	            "LEFT OUTER JOIN `users_groups` ug " +
                	            "ON u.id=ug.user_id " +
                	            "WHERE ug.group_id=" + group_id.ToString() + " and u.admin=0;";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserItem user = new UserItem();
                    user.id = (int)reader[0];
                    user.email = (String)reader[1];
                    user.fname = (String)reader[2];
                    user.lname = (String)reader[3];
                    user.name = user.fname + " " + user.lname;
                    users.Add(user);
                }
                reader.Close();

                if (notIn)
                {
                    query = "SELECT u.id, u.email, u.first_name, u.last_name FROM `users` u " +
                            "LEFT OUTER JOIN `users_groups` ug " +
                            "ON u.id=ug.user_id " +
                            "WHERE u.admin=0";
                    foreach(UserItem user in users)
                    {
                        query += " and u.id!=" + user.id.ToString();
                    }
                    query += ";";

                    command = new MySqlCommand(query, conn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UserItem user = new UserItem();
                        user.id = (int)reader[0];
                        user.email = (String)reader[1];
                        user.fname = (String)reader[2];
                        user.lname = (String)reader[3];
                        user.name = user.fname + " " + user.lname;
                        notUsers.Add(user);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return notIn ? notUsers : users;
        }

        public bool PostUser(UserItem user)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "INSERT INTO `users` (email, password, first_name, last_name) VALUES " +
                                "('" + user.email +
                                "', '" + user.password +
                                "', '" + user.fname +
                                "', '" + user.lname +
                                "');";
                MySqlCommand command = new MySqlCommand(query, conn);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("User wasn't properly inserted.");
                    return false;
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool PostUserGroup(UserGroup ug)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "INSERT INTO `users_groups` (user_id, group_id) " +
                	            "VALUES ("+ug.user_id.ToString()+", "+ug.group_id.ToString()+");";
                MySqlCommand command = new MySqlCommand(query, conn);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("UserGroup wasn't properly inserted.");
                    return false;
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool PostGroup(GroupItem group)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "INSERT INTO `groups` (name) VALUES ('"+group.name+"'); SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                int group_id = 0;
                while (reader.Read())
                {
                    group_id = Convert.ToInt32(reader[0]);
                    Console.WriteLine("ADDED GROUP ON ID: " +group_id.ToString());
                }
                if (group_id == 0)
                {
                    throw new Exception("no id returned after insert");
                }
                reader.Close();


                UserGroup ug = new UserGroup();
                ug.group_id = group_id;
                foreach (int user in group.users)
                {
                    ug.user_id = user;
                    bool error = PostUserGroup(ug);
                    if (!error) { return error; }
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
