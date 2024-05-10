using MediatR;

namespace POC.DriverServiceAppWebAPI.Models.Handlers
{
    public class GetCallbackQuery : IRequest<string>
    {
        public string ClientId { get; set; }
    }

    public class GetCallbackUrlHandler : IRequestHandler<GetCallbackQuery, string>
    {
        private readonly AppSettings _appSettings;

        public GetCallbackUrlHandler(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Task<string> Handle(GetCallbackQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_appSettings.CallbackUrl.Replace("{replaceClientId}", request.ClientId));
        }
    }
}
