namespace ViprUnitTests
{
    // Its.Configuration can cache settings based on Type
    // To avoid test issues, creating a second type to
    // bust the cache
    public class TestSettings2 : TestSettings
    {
    }
}