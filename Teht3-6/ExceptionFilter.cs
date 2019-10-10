using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace gamewebapi
{
    public class ExceptionFilter : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context){
            if (context.Exception is RequirementException){
                   
            }
        }
    }
}