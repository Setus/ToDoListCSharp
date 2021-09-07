using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WebAPI.integrationlayer
{
    public class AzureSQLDBConnection : IDBConnection
    {

        private string Server { get; set; } = "";
        private string InitialCatalog { get; set; } = "todolistDatabase";
        private string PersistSecurityInfo { get; set; } = "False";
        private string UserID { get; set; } = "";
        private string Password { get; set; } = "";
        private string MultipleActiveResultSets { get; set; } = "False";
        private string Encrypt { get; set; } = "True";
        private string TrustServerCertificate { get; set; } = "False";
        private string ConnectionTimeout { get; set; } = "30";

        private SqlConnection azureConnection { get; set; }

        public void CreateNewConnection()
        {
            if (azureConnection == null)
            {
                azureConnection = new SqlConnection(string.Format(
                    "Server={0};" +
                    "Initial Catalog={1};" +
                    "Persist Security Info={2};" +
                    "User ID={3};" +
                    "Password={4};" +
                    "MultipleActiveResultSets={5};" +
                    "Encrypt={6};" +
                    "TrustServerCertificate={7};" +
                    "Connection Timeout={8};",
                    Server, InitialCatalog, PersistSecurityInfo, UserID, Password,
                    MultipleActiveResultSets, Encrypt, TrustServerCertificate, ConnectionTimeout));
                azureConnection.Open();
            }
        }

        public void RunSqlTransaction(String[] sqlCommand)
        {
            CreateNewConnection();

            SqlCommand myCommand = azureConnection.CreateCommand();
            SqlTransaction myTransaction = azureConnection.BeginTransaction();
            myCommand.Connection = azureConnection;
            myCommand.Transaction = myTransaction;

            try
            {
                for (int i = 0; i < sqlCommand.Length; i++)
                {
                    Console.WriteLine(sqlCommand[i]);
                    myCommand.CommandText = sqlCommand[i];
                    myCommand.ExecuteNonQuery();
                }
                myTransaction.Commit();
            }
            catch (Exception e)
            {
                try
                {
                    myTransaction.Rollback();
                }
                catch (SqlException ex)
                {
                    if (myTransaction.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                        Console.WriteLine("Rollback exception details: " + ex.GetBaseException());
                        azureConnection.Close();
                    }
                }

                Console.WriteLine("An exception of type " + e.GetType() +
                " was encountered while inserting data.");
                Console.WriteLine("Exception details: " + e.GetBaseException());
                azureConnection.Close();
            }
        }

        public void AddNewItem(Item item)
        {
            Console.WriteLine("AzureSQL, called AddNewItem, with the new object: " + item.itemId + ", " + item.itemName + ", " + item.done);
            string[] sqlStrings = { string.Format("INSERT INTO [dbo].[Item]([itemId], [itemName], [done]) VALUES('{0}', '{1}', '{2}')", item.itemId, item.itemName, item.done ? 1 : 0) };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public void UpdateItem(Item item)
        {
            Console.WriteLine("AzureSQL, called UpdateItem()");
            string[] sqlStrings = { string.Format("UPDATE [dbo].[Item] SET [itemName] = '{0}', [done] = '{1}' WHERE [itemId] = '{2}'", item.itemName, item.done ? 1 : 0, item.itemId) };
            RunSqlTransaction(sqlStrings);
            Close();
        }


        public void DeleteItem(int id)
        {
            Console.WriteLine("AzureSQL, called DeleteItem() with itemId: " + id);
            string[] sqlStrings = { string.Format("DELETE FROM [dbo].[Item] WHERE [itemId] = '{0}'", id) };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public void DeleteAllDoneItems()
        {
            Console.WriteLine("AzureSQL, called DeleteAllDoneItems()");
            string[] sqlStrings = { string.Format("DELETE FROM [dbo].[Item] WHERE [done] = 1") };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public void DeleteAllItems()
        {
            Console.WriteLine("AzureSQL, called DeleteAllItems()");
            string[] sqlStrings = { string.Format("DELETE FROM [dbo].[Item]") };
            RunSqlTransaction(sqlStrings);
            Close();
        }

        public List<Item> GetAllItems()
        {
            Console.WriteLine("AzureSQL, called GetAllItems()");
            CreateNewConnection();
            List<string[]> tableData = new();
            try
            {
                string query = "SELECT [itemId], [itemName], [done] FROM [dbo].[Item] ORDER BY [itemId] ASC";
                var cmd = new SqlCommand(query, azureConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string[] columns = new string[3];
                    columns[0] = reader.GetSqlInt32(0).ToString();
                    columns[1] = reader.GetSqlString(1).ToString();
                    columns[2] = reader.GetSqlByte(2).ToString();
                    tableData.Add(columns);
                    Console.WriteLine(columns[0] + ", " + columns[1] + ", " + columns[2]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was encountered of type " + e.GetType());
                Console.WriteLine("Exception details: " + e.GetBaseException());
                azureConnection.Close();
            }
            return ItemMapper.MapToListOfItems(tableData);
        }

        public Item GetSingleItem(int id)
        {
            Console.WriteLine("AzureSQL, called GetSingleItem()");
            CreateNewConnection();
            string[] itemColumn = new string[3];
            try
            {
                string query = string.Format("SELECT [itemId], [itemName], [done] FROM [dbo].[Item] WHERE [itemId] = '{0}'", id);
                var cmd = new SqlCommand(query, azureConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemColumn[0] = reader.GetSqlInt32(0).ToString();
                    itemColumn[1] = reader.GetSqlString(1).ToString();
                    itemColumn[2] = reader.GetSqlByte(2).ToString();
                    //Console.WriteLine(itemColumn[0] + ", " + itemColumn[1] + ", " + itemColumn[2]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was encountered of type " + e.GetType());
                Console.WriteLine("Exception details: " + e.GetBaseException());
                azureConnection.Close();
            }
            string[] item = String.IsNullOrEmpty(itemColumn[0]) ? null : itemColumn;
            return ItemMapper.MapToItem(item);
        }

        public void OutputDB()
        {
            CreateNewConnection();
            try
            {
                string query = "SELECT [itemId], [itemName], [done] FROM [dbo].[Item]";
                var cmd = new SqlCommand(query, azureConnection);
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
                azureConnection.Close();
            }
        }

        public void Close()
        {
            azureConnection.Close();
            azureConnection = null;
        }

    }
}
