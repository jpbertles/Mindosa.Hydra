using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Mindosa.Hydra.Tests.Infrastructure;
using NUnit.Framework;

namespace Mindosa.Hydra.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private const string DatabaseConnName = "Db";
        private const string DatabaseName = "TestDatabase";
       
        #region private methods

        protected static void ExecuteNonQuery(string sql)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DatabaseConnName].ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            UnitTestStartAndEnd.Start(DatabaseName);

            var customObject = new CustomObject()
            {
                IntegerField = 42,
                StringField = "The answer to everything..."
            };

            var customXml = string.Empty;
            var stringBuilder = new StringBuilder();

            using(var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                new XmlSerializer(typeof(CustomObject)).Serialize(writer, customObject, ns);
                customXml = stringBuilder.ToString();
            }

            ExecuteNonQuery("CREATE Table TestObject (id int not null, name varchar(250), location geography, customData xml)");
            ExecuteNonQuery("INSERT INTO TestObject (id, name, location, customData) values(1, 'One', geography::STGeomFromText('Point(-122.360 47.656)', 4326), '" + customXml + "')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name, location, customData) values(2, 'Two', geography::STGeomFromText('Point(-122.360 47.656)', 4326), '" + customXml + "')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name, location, customData) values(3, 'Three', geography::STGeomFromText('Point(-122.360 47.656)', 4326), '" + customXml + "')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name, location, customData) values(4, 'Four', geography::STGeomFromText('Point(-122.360 47.656)', 4326), '" + customXml + "')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name, location, customData) values(5, 'Five', geography::STGeomFromText('Point(-122.360 47.656)', 4326), '" + customXml + "')");
        }

        [Test]
        public void TestingSynchronousHydration()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DatabaseConnName].ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"select id, name, id * 4 as Comp, Location, CustomData from TestObject order by id; 
                                        select top 1 id, name, id * 4 as Comp, Location, CustomData from TestObject order by id desc;";

                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var resultSet = reader.Hydrate<TestObject>();

                        var secondResultSet = reader.Hydrate<TestObject>();

                        Assert.AreEqual(5, resultSet.Count);
                        Assert.AreEqual("One", resultSet.First().Name);
                        Assert.AreEqual(4, resultSet.First().Comp);
                        Assert.AreEqual("Id: 1", resultSet.First().StringId);
                        Assert.AreEqual(42, resultSet.First().CustomData.IntegerField);

                        Assert.AreEqual(1, secondResultSet.Count);
                        Assert.AreEqual("Id: 5", secondResultSet.Single().StringId);
                    }
                }
            }
        }

        [Test]
        public async void TestingAsyncHydration()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DatabaseConnName].ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"select id, name, id * 4 as Comp, Location, CustomData from TestObject order by id; 
                                        select top 1 id, name, id * 4 as Comp, Location, CustomData from TestObject order by id desc;";

                    cmd.Connection.Open();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var resultSet = await reader.HydrateAsync<TestObject>();

                        var secondResultSet = await reader.HydrateAsync<TestObject>();

                        Assert.AreEqual(5, resultSet.Count);
                        Assert.AreEqual("One", resultSet.First().Name);
                        Assert.AreEqual(4, resultSet.First().Comp);
                        Assert.AreEqual("Id: 1", resultSet.First().StringId);
                        Assert.AreEqual(42, resultSet.First().CustomData.IntegerField);

                        Assert.AreEqual(1, secondResultSet.Count);
                        Assert.AreEqual("Id: 5", secondResultSet.Single().StringId);
                    }
                }
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            UnitTestStartAndEnd.End(DatabaseName);
        }
    }    
}
