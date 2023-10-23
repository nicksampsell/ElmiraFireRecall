using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace ElmiraFireRecall
{
    
    public class CCClaimsTransformation : IClaimsTransformation
    {
        private FireDBContext _context;
        public CCClaimsTransformation(FireDBContext context)
        {
            _context = context;
        }

        Task<ClaimsPrincipal> IClaimsTransformation.TransformAsync(ClaimsPrincipal principal)
        {

            User? user = _context.Users.Where(u => u.UserId == principal.Identity.Name).AsNoTracking().FirstOrDefault();


            if (user != null)
            {
                var ci = (ClaimsIdentity)principal.Identity;

                string role;
                switch (user.UserRole)
                {
                    case UserRole.Administrator:
                        role = "Administrator";
                        break;
                    default:
                        role = "User";
                        break;
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("UserId", user.Id.ToString()));
                claims.Add(new Claim("FirstName", user.FirstName));
                claims.Add(new Claim("LastName", user.LastName));
                claims.Add(new Claim("FullName", $"{user.FirstName} {user.LastName}"));
                claims.Add(new Claim("Email", user.Email));
                claims.Add(new Claim("UserRole", user.UserRole.ToString()));
                ci.AddClaims(claims);

            }

            return Task.FromResult(principal);
        }
    }
}
