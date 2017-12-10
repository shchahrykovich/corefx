﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Tests;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlConnectionBasicTests
{
    public class SqlQueryTest
    {
        [Fact]
        public void SelectNTextRow()
        {
            using (TestTdsServer server = TestTdsServer.StartTestServer())
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT [name], [description] FROM MyTable";

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Assert.Equal("Name-Name", reader.GetString(0));
                            Assert.Equal("Description", reader.GetTextReader(1).ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
