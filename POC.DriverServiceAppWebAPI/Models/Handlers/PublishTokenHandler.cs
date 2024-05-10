using MediatR;
using Microsoft.AspNetCore.SignalR;
using POC.DriverServiceAppWebAPI.Hubs;
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
        private readonly IHubContext<DriverServiceHub> _hubContext;
        private readonly AppSettings _appSettings;

        public PublishTokenHandler(TokenRepository tokenRepository, IHubContext<DriverServiceHub> hubContext, AppSettings appSettings)
        {
            _tokenRepository = tokenRepository;
            _hubContext = hubContext;
            _appSettings = appSettings;
        }

        public async Task Handle(PublishTokenQuery request, CancellationToken cancellationToken)
        {
            _tokenRepository.AddToken(request.ClientId, request.Token);

            if (_appSettings.UseSignalR)
            {
                await _hubContext.Clients.All.SendAsync("Message", "Received Token. Sending to appropriate client");
                await _hubContext.Clients.Group(request.ClientId).SendAsync("Token", request.Token);
            }
        }
    }

}
