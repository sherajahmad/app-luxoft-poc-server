using AppLuxoft.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppLuxoft.Controllers
{
   
    public class EmployeeController : ApiController
    {
       
       
        [Route("api/Employee/GetPieChartData")]
        public PieChartModel GetPieChartData()
        {
                 PieChartModel model = new PieChartModel();
            using (var context = new LuxoftDBEntities1())
            {
                model.TotalCount =context.employees.Count();
                var Employees = (from employee in context.employees
                                 where employee.deactivaed == false
                                   select employee 
                                   );
                model.ActiveCount = Employees.Count();
                model.InActiveCount= model.TotalCount- model.ActiveCount;
            }
                return model;
            
         }
        
         [Route("api/Employee/GetData/{page}/{sortCol}")]
        public EmployeeModel GetData(int page, string sortCol = "createdDate_desc")
        {
            using (var context = new LuxoftDBEntities1())
            {
                EmployeeModel model = new EmployeeModel();
                model.PageIndex = page;
                model.PageSize = 1;
                
                int startIndex = (page - 1) * model.PageSize;
                var Employees = (from employee in context.employees
                                 where employee.deactivaed == false
                                   select employee 
                                   );
                model.RecordCount = Employees.Count();
                switch(sortCol)
                {
                    case "name" :
                        Employees = Employees.OrderBy(employee => employee.name);
                    break;
                    case "address":
                    Employees = Employees.OrderBy(employee => employee.address);
                    break;
                    case "createdDate":
                    Employees = Employees.OrderBy(employee => employee.createdDate);
                    break;
                    case "name_desc":
                    Employees = Employees.OrderByDescending(employee => employee.name);
                    break;
                    case "address_desc":
                    Employees = Employees.OrderByDescending(employee => employee.address);
                    break;
                    case "createdDate_desc":
                    Employees = Employees.OrderByDescending(employee => employee.createdDate);
                    break;
                  

                }

                model.Employees = Employees
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                return model;
            }
        }


        
        [HttpPost()]
        [Route("api/Employee/AddEmployee")]
        public bool AddEmployee( employee employee)
        {

            using (var context = new LuxoftDBEntities1())
            {
                employee.createdDate = DateTime.Now;
                employee.deactivaed = false;
                employee.dob = DateTime.Now;
                if (employee.id != 0)
                {
                    context.Entry(employee).State = EntityState.Modified;
                }
                else
                {
                    context.employees.Add(employee);
                }
                
                context.SaveChanges();
            }
            // Add product
            return true;
        }
              
        [HttpPost()]
        [Route("api/Employee/EmpDelete")]
        public bool EmpDelete(employee employee)
        {
            using (var context = new LuxoftDBEntities1())
            {
                var employee1 = context.employees.Where(i => i.id == employee.id)
       .Single();

                employee1.deactivaed = true;
                context.Entry(employee1).State = EntityState.Modified;
                context.SaveChanges();
            }
            return true;
        }
    }
}