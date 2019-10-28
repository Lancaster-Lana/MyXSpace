﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
//using AspNet.Security.OpenIdConnect.Primitives;

namespace MyXSpace.Core.Permissions
{
    public static class Utilities
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst("Subject")?.Value?.Trim();
            //return user.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }

        public static string[] GetRoles(ClaimsPrincipal identity)
        {
            return identity.Claims
                .Where(c => c.Type == "Role")//OpenIdConnectConstants.Claims.Role)
                .Select(c => c.Value)
                .ToArray();
        }
    }
}
