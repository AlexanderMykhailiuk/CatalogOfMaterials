using Microsoft.VisualStudio.TestTools.UnitTesting;
using WEBLayer.Models;
using BusinessLogicLayer.DataTransferObjects;


namespace WEBLayer.Tests
{
    [TestClass]
    public class MappingConfigsTests
    {
        [TestMethod]
        public void GenreModelToGenreDTOTest()
        {
            WEBLayer.Mapping.MappingConfigs.GenreModelToGenreDTO.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void RegisterModeltoUserDTOConfigTest()
        {
            // arrange
            var mapper = WEBLayer.Mapping.MappingConfigs.RegisterModeltoUserDTOConfig.CreateMapper();
            RegisterModel originalUser = new RegisterModel()
            {
                Username = "User1",
                Email = "test@mail.com",
                Password = "111111"
            };

            // act
            UserDTO mappingUser = mapper.Map<UserDTO>(originalUser);

            // assert
            Assert.AreEqual(originalUser.Username, mappingUser.UserName, "Usernames not equal after mapping");
            Assert.AreEqual(originalUser.Email, mappingUser.Email, "Emals not equal after mapping");
            Assert.AreEqual(originalUser.Password, mappingUser.Password, "Passwords not equal after mapping");
        }
    }
}
