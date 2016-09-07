
namespace MPWebAPI.Fixtures
{
    public interface IFixtureBuilder
    {
        void AddFixture(string fixtureFile, bool flushDb = false);
    }    
}

