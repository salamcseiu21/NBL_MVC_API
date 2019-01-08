using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NBL.DAL;
using NBL.Models;

namespace NBL.Controllers
{
    public class EmployeesController : ApiController
    {

        private readonly EmployeeGateway _employeeGateway=new EmployeeGateway();
        // GET: api/Employees
        public IEnumerable<Employee> Get()
        {
            return _employeeGateway.GetAll();
        }

        // GET: api/Employees/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employees
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employees/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employees/5
        public void Delete(int id)
        {
        }
    }
}
