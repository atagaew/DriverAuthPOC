using MediatR;
using POC.DriverServiceAppWebAPI.Services;

namespace POC.DriverServiceAppWebAPI.Models.Handlers
{
    public class GetTokenQuery : IRequest<string>
    {
        public string ClientId { get; set; }
    }

    public class GetTokenHandler : IRequestHandler<GetTokenQuery, string>
    {
        private readonly TokenRepository _tokenRepository;

        public GetTokenHandler(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public Task<string> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var token = _tokenRepository.GetToken(request.ClientId);
            return Task.FromResult(token);
        }
    }
}
