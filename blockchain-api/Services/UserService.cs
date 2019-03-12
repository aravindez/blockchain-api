using System;
using MySql.Data.MySqlClient;
using blockchainapi.Models;

namespace blockchainapi.Services
{
    public class UserService
    {
        private readonly String connString;

        public UserService()
        {
            connString = "server=127.0.0.1;uid=blockchain;pwd=blockchain;database=blockchain";
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
                String query = "SELECT id, first_name, last_name FROM users WHERE email='"+email+"' AND password='"+pword+"';";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.email = email;
                    user.id = (int)reader[0];
                    user.name = (String)reader[1] + " " + (String)reader[2];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return user;
        }

        public bool PostUser(UserItem user)
        {
            //insert user into db
            while (false)
            {
                return true;
            }
            return false;
        }
    }
}
