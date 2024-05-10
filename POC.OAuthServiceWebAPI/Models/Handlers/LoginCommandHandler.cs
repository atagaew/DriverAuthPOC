using MediatR;
using POC.Common;
using POC.OAuthServiceWebAPI.Services;

namespace POC.OAuthServiceWebAPI.Models.Handlers
{
    public class LoginCommand : IRequest<UserInfo?>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CallbackUrl { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserInfo?>
    {
        private readonly UserRepository _userRepository;

        public LoginCommandHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserInfo?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Find(request.UserName, request.Password);
            return Task.FromResult(user);
        }
    }
}
