using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Spotify.SearchEngine.Application.Handlers;
using Spotify.SearchEngine.Application.Interfaces;
using Xunit;

namespace Spotify.SearchEngine.Application.Tests
{
    public class ArtistsHandlerTests
    {
        private readonly Mock<IAuthenticationService> _authenticationService;
        private readonly Mock<ISearchService> _searchService;
        private readonly ArtistsHandler _handler;

        public ArtistsHandlerTests()
        {
            _authenticationService = new Mock<IAuthenticationService>();
            _searchService = new Mock<ISearchService>();
            _handler = new ArtistsHandler(_authenticationService.Object, _searchService.Object);
        }

        [Fact]
        public async Task GetAuthenticationTokenReturnsFalse()
        {
            _authenticationService.Setup(x => x.GetAuthenticationToken())
                .Returns(Task.FromResult(new Models.ActionResponse<string>(false)));

            var sut = await _handler.GetArtistsTracks("Rainbow");

            Assert.False(sut.IsSuccessful);
        }

        [Fact]
        public async Task GetTracksFromUrlsTokenReturnsFalse()
        {
            _authenticationService.Setup(x => x.GetAuthenticationToken())
                .Returns(Task.FromResult(new Models.ActionResponse<string>(true)
                {
                    ResponsePayload = "token"
                }));
            _searchService.Setup(x => x.GetTracksUrlForArtist(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Models.ActionResponse<List<Models.ArtistTracksUrlResponse>>(false)));

            var sut = await _handler.GetArtistsTracks("Rainbow");

            Assert.False(sut.IsSuccessful);
        }

        [Fact]
        public async Task GetArtistsTracksIsSuccessful()
        {
            _authenticationService.Setup(x => x.GetAuthenticationToken())
                .Returns(Task.FromResult(new Models.ActionResponse<string>(true)
                {
                    ResponsePayload = "token"
                }));
            _searchService.Setup(x => x.GetTracksUrlForArtist(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Models.ActionResponse<List<Models.ArtistTracksUrlResponse>>(true)
                {
                    ResponsePayload = new List<Models.ArtistTracksUrlResponse>(){
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "Rainbow",
                                TracksUrl = "Url"
                            }
                    }
                }));

            var sut = await _handler.GetArtistsTracks("Rainbow");

            Assert.True(sut.IsSuccessful);
        }

        [Fact]
        public async Task ArtistsReturnFromSearchAllMatchOnFilteringCriteria()
        {
            _authenticationService.Setup(x => x.GetAuthenticationToken())
                .Returns(Task.FromResult(new Models.ActionResponse<string>(true)
                {
                    ResponsePayload = "token"
                }));
            _searchService.Setup(x => x.GetTracksUrlForArtist(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Models.ActionResponse<List<Models.ArtistTracksUrlResponse>>(true)
                {
                    ResponsePayload = new List<Models.ArtistTracksUrlResponse>(){
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "Rainbow",
                                TracksUrl = "Url1"
                            },
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "Rai nbo w",
                                TracksUrl = "Url2"
                            },
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "rainbow",
                                TracksUrl = "Url3"
                            }

                    }
                }));

            var sut = await _handler.GetArtistsTracks("Rainbow");

            Assert.True(sut.IsSuccessful);
            Assert.Equal(3, sut.ResponsePayload.Count);
        }

        [Fact]
        public async Task ArtistsReturnFromSearchNoMatchOnFilteringCriteria()
        {
            _authenticationService.Setup(x => x.GetAuthenticationToken())
                .Returns(Task.FromResult(new Models.ActionResponse<string>(true)
                {
                    ResponsePayload = "token"
                }));
            _searchService.Setup(x => x.GetTracksUrlForArtist(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Models.ActionResponse<List<Models.ArtistTracksUrlResponse>>(true)
                {
                    ResponsePayload = new List<Models.ArtistTracksUrlResponse>(){
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "Rainbow",
                                TracksUrl = "Url1"
                            },
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "The Rainbow",
                                TracksUrl = "Url2"
                            },
                            new Models.ArtistTracksUrlResponse()
                            {
                                ArtistName = "rainb",
                                TracksUrl = "Url3"
                            }

                    }
                }));

            var sut = await _handler.GetArtistsTracks("Rainbow");

            Assert.True(sut.IsSuccessful);
            Assert.Single(sut.ResponsePayload);
            Assert.Equal("Url1", sut.ResponsePayload[0].TracksUrl);
        }
    }
}
