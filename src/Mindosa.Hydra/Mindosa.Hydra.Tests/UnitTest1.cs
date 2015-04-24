using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Mindosa.Hydra.Attributes;
using Mindosa.Hydra.Internal;
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
            ExecuteNonQuery("CREATE Table TestObject (id int not null, name varchar(250))");
            ExecuteNonQuery("INSERT INTO TestObject (id, name) values(1, 'One')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name) values(2, 'Two')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name) values(3, 'Three')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name) values(4, 'Four')");
            ExecuteNonQuery("INSERT INTO TestObject (id, name) values(5, 'Five')");
        }

        [Test]
        public void TestingSynchronousHydration()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DatabaseConnName].ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select id, name, id * 4 as Comp from TestObject order by id";

                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var data = reader.Hydrate<TestObject>();

                        Assert.AreEqual(5, data.Count);
                        Assert.AreEqual("One", data.First().Name);
                        Assert.AreEqual(4, data.First().Comp);
                        Assert.AreEqual("Id: 1", data.First().StringId);
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

    public class TestObject
    {
        public string Name { get; set; }
        //[ComputedColumn]
        public int Comp { get; set; }
        [CustomMapping("Id", typeof(IdMapper))]
        public string StringId { get; set; }
    }

    public class IdMapper: ICustomMapper
    {

        public Func<object, object> GetMapper()
        {
            return x => "Id: " + x;
        }
    }
}
