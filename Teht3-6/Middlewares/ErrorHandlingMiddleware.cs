using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace gamewebapi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
 
        public ErrorHandlingMiddleware(RequestDelegate next){
 
            _next = next;
        }
	
        public async Task InvokeAsync(HttpContext context)
        {
	        try
            {
                await _next(context);
	        }
	        catch(NotFoundException)
            {
                context.Response.StatusCode = 404;
	        }
            catch(RequirementException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Player isn't high enough leveled to obtain a sword");
            }
            catch(UserIsBannedException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("User is banned");
            }
	    }
    }
}