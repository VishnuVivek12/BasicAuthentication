using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using APIBasicAuth.Models;
using APIBasicAuthentication.Models;

namespace APIBasicAuth.Filters
{
        /// <summary>
        /// Custom Basic Authentication filter for validating credentials and assigning roles.
        /// </summary>
        public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
        {
                /// <summary>
                /// A list of users with their credentials and roles.
                /// </summary>
                public List<User> Users { get; set; }

                /// <summary>
                /// Initializes the BasicAuthenticationAttribute and sets up hardcoded users.
                /// </summary>
                public BasicAuthenticationAttribute()
                {
                        this.Users = new List<User>()
                        {
                                new User(){Username="admin",Password="Admin@123",Roles=new string[]{ "Admin" } },

                                new User(){Username="hr1",Password="Hr1@123",Roles=new string[]{ "Hr" } },
                                new User(){Username="hr2",Password="Hr2@123",Roles=new string[]{ "Hr" } },

                                new User(){Username="user1",Password="User1@123",Roles=new string[]{ "User" } },
                                new User(){Username="user2",Password="User2@123",Roles=new string[]{ "User" } },
                                new User(){Username="user2",Password="User2@123",Roles=new string[]{ "User" } },
                        };
                }

                /// <summary>
                /// Called before a controller action is executed. Handles authentication logic.
                /// </summary>
                /// <param name="actionContext">The HTTP action context.</param>
                public override void OnAuthorization(HttpActionContext actionContext)
                {
                        AuthenticationHeaderValue authHeader = actionContext.Request.Headers.Authorization;

                        //Return Unauthorized if Authorization header is not present
                        if (authHeader == null)
                        {
                                actionContext.Response = actionContext.Request.CreateResponse(
                                        HttpStatusCode.Unauthorized,
                                        "Authorization header is missing.");
                                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic");
                                return;
                        }

                        //Return Unauthorized if Authorization scheme is not Basic
                        if (authHeader.Scheme != "Basic")
                        {
                                actionContext.Response = actionContext.Request.CreateResponse(
                                        HttpStatusCode.Unauthorized,
                                        "Basic Authorization scheme required.");
                                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic");
                                return;
                        }

                        //Decodes the credential
                        string encodedCredentials = authHeader.Parameter;
                        byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
                        string decodedString = Encoding.UTF8.GetString(decodedBytes);

                        string[] parts = decodedString.Split(':');
                        string username = parts[0];
                        string password = parts[1];

                        //Verify the identity of clients and its roles
                        if (VerifyCredentials(username, password, out string[] roles))
                        {
                                IPrincipal principal = GetPrincipal(Principals.Claims, username, roles);

                                //Thread.CurrentPrincipal = principal;    //Sets the user info for the current execution thread

                                //Sets the user info for classic ASP.NET context (not always needed, but safe to include)
                                if (HttpContext.Current != null)
                                        HttpContext.Current.User = principal;   // For ASP.NET context
                        }
                        else
                        {
                                //Return Unauthorized if credentials are not match
                                actionContext.Response = actionContext.Request.CreateResponse(
                                        HttpStatusCode.Unauthorized,
                                        "Invalid username or password.");
                                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic");
                        }
                }

                /// <summary>
                /// Verifies the provided username and password against the hardcoded users list.
                /// </summary>
                /// <param name="username">The username from the request.</param>
                /// <param name="password">The password from the request.</param>
                /// <param name="roles">Out parameter for the user's roles if authentication is successful.</param>
                /// <returns>True if credentials are valid; otherwise, false.</returns>
                public bool VerifyCredentials(string username, string password, out string[] roles)
                {
                        User user = this.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();

                        roles = user == null ? new string[] { } : user.Roles;

                        return user != null;
                }

                public IPrincipal GetPrincipal(Principals principals, string username, string[] roles)
                {
                        IIdentity identity;
                        IPrincipal principal;

                        if (principals == Principals.Claims)
                        {
                                List<Claim> claims = new List<Claim>()
                                {
                                        new Claim(ClaimTypes.Name,username)
                                };
                                foreach (string role in roles)
                                {
                                        claims.Add(new Claim(ClaimTypes.Role, role));
                                }

                                identity = new ClaimsIdentity(claims, "Basic");
                                principal = new ClaimsPrincipal(identity);
                        }
                        else
                        {
                                //If authentication is successful set Identity & Principal / Roles
                                identity = new GenericIdentity(username);   //Creates a basic identity object for the user
                                principal = new GenericPrincipal(identity, roles);  //Combines the identity with the user's roles
                        }

                        return principal;
                }
        }
}