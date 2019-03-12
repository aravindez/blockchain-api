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
            connString = "server=127.0.0.1;uid=blockchain;pwd=blockchain;database=blockchain";
        }

        public List<BlockItem> GetBlocks()
        {
            List<BlockItem> blocks = new List<BlockItem>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.* FROM `blocks` AS b INNER JOIN users AS u ON b.created_by=u.id;";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BlockItem block = new BlockItem();
                    block.id = (int)reader[2];
                    block.previous_hash = (String)reader[3];
                    block.timestamp = (String)reader[4];
                    block.user_id = (int)reader[5];
                    block.user_name = (String)reader[0] + " " + (String)reader[1];
                    block.data = (int)reader[6];
                    block.hash = (String)reader[7];
                    block.nonce = (int)reader[8];
                    block.isValid = (int)reader[9];
                    blocks.Add(block);
                    Console.WriteLine("block");
                    Console.WriteLine(block);
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
                String query = "SELECT u.first_name as fname, u.last_name as lname, b.* FROM BLOCKS AS b INNER JOIN user AS u ON b.created_by=u.id WHERE b.id="+id.ToString()+";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader);
                    block.id = (int)reader[2];
                    block.previous_hash = (String)reader[3];
                    block.timestamp = (String)reader[4];
                    block.user_id = (int)reader[5];
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

        public bool PostNewBlock(BlockItem block, int chain_id)
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
                                + "'," + block.user_id.ToString()
                                + "," + block.data.ToString()
                                + ",'" + block.hash
                                + "'," + block.nonce.ToString()
                                + ");"
                                + "SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                String block_id = "0";
                while (reader.Read())
                {
                    block_id = (String)reader[0];
                }
                if (block_id == "0")
                {
                    throw new Exception("no id returned after insert");
                }

                // Get users with access to this chain
                query = "SELECT user_id FROM `block_chain_user` WHERE user_id!=" + block.user_id.ToString() + "block=0 AND chain_id=" + chain_id.ToString() + ";";
                command = new MySqlCommand(query, conn);
                reader = command.ExecuteReader();
                List<int> notifyUsers = new List<int>();
                while (reader.Read())
                {
                    notifyUsers.Add((int)reader[0]);
                }

                // Insert block into pending_blocks
                query = "INSERT INTO `pending_blocks` (block_id, chain_id, authorizer_id) VALUES ";
                for (int i=0; i<notifyUsers.Count; i++)
                {
                    query += "(" + block_id + ", " + chain_id.ToString() + ", " + notifyUsers[i].ToString() + "),";
                }
                query += "(" + block_id + ", " + chain_id.ToString() + ", " + notifyUsers[notifyUsers.Count].ToString() + ");";
                command = new MySqlCommand(query, conn);
                reader = command.ExecuteReader();

                reader.Close();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool PostValidation(int user_id, int block_id, int chain_id, Boolean valid)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                String query = "UPDATE `pending_blocks` SET isValid="+(valid ? "1" : "0")+";";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();
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
                String query = "SELECT isValid FROM `pending_blocks` ";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
