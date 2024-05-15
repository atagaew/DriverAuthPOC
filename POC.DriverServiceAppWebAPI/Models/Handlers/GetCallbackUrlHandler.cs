using MediatR;

namespace POC.DriverServiceAppWebAPI.Models.Handlers
{
    public class GetCallbackQuery : IRequest<string>
    {
        public string ClientId { get; set; }
        public string Type { get; set; }
    }

    public class GetCallbackUrlHandler : IRequestHandler<GetCallbackQuery, string>
    {
        private readonly ConfigurationSettings _appSettings;

        public GetCallbackUrlHandler(ConfigurationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Task<string> Handle(GetCallbackQuery request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case "lp":
                    return Task.FromResult(_appSettings.CallbackUrlLp.Replace("{replaceClientId}", request.ClientId));
                case "ws":
                    return Task.FromResult(_appSettings.CallbackUrlWs.Replace("{replaceClientId}", request.ClientId));
                default:
                    return Task.FromResult(string.Empty);
            }
        }
    }
}
