using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RRRRCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RRRRCoreMVC.Controllers
{

    [Authorize]
    public class HomeController : Controller
        {




            Uri apiurl = new Uri("http://localhost:10420");
            HttpClient Client;
            public HomeController()
            {
                Client = new HttpClient();
                Client.BaseAddress = apiurl;
            }
            public IActionResult Index()
            {
                List<Employee> list = new List<Employee>();
                HttpResponseMessage list_Detail = Client.GetAsync(Client.BaseAddress + "api/home/Get/Table").Result;
                if (list_Detail.IsSuccessStatusCode)
                {
                    string data = list_Detail.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<List<Employee>>(data);

                    foreach (var item in res)
                    {
                        list.Add(new Employee
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Mobile = item.Mobile,
                            Dept = item.Dept,
                            Address = item.Address,

                        });


                    }


                }
                return View(list);
            }
            [HttpGet]
            public ActionResult AddEmp()
            {
                return View();
            }
            [HttpPost]
            public ActionResult AddEmp(Employee mvc)
            {

                string data = JsonConvert.SerializeObject(mvc);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "api/home/Post/Table", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");


            }

            public ActionResult Edit(int Id)
            {

                var res1 = Client.GetAsync(Client.BaseAddress + "api/home/Edit/Table" + '?' + "Id" + "=" + Id.ToString()).Result;
                string data = res1.Content.ReadAsStringAsync().Result;
                var v = JsonConvert.DeserializeObject<Employee>(data);
              TempData["D"]=v;
                return View("AddEmp", v);



            }
            public ActionResult Delete(int Id)
            {
                var res2 = Client.DeleteAsync(Client.BaseAddress + "api/home/Delete/Table" + '?' + "Id" + "=" + Id.ToString()).Result;

                return RedirectToAction("Index");
            }
            [HttpGet]
          [AllowAnonymous]
        public ActionResult Login()
            {
                return View();
            }
            [HttpPost]
            [AllowAnonymous]
        public ActionResult Login(UserInfo obj)
            {

                string data = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "api/home/Login/Table", content).Result;

                string data1 = res.Content.ReadAsStringAsync().Result;

                var v = JsonConvert.DeserializeObject<UserInfo>(data1);

                if (v.UserName == "Email is Not Found")
                {

                    TempData["Email"] = "Email not found";

                }

                else
                {

                    if (v.UserName == obj.UserName && v.Password == obj.Password)
                    {
                    var claims = new[] {new Claim(ClaimTypes.Name,v.UserName),
                    new Claim(ClaimTypes.Email,obj.Password)};

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var AuthProperties = new AuthenticationProperties
                    {
                        IsPersistent = false
                   };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), AuthProperties);

                    //Apply Session.....

                    HttpContext.Session.SetString("Name", v.UserName);
                    HttpContext.Session.SetString("Password", v.Password);

                    return RedirectToAction("Index");

                  }
                    else
                     { 
                        TempData["wrong"] = "PassWord inccorect ";
                         return View();
                      }


                 }

                return View();

            }
        public IActionResult LogOut()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Name");
            return RedirectToAction("Login");
        }
    }
    
}
