using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using XevosASPWebAppDemo.Models;

namespace XevosASPWebAppDemo.Controllers
{
    public class HomeController : Controller
    {        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        } 
        
        public JsonResult GetEmployees()
        {       
            using (PeopleEntities db = new PeopleEntities())
            {
                List<Employee> employees = db.Employee.ToList<Employee>();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }               
        }
        
        public JsonResult GetCurrentEmployeeData()
        {
            string json;
            string url = "https://xevos.store/domaci-ukol/Jmena.json";
            using (HttpClient client = new HttpClient())
            {
                json = client.GetStringAsync(url).Result;
            }
            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            using (PeopleEntities db = new PeopleEntities())
            {

                db.Employee.AddOrUpdate(employees.ToArray());
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (PeopleEntities entities = new PeopleEntities())
            {
                var person = entities.Employee.Where(x => x.Id == id).FirstOrDefault();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using(PeopleEntities entities = new PeopleEntities())
                {
                    if(emp.Id > 0)
                    {
                        var person = entities.Employee.Where(x => x.Id == emp.Id).FirstOrDefault();
                        if(person != null)
                        {
                            person.Jmeno = emp.Jmeno;
                            person.Prijmeni = emp.Prijmeni;
                            person.Date = emp.Date;
                        }
                        else
                        {
                            entities.Employee.Add(emp);
                        }
                        entities.SaveChanges();
                        status = true;
                    }
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using(PeopleEntities entities = new PeopleEntities())
            {
                var person = entities.Employee.Where(x => x.Id == id).FirstOrDefault();
                if(person != null)
                {
                    return View(person);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (PeopleEntities entities = new PeopleEntities())
            {
                var person = entities.Employee.Where(x => x.Id == id).FirstOrDefault();
                if (person != null)
                {
                    entities.Employee.Remove(person);
                    entities.SaveChanges();
                    status = true;
                }               
            }
            return new JsonResult { Data = new { status = status } };
        }        
    }
}