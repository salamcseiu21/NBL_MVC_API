using System.Collections.Generic;
using System.Web.Http;
using NBL.DAL;
using NBL.Models;
using NBL.Models.Employees;

namespace NBL.Controllers
{

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
