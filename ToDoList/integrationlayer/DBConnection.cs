using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList

{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        public static string Server { get; set; }
        public static string Port { get; set; }
        public static string DatabaseName { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }

        public MySqlConnection mySQLConnection { get; set; }

        private static readonly DBConnection _instance = new DBConnection();

        public static DBConnection GetSingletonInstance()
        {
            return _instance;
        }

        private static void ConnectionDetails()
        {
            Server = "localhost";
            Port = "3306";
            DatabaseName = "toDoListSchema";
            UserName = "devuser";
            Password = "abc123";
        }

        public void CreateNewConnection()
        {
            if (mySQLConnection == null)
            {
                ConnectionDetails();
                mySQLConnection = new MySqlConnection(string.Format(
                    "Server={0}; Port={1}; database={2}; UID={3}; password={4}",
                    Server, Port, DatabaseName, UserName, Password));
                mySQLConnection.Open();
            }
        }

        public void RunSqlTransaction(String[] sqlCommand)
        {
            CreateNewConnection();

            MySqlCommand myCommand = mySQLConnection.CreateCommand();

            // Start a local transaction
            MySqlTransaction myTrans = mySQLConnection.BeginTransaction();

            // Must assign both transaction object and connection
            // to Command object for a pending local transaction
            myCommand.Connection = mySQLConnection;
            myCommand.Transaction = myTrans;

            try
            {
                for (int i = 0; i < sqlCommand.Length; i++)
                {
                    Console.WriteLine(sqlCommand[i]);
                    myCommand.CommandText = sqlCommand[i];
                    myCommand.ExecuteNonQuery();
                }  
                myTrans.Commit();
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (MySqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                        Console.WriteLine("Rollback exception details: " + ex.GetBaseException());
                        mySQLConnection.Close();
                    }
                }

                Console.WriteLine("An exception of type " + e.GetType() +
                " was encountered while inserting data.");
                Console.WriteLine("Exception details: " + e.GetBaseException());
                mySQLConnection.Close();
            }
        }

        public void AddNewItem(int id, string itemName, int done)
        {
            string[] sqlStrings = { string.Format("INSERT INTO Item(itemId, itemName, done) VALUES('{0}', '{1}', '{2}')", id, itemName, done) };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public void UpdateItem(int id, string itemName, int done)
        {
            string[] sqlStrings = { string.Format("UPDATE Item SET itemName = '{0}', done = '{1}' WHERE itemId = '{2}'", itemName, done, id) };
            RunSqlTransaction(sqlStrings);
            Close();
        }


        public void DeleteItem(int id)
        {
            string[] sqlStrings = { string.Format("DELETE FROM Item WHERE itemId = '{0}'", id) };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public void DeleteAllDoneItems()
        {
            string[] sqlStrings = { string.Format("DELETE FROM Item WHERE done = 1") };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public List<string[]> GetAllItems()
        {
            CreateNewConnection();
            List<string[]> tableData = new();
            try
            {
                string query = "SELECT itemId, itemName, done FROM Item ORDER BY itemId ASC";
                var cmd = new MySqlCommand(query, mySQLConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string[] columns = new string[3];
                    columns[0] = reader.GetString(0);
                    columns[1] = reader.GetString(1);
                    columns[2] = reader.GetString(2);
                    tableData.Add(columns);
                    Console.WriteLine(columns[0] + ", " + columns[1] + ", " + columns[2]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was encountered of type " + e.GetType());
                Console.WriteLine("Exception details: " + e.GetBaseException());
                mySQLConnection.Close();
            }
            return tableData;
        }

        public string[] GetItemWithId(int id)
        {
            CreateNewConnection();
            string[] itemColumn = new string[3];
            try
            {
                string query = string.Format("SELECT itemId, itemName, done FROM Item WHERE itemId = '{0}'", id);
                var cmd = new MySqlCommand(query, mySQLConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemColumn[0] = reader.GetString(0);
                    itemColumn[1] = reader.GetString(1);
                    itemColumn[2] = reader.GetString(2);
                    //Console.WriteLine(itemColumn[0] + ", " + itemColumn[1] + ", " + itemColumn[2]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was encountered of type " + e.GetType());
                Console.WriteLine("Exception details: " + e.GetBaseException());
                mySQLConnection.Close();
            }
            return String.IsNullOrEmpty(itemColumn[0]) ? null : itemColumn;
        }

        public void OutputDB()
        {
            CreateNewConnection();
            try
            {
                string query = "SELECT itemId, itemName, done FROM Item";
                var cmd = new MySqlCommand(query, mySQLConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string col0 = reader.GetString(0);
                    string col1 = reader.GetString(1);
                    string col2 = reader.GetString(2);
                    Console.WriteLine(col0 + ", " + col1 + ", " + col2);
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was encountered of type " + e.GetType());
                Console.WriteLine("Exception details: " + e.GetBaseException());
                mySQLConnection.Close();
            }
        }

        public void Close()
        {
            mySQLConnection.Close();
            mySQLConnection = null;
        }
    }
}