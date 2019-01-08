using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using NBL.DAL;
using NBL.Models;

namespace NBL.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EmployeesController : ApiController
    {

        private readonly EmployeeGateway _employeeGateway=new EmployeeGateway();
        readonly DepartmentGateway _departmentGateway=new DepartmentGateway();
        readonly  DesignationGateway _designationGateway=new DesignationGateway();
        // GET: api/Employees
        public IEnumerable<Employee> Get()
        {
            var employees = _employeeGateway.GetAll();
            foreach (Employee employee in employees)
            {
                employee.Department = _departmentGateway.GetById(employee.DepartmentId);
                employee.Designation = _designationGateway.GetById(employee.DesignationId);

            }
            return employees;
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
