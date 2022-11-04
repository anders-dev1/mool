using Application.Domain;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Session
{
    public interface IContextUserFetcher
    {
        User FetchOrThrow();
        User? Fetch();
    }

    public class ContextUserFetcher : IContextUserFetcher
    {
        public const string HttpContextUserKey = "user";
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextUserFetcher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User FetchOrThrow()
        {
            var user = Fetch();
            if (user == null)
            {
                throw new UnauthorizedException("Requesting user could not be found");
            }

            return user;
        }

        public User? Fetch()
        {
            var user = _httpContextAccessor.HttpContext?.Items[ContextUserFetcher.HttpContextUserKey] as User;
            return user;
        }
    }
}