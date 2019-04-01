using System;
using MySql.Data.MySqlClient;
using blockchainapi.Models;
using System.Collections.Generic;

namespace blockchainapi.Services
{
    public class ChainService
    {
        private readonly String connString;

        public ChainService()
        {
            connString = "server=127.0.0.1;uid=blockchain;pwd=blockchain;database=blockchain;pooling=false";
        }

        public List<ChainItem> GetChainItems(int user_id)
        {
            List<ChainItem> chains = new List<ChainItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT c.* FROM chains AS c INNER JOIN block_chain_user AS bcu ON bcu.chain_id=c.id WHERE user_id=" + user_id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ChainItem chain = new ChainItem();
                    //ulong temp = (ulong)reader[0];
                    chain.id = Convert.ToInt32(reader[0]);
                    chain.name = (string)reader[1];
                    //temp = (ulong)reader[2];
                    chain.created_by = Convert.ToInt32(reader[2]);
                    chains.Add(chain);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return chains;
        }

        public ChainItem GetChainItem(int id)
        {
            ChainItem chain = new ChainItem();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT id, name, created_by FROM chains WHERE id=" + id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ulong temp = (ulong)reader[0];
                    chain.id = Convert.ToInt32(temp);
                    chain.name = (string)reader[1];
                    temp = (ulong)reader[2];
                    chain.created_by = Convert.ToInt32(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return chain;
        }

        public List<BlockItem> GetChain(int chain_id, bool store)
        {
            List<BlockItem> blocks = new List<BlockItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.*, bcu.chain_id " +
                	            "FROM `blocks` AS b " +
                	            "INNER JOIN users AS u ON b.created_by=u.id " +
                	            "INNER JOIN block_chain_user AS bcu ON bcu.block_id=b.id " +
                	            "WHERE chain_id="+chain_id.ToString()+";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BlockItem block = new BlockItem();
                    block.id = (int)reader[2];
                    block.previous_hash = (String)reader[3];
                    DateTime tempTimestamp = (DateTime)reader[4];
                    block.timestamp = tempTimestamp.ToString();
                    block.created_by = (int)reader[5];
                    block.user_name = (String)reader[0] + " " + (String)reader[1];
                    block.data = (int)reader[6];
                    block.hash = (String)reader[7];
                    block.nonce = (int)reader[8];
                    if (reader[9] != System.DBNull.Value)
                    {
                        ulong tempValid = (ulong)reader[9];
                        block.isValid = Convert.ToInt32(tempValid);
                    }
                    blocks.Add(block);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            List<BlockItem> trim = new List<BlockItem>();
            if (blocks.Count < 5) { trim = blocks; }
            else { trim = blocks.GetRange(blocks.Count-5, 5); }

            return store ? trim : blocks;
        }

        public List<BlockItem> GetValidChain(int chain_id)
        {
            List<BlockItem> blocks = new List<BlockItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.*, bcu.chain_id " +
                                "FROM `blocks` AS b " +
                                "INNER JOIN users AS u ON b.created_by=u.id " +
                                "INNER JOIN block_chain_user AS bcu ON bcu.block_id=b.id " +
                                "WHERE bcu.chain_id=" + chain_id.ToString() + " " +
                                "AND b.isValid=1;";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BlockItem block = new BlockItem();
                    block.id = (int)reader[2];
                    block.previous_hash = (String)reader[3];
                    DateTime tempTimestamp = (DateTime)reader[4];
                    block.timestamp = tempTimestamp.ToString();
                    block.created_by = (int)reader[5];
                    block.user_name = (String)reader[0] + " " + (String)reader[1];
                    block.data = (int)reader[6];
                    block.hash = (String)reader[7];
                    block.nonce = (int)reader[8];
                    if (reader[9] != System.DBNull.Value)
                    {
                        ulong tempValid = (ulong)reader[9];
                        block.isValid = Convert.ToInt32(tempValid);
                    }
                    blocks.Add(block);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return blocks;
        }

        public int PostChain(ChainItem chain)
        {
            Console.WriteLine(chain.groups);
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "INSERT INTO `chains` (name, created_by) " +
                                "VALUES ('" + chain.name + "', " + chain.created_by.ToString() + ");" +
                                "SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                int chain_id = 0;
                while (reader.Read())
                {
                    chain_id = Convert.ToInt32(reader[0]);
                }
                reader.Close();

                if (chain_id == 0)
                {
                    Console.WriteLine("failed to insert chain into db.");
                    return 0;
                }

                int rowsAffected;
                query = "INSERT INTO `groups_chains` (group_id, chain_id) VALUES ";
                for (int i=0; i<chain.groups.Count; i++)
                {
                    command = new MySqlCommand(query+"("+chain.groups[i]+", "+chain_id.ToString()+");", conn);
                    rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected != 1)
                    {
                        Console.WriteLine("failed to properly insert chain into db.");
                        return 0;
                    }
                }

                if (!chain.users.Contains(chain.created_by))
                {
                    query = "INSERT INTO `block_chain_user` (chain_id, user_id, block)" +
                            "VALUES (" + chain_id.ToString() + ", " + chain.created_by.ToString() + ", 0);";
                    command = new MySqlCommand(query, conn);
                    rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected != 1)
                    {
                        Console.WriteLine("failed to properly insert chain into db.");
                        return 0;
                    }
                }
                for (int i=0; i<chain.users.Count; i++)
                {
                    query = "INSERT INTO `block_chain_user` (chain_id, user_id, block)" +
                        "VALUES (" + chain_id.ToString() + ", " + chain.users[i].ToString() + ", 0);";
                    command = new MySqlCommand(query, conn);
                    rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected != 1)
                    {
                        Console.WriteLine("failed to properly insert chain into db.");
                        return 0;
                    }
                }

                return chain_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}