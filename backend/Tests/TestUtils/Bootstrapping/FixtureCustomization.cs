using System.Net.Mail;
using Application.Domain;
using Application.Features;
using AutoFixture;
using MongoDB.Bson;

namespace Tests.TestUtils.Bootstrapping
{
    public class FixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(NextObjectId);

            fixture.Customize<User>(e => e.With(x => x.Email, fixture.Create<MailAddress>().Address));
            fixture.Customize<UserCreationCommand>(e => e.With(x => x.Email, fixture.Create<MailAddress>().Address));
        }

        private ObjectId NextObjectId()
        {
            return ObjectId.GenerateNewId();
        }
    }
}