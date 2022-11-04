using AutoFixture;

namespace Tests.TestUtils.Bootstrapping
{
    public static class CustomFixtureCreator
    {
        public static IFixture Create() => new Fixture().Customize(new FixtureCustomization());
    }
}