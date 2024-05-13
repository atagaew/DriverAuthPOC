using MediatR;
using POC.DriverServiceAppWebAPI.Services;

namespace POC.DriverServiceAppWebAPI.Models.Handlers
{
    public class PublishTokenQuery : IRequest
    {
        public string ClientId { get; set; }

        public string Token { get; set; }
    }

    public class PublishTokenHandler : IRequestHandler<PublishTokenQuery>
    {
        private readonly TokenRepository _tokenRepository;

        public PublishTokenHandler(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task Handle(PublishTokenQuery request, CancellationToken cancellationToken)
        {
            _tokenRepository.AddToken(request.ClientId, request.Token);
        }
    }

}
