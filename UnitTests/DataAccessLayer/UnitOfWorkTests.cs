using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace DataAccessLayer.Tests
{
    [TestClass()]
    public class UnitOfWorkTests
    {
        [TestInitialize()]
        public void DropAllTables()
        {
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString;
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\sasha\Documents\Visual Studio 2015\Projects\Epam\CatalogProject\CatalogOfMaterials\UnitTests\TestDBs\testDB.mdf; Integrated Security = True; Connect Timeout = 30";
            using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                conn.Open();
                var command = new System.Data.SqlClient.SqlCommand("DECLARE @sql NVARCHAR(max)='' SELECT @sql += ' Drop table [' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']' FROM   INFORMATION_SCHEMA.TABLES WHERE  TABLE_TYPE = 'BASE TABLE' Exec Sp_executesql @sql", conn);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod()]
        public void UnitOfWorkInitializerTest()
        {
            // arrange
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString;
            var uof = new UnitOfWork(connectionString);

            // act
            int numberUsers = uof.UserManager.Users.Count();

            List<string> NameOfRoles = uof.RoleManager.Roles.Select(role => role.Name).ToList();
            string[] waitingNameOfRoles = { "User", "Moderator", "Admin" };

            List<string> NameOfGenres = uof.Genres.GetAll().Select(genre => genre.Name).ToList();
            string[] waitingNameOfGenrers = { "Pop", "Rock", "Hip-hop", "Classical" };

            // assert
            Assert.AreEqual(numberUsers, 1, "Not correct number of users");
            CollectionAssert.AreEquivalent(NameOfRoles, waitingNameOfRoles, "Not correct roles initialized");
            CollectionAssert.AreEquivalent(NameOfGenres, waitingNameOfGenrers, "Not correct genres initialized");
        }

        [TestMethod]
        public void UnitOfWorkAddingUser()
        {
            // arrange
            //var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString;
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\sasha\Documents\Visual Studio 2015\Projects\Epam\CatalogProject\CatalogOfMaterials\UnitTests\TestDBs\testDB.mdf; Integrated Security = True; Connect Timeout = 30";
            var uof = new UnitOfWork(connectionString);

            // act
            var newUser = new Entities.User() { UserName = "User1", Email = "someemail@mails.com" };
            uof.UserManager.Create(newUser, "111111");
            uof.UserManager.AddToRoles(newUser.Id, "User");
            uof.Complete();

            bool IsAdded = uof.UserManager.FindByName("User1") != null;

            //assert
            Assert.IsTrue(IsAdded, "User not added");
        }
    }
}