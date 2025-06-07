using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIBasicAuth.Models;
using APIBasicAuth.Filters;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace APIBasicAuth.Controllers
{
        /// <summary>
        /// Controller to manage Employee operations such as retrieval, creation, update, and deletion.
        /// Requires Basic Authentication and role-based access.
        /// </summary>
        [BasicAuthentication]
        public class EmployeeController : ApiController
        {
                /// <summary>
                /// In-memory list of employees.
                /// </summary>
                public List<Employee> Employees { get; set; }

                /// <summary>
                /// Initializes the employee list with sample data.
                /// </summary>
                public EmployeeController()
                {
                        this.Employees = new List<Employee>()
                        {
                                new Employee(){Id=1,Name="Shahid Kapoor",Department="IT",Salary=1200000 },
                                new Employee(){Id=2,Name="Alia Bhat",Department="Finance",Salary=800000 },
                                new Employee(){Id=3,Name="Ranveer Singh",Department="Marketing",Salary=700000 },
                                new Employee(){Id=4,Name="Sara Ali Khan",Department="HR",Salary=900000 },
                                new Employee(){Id=5,Name="Ranbir Kapoor",Department="R&D",Salary=1000000 },
                        };
                }

                /// <summary>
                /// Gets the current user details.
                /// Accessible to users with roles: Admin or HR or User.
                /// </summary>
                /// <returns>User details.</returns>
                /// 
                [Authorize(Roles = "Admin,Hr,User")]
                [HttpGet]
                public IHttpActionResult GetCurrentUser()
                {
                        IPrincipal user = Thread.CurrentPrincipal;
                        if (user == null)
                        {
                                return Unauthorized();
                        }

                        string username = user.Identity.Name;
                        bool isAdmin = user.IsInRole("Admin");

                        //or
                        //IPrincipal contextUser = HttpContext.Current?.User;
                        //if (contextUser == null)
                        //{
                        //        return Unauthorized();
                        //}
                        //string username = contextUser.Identity.Name;
                        //bool isAdmin = contextUser.IsInRole("Admin");

                        return Ok(new
                        {
                                Username = username,
                                IsAdmin = isAdmin,
                        });
                }

                /// <summary>
                /// Gets the full list of employees.
                /// Accessible to users with roles: Admin or HR.
                /// </summary>
                /// <returns>List of all employees.</returns>
                [Authorize(Roles = "Admin,Hr")]
                [HttpGet]
                public IHttpActionResult GetEmployees()
                {
                        return Ok(this.Employees);
                }

                /// <summary>
                /// Gets a specific employee by ID.
                /// Accessible to users with roles: Admin, HR, or User.
                /// </summary>
                /// <param name="id">ID of the employee to retrieve.</param>
                /// <returns>A placeholder string value.</returns>
                [Authorize(Roles = "Admin,Hr,User")]
                public IHttpActionResult GetEmployee(int id)
                {
                        Employee employee = this.Employees.Where(e => e.Id == id).FirstOrDefault();

                        if (employee == null)
                                return NotFound();

                        return Ok(employee);
                }

                /// <summary>
                /// Adds a new employee.
                /// Accessible only to users with the HR role.
                /// </summary>
                /// <param name="employee">The employee object to add.</param>
                /// <returns>HTTP 200 OK on success.</returns>
                [Authorize(Roles = "Hr")]
                public IHttpActionResult Post([FromBody] Employee employee)
                {
                        try
                        {
                                this.Employees.Add(employee);
                                return Created("Create Employee", employee);
                        }
                        catch (Exception ex)
                        {
                                return InternalServerError(ex);
                        }
                }

                /// <summary>
                /// Updates an existing employee.
                /// Accessible only to users with the HR role.
                /// </summary>
                /// <param name="id">ID of the employee to update.</param>
                /// <param name="employee">Updated employee object.</param>
                /// <returns>HTTP 200 OK on success.</returns>
                [Authorize(Roles = "Hr")]
                public IHttpActionResult Put(int id, [FromBody] Employee employee)
                {
                        return Ok();
                }

                /// <summary>
                /// Deletes an employee by ID.
                /// Accessible only to users with the HR role.
                /// </summary>
                /// <param name="Id">ID of the employee to delete.</param>
                /// <returns>HTTP 200 OK on success.</returns>
                [Authorize(Roles = "Hr")]
                public IHttpActionResult Delete(int Id)
                {
                        Employee employee = this.Employees.Where(e => e.Id == Id).FirstOrDefault();

                        if (employee == null)
                                return NotFound();

                        this.Employees.Remove(employee);
                        return Ok();
                }
        }
}
