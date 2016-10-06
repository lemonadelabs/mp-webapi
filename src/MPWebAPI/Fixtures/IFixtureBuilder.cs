
using System.Threading.Tasks;

namespace MPWebAPI.Fixtures
{
    public interface IFixtureBuilder
    {
        Task AddFixture(string fixtureFile, bool flushDb = false);
    }    
}

