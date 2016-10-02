using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer.Entities;
using BusinessLogicLayer.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Tests
{
    [TestClass]
    public class MappingConfigsTests
    {
        [TestMethod]
        public void GenretoGenreDTOConfigTest()
        {
            BusinessLogicLayer.Mapping.MappingConfigs.GenretoGenreDTOConfig.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void FiletoFileDTOConfigTest()
        {
            BusinessLogicLayer.Mapping.MappingConfigs.FiletoFileDTOConfig.AssertConfigurationIsValid();
        }

        public void UserDTOtoUserConfigTest()
        {
            // arrange
            var mapper = BusinessLogicLayer.Mapping.MappingConfigs.UserDTOtoUserConfig.CreateMapper();
            UserDTO[] originalUsers =
            {
                new UserDTO { Email="some1@mail.com", UserName="User1" },
                new UserDTO { Email="some2@mail.com", UserName="User2" },
                new UserDTO { Email="some3@mail.com", UserName="User3" },
                new UserDTO { Email="some4@mail.com", UserName="User4" },
                new UserDTO { Email="some5@mail.com", UserName="User5" },
            };

            // act
            IEnumerable<User> mappingUsers = mapper.Map<IEnumerable<User>>(originalUsers);

            int originalCount = originalUsers.Length;
            int mappingCount = mappingUsers.Count();

            var originalNames = originalUsers.Select(x => x.UserName).ToList();
            var mappingNames = mappingUsers.Select(x => x.UserName).ToList();

            var originalEmails = originalUsers.Select(x => x.Email).ToList();
            var mappingEmails = mappingUsers.Select(x => x.Email).ToList();

            // assert
            Assert.AreEqual(originalCount, mappingCount, "Counts not equal");
            CollectionAssert.AreEquivalent(originalEmails, mappingEmails, "Not matching ids");
            CollectionAssert.AreEquivalent(originalNames, mappingNames, "Not matching names");
        }
    }
}
