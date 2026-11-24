using Microsoft.AspNetCore.Mvc;

namespace Template.QrCode.Api.Filters
{
    public class AuthorizeAccessAttribute : TypeFilterAttribute
    {
        public AuthorizeAccessAttribute(): base(typeof(AuthorizeAccessFilter))
        {

        }
        public AuthorizeAccessAttribute(string actionName) : base(typeof(AuthorizeAccessFilter))
        {
            Arguments = new object[] { actionName };
        }
    }
}
