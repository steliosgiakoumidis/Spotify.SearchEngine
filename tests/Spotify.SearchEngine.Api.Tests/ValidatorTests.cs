using Spotify.SearchEngine.Api.Utilities;
using Xunit;

namespace Spotify.SearchEngine.Api.Tests
{
    public class ValidatorTests
    {
        private readonly Validator _validator;

        public ValidatorTests()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("Johny Cash", true)]
        [InlineData("Rainbow", true)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("    ", false)]
        [InlineData(null, false)]
        public void Test1(string artistName, bool expectedResult)
        {
            var sut = _validator.ValidateArtistsName(artistName);

            Assert.Equal(expectedResult, sut);
        }
    }
}
