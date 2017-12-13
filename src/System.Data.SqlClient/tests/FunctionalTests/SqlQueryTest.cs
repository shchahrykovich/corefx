using System;
using System.Collections.Generic;
using System.Data;
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

        [Fact]
        public void SelectXmlRow()
        {
            using (TestTdsServer server = TestTdsServer.StartTestServer())
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT [xml-field] FROM MyTable";

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var xml = reader.GetSqlXml(0);
                            Assert.Equal("<books><book><id>2</id></book></books>", xml.Value);
                        }
                    }
                }
            }
        }

        [Fact]
        public void SendXmlRow()
        {
            using (TestTdsServer server = TestTdsServer.StartTestServer())
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT [xml-field] FROM MyTable WHERE [old-xml-field] = @OldXmlField";

                    var oldFieldParameter = new SqlParameter("@OldXmlField", SqlDbType.Xml)
                    {
                        Value = "<test></test>"
                    };
                    cmd.Parameters.Add(oldFieldParameter);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        Assert.Equal(false, reader.Read());
                    }
                }
            }
        }
    }
}
