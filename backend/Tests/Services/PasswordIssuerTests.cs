using Application.Services;
using Xunit;

namespace Tests.Services
{
    public class PasswordIssuerTests
    {
        private readonly PasswordIssuer _passwordIssuer;

        public PasswordIssuerTests()
        {
            _passwordIssuer = new PasswordIssuer();
        }

        [Fact]
        public void Verify_WhenPasswordsMatch_ReturnsTrue()
        {
            // Arrange
            var originalPassword = "kimbokastekniv";
            var hashed = _passwordIssuer.Hash(originalPassword);

            // Act
            var result = _passwordIssuer.Verify(originalPassword, hashed);

            // Assesrt
            Assert.True(result);
        }
        
        [Fact]
        public void Verify_WhenPasswordsDontMatch_ReturnsFalse()
        {
            // Arrange
            var originalPassword = "callacab";
            var hash = _passwordIssuer.Hash("konamicode");

            // Act
            var result = _passwordIssuer.Verify(originalPassword, hash);

            // Assesrt
            Assert.False(result);
        }
    }
}