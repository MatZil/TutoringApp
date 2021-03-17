using AutoMapper;
using TutoringApp.Configurations.Mapping;
using Xunit;

namespace TutoringAppTests.UnitTests.Shared
{
    public class AutoMapperTests
    {
        [Fact]
        public void When_CheckingModuleMapping_Expect_ConfigurationValid()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile(new ModuleMappingProfile());
            });

            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void When_CheckingAuthMapping_Expect_ConfigurationValid()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile(new AuthMappingProfile());
            });

            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void When_CheckingUserMapping_Expect_ConfigurationValid()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile(new UserMappingProfile());
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}
