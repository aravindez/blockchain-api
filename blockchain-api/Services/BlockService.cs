using MySql.Data.MySqlClient;
using blockchainapi.Models;
using System;
using System.Collections.Generic;

namespace blockchainapi.Services
{
    public class BlockService
    {
        private readonly String connString;

        public BlockService()
        {
            connString = "server=127.0.0.1;uid=blockchain;pwd=blockchain;database=blockchain;pooling=false";
        }

        public List<BlockItem> GetBlocks()
        {
            List<BlockItem> blocks = new List<BlockItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                //TODO UPDATE QUERY TO INCLUDE INNER JOIN WITH `chains` TO GET CHAIN_ID
                // Perform database operations
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.* FROM `blocks` AS b INNER JOIN users AS u ON b.created_by=u.id;";
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

        public BlockItem GetBlock(int id)
        {
            BlockItem block = new BlockItem();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.* FROM BLOCKS AS b INNER JOIN user AS u ON b.created_by=u.id WHERE b.id=" + id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    block.id = (int)reader[2];
                    block.previous_hash = (String)reader[3];
                    block.timestamp = (String)reader[4];
                    block.created_by = (int)reader[5];
                    block.user_name = (String)reader[0] + " " + (String)reader[1];
                    block.data = (int)reader[6];
                    block.hash = (String)reader[7];
                    block.nonce = (int)reader[8];
                    block.isValid = (int)reader[9];
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return block;
        }

        public int PostInitBlock(BlockItem block)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                // Insert block into blocks
                String query = "INSERT INTO `blocks` (previous_hash, created_by, data, hash, nonce, isValid) VALUES "
                                + "('" + block.previous_hash
                                + "'," + block.created_by.ToString()
                                + "," + block.data.ToString()
                                + ",'" + block.hash
                                + "'," + block.nonce.ToString()
                                + ", 1);"
                                + " SELECT LAST_INSERT_ID();";
                Console.WriteLine(query);
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                int block_id = 0;
                while (reader.Read())
                {
                    block_id = Convert.ToInt32(reader[0]);
                    Console.WriteLine("ADDED BLOCK ON ID: " + block_id.ToString());
                }
                if (block_id == 0)
                {
                    throw new Exception("no id returned after insert");
                }
                reader.Close();

                query = "INSERT INTO `block_chain_user` (block_id, chain_id, block) VALUES (" + block_id.ToString() + ", " + block.chain_id.ToString() + ", 1);";
                command = new MySqlCommand(query, conn);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("failed to properly insert block into db.");
                    return 0;
                }
                Console.WriteLine("init block success.");
                return block_id;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public int PostNewBlock(BlockItem block)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                // Insert block into blocks
                String query = "INSERT INTO `blocks` (previous_hash, created_by, data, hash, nonce) VALUES "
                                + "('" + block.previous_hash
                                + "'," + block.created_by.ToString()
                                + "," + block.data.ToString()
                                + ",'" + block.hash
                                + "'," + block.nonce.ToString()
                                + ");"
                                + "SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                int block_id = 0;
                while (reader.Read())
                {
                    block_id = Convert.ToInt32(reader[0]);
                }
                if (block_id == 0)
                {
                    throw new Exception("no id returned after insert");
                }
                reader.Close();

                // Get users with access to this chain
                query = "SELECT user_id FROM `block_chain_user` WHERE user_id!=" + block.created_by.ToString() + " AND block=0 AND chain_id=" + block.chain_id.ToString() + ";";
                command = new MySqlCommand(query, conn);
                reader = command.ExecuteReader();
                List<int> notifyUsers = new List<int>();
                while (reader.Read())
                {
                    notifyUsers.Add((int)reader[0]);
                }
                reader.Close();

                // Insert block into pending_blocks
                if (notifyUsers.Count > 0)
                {
                    query = "INSERT INTO `pending_blocks` (block_id, chain_id, authorizer_id) VALUES ";
                    Console.WriteLine(notifyUsers.Count);
                    if (notifyUsers.Count > 1)
                    {
                        for (int i = 0; i < notifyUsers.Count - 1; i++)
                        {
                            query += "(" + block_id.ToString() + ", " + block.chain_id.ToString() + ", " + notifyUsers[i].ToString() + "),";
                        }
                        query += "(" + block_id.ToString() + ", " + block.chain_id.ToString() + ", " + notifyUsers[notifyUsers.Count - 1].ToString() + ");";
                    }
                    else
                    {
                        query += "(" + block_id.ToString() + ", " + block.chain_id.ToString() + ", " + notifyUsers[0].ToString() + ");";
                    }
                    command = new MySqlCommand(query, conn);
                    int insertPendingBlocks = command.ExecuteNonQuery();
                    if (insertPendingBlocks != notifyUsers.Count) { Console.WriteLine("failed to insert into pending_blocks."); return 0; }
                }

                // Insert block into block_chain_user
                query = "INSERT INTO `block_chain_user` (block_id, chain_id, block) VALUES (" + block_id.ToString() + ", " + block.chain_id.ToString() + ", 1);";
                command = new MySqlCommand(query, conn);
                int insertBlockChainUser = command.ExecuteNonQuery();
                if (insertBlockChainUser != 1) { Console.WriteLine("failed to properly insert into block_chain_user."); return 0; }

                reader.Close();
                conn.Close();
                return block_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public bool PostValidation(ValidationItem validation)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "UPDATE `pending_blocks` SET isValid=" + validation.isValid.ToString()
                    + " WHERE authorizer_id=" + validation.user_id.ToString() + " AND block_id=" + validation.block_id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                int updatePendingBlocks = command.ExecuteNonQuery();
                if (updatePendingBlocks != 1) { Console.WriteLine("failed to properly update pending blocks."); return false; }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public String GetValidation(int block_id)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT isValid FROM `pending_blocks` WHERE block_id=" + block_id.ToString() + ";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                bool isValid = true;
                while (reader.Read())
                {
                    if (Convert.IsDBNull(reader[0]))
                    {
                        return "pending";
                    }
                    if ((ulong)reader[0] == 0)
                    {
                        isValid = false;
                    }
                }
                reader.Close();

                query = "UPDATE `blocks` SET isValid=" + (isValid ? "1" : "0") + " WHERE id=" + block_id.ToString() + ";";
                command = new MySqlCommand(query, conn);
                int updateBlocks = command.ExecuteNonQuery();
                if (updateBlocks != 1) { Console.WriteLine("failed to properly update blocks."); return "failed"; }

                conn.Close();

                return (isValid ? "valid" : "invalid");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "failed";
            }
        }

        public List<BlockItem> GetPendingBlocks(int user_id)
        {
            List<BlockItem> pendingBlocks = new List<BlockItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.first_name, u.last_name, b.* FROM `blocks` as b " +
                	            "INNER JOIN `users` AS u " +
                	            "ON u.id=b.created_by " +
                	            "INNER JOIN `pending_blocks` AS pb " +
                	            "ON b.id=pb.block_id " +
                	            "WHERE pb.authorizer_id="+user_id.ToString()+" " +
                	            "AND pb.isValid IS NULL;";
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
                    pendingBlocks.Add(block);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return pendingBlocks;
        }
    }
}
