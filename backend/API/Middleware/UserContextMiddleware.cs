using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services;
using Application.Services.Session;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace API.Middleware
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IUserFetcher _userFetcher;
        
        public UserContextMiddleware(
            RequestDelegate next, 
            IUserFetcher userFetcher)
        {
            _next = next;
            _userFetcher = userFetcher;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated && 
                ObjectId.TryParse(context.User.Identity.Name, out var userId))
            {
                var user = await _userFetcher.Fetch(userId);
                if (user == null)
                {
                    throw new UnauthorizedException("UserId in token does not match an existing user.");
                }

                context.Items.Add(ContextUserFetcher.HttpContextUserKey, user);
            }

            await _next.Invoke(context);
        }
    }
}