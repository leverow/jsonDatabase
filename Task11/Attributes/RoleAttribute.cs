using Microsoft.AspNetCore.Mvc;

namespace Task11.Attributes;

public class RoleAttribute : TypeFilterAttribute
{
    public RoleAttribute(string role) : base(typeof(AuthAttribute))
    {
        Arguments = new object[] { role };
    }
}