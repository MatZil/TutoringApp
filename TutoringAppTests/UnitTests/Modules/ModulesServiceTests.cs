using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Modules;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Modules
{
    public class ModulesServiceTests
    {
        private readonly IModulesService _modulesService;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ApplicationDbContext _context;

        public ModulesServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;

            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _modulesService = new ModulesService(
                new ModulesRepository(setup.Context),
                UnitTestSetup.Mapper,
                _currentUserServiceMock.Object
                );
        }

        [Fact]
        public async Task When_GettingAllModules_Expect_CorrectModules()
        {
            var actualModules = await _modulesService.GetAll();

            Assert.Collection(actualModules,
                m => Assert.Equal("Operating Systems", m.Name),
                m => Assert.Equal("Databases", m.Name),
                m => Assert.Equal("Analysis of Algorithms", m.Name),
                m => Assert.Equal("Cyber Security", m.Name)
                );
        }

        [Theory]
        [InlineData("Software Testing")]
        public async Task When_CreatingModule_Expect_ModuleCreated(string name)
        {
            var moduleNew = new ModuleNewDto { Name = name };
            var id = await _modulesService.Create(moduleNew);

            var actualModuleCreated = await _context.Modules.FirstAsync(m => m.Id == id);
            Assert.Equal(name, actualModuleCreated.Name);
        }

        [Theory]
        [InlineData(2)]
        public async Task When_DeletingModule_Expect_ModuleDeleted(int id)
        {
            await _modulesService.Delete(id);

            var actualModuleDeleted = await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);
            Assert.Null(actualModuleDeleted);
        }
    }
}
