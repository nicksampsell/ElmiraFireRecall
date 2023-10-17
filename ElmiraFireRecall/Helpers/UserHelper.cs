using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using Microsoft.EntityFrameworkCore;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;

namespace ElmiraFireRecall.Helpers
{
    [SupportedOSPlatform("windows")]
    public class UserHelper
    {
        public static string GetDisplayName(string? username = null)
        {
            string[] id;
            if (username == null)
            {
                id = WindowsIdentity.GetCurrent().Name.Split('\\');
            }
            else
            {
                id = username.Split('\\');
            }
            var dc = new PrincipalContext(ContextType.Domain, id[0]);
            var adUser = UserPrincipal.FindByIdentity(dc, id[1]);
            return $"{adUser.GivenName} {adUser.Surname}";

        }

        public static List<string> GetAllUsers()
        {
            string[] id = WindowsIdentity.GetCurrent().Name.Split('\\');
            var dc = new PrincipalContext(ContextType.Domain, id[0]);
            var group = GroupPrincipal.FindByIdentity(dc, "Domain Users");

            List<string> users = new List<string>();

            if (group != null)
            {

                var adUsers = group.GetMembers(true);
                foreach (var user in adUsers)
                {
                    users.Add(user.Name);
                }

            }
            return users;
        }

        public static Task<List<string>> GetAllUsersAsync()
        {
            return Task.Run(() => GetAllUsers());
        }
    }

}
