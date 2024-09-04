using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.Sqlite;



namespace QR
{
    internal class SqliteDataAccsess
    {
        private const string ConnectionString = "Data Source=OCRData.db";

        public SqliteDataAccsess()
        {
            CreateDatabaseAndTable();
        }

        private void CreateDatabaseAndTable()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                string tableCommand = @"CREATE TABLE IF NOT EXISTS OCRData (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT NOT NULL,
                                        Total TEXT NOT NULL,
                                        TotalTax TEXT NOT NULL,
                                        SaleReceiptNo TEXT NOT NULL)";

                using (var createTable = new SqliteCommand(tableCommand, connection))
                {
                    createTable.ExecuteNonQuery();
                }
            }
        }

        public void InsertOCRData(string date, string total, string totalTax, string saleReceiptNo)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                string insertCommand = "INSERT INTO OCRData (Date, Total, TotalTax, SaleReceiptNo) VALUES (@Date, @Total, @TotalTax, @SaleReceiptNo)";

                using (var command = new SqliteCommand(insertCommand, connection))
                {
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Total", total);
                    command.Parameters.AddWithValue("@TotalTax", totalTax);
                    command.Parameters.AddWithValue("@SaleReceiptNo", saleReceiptNo);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
