using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Service;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [AuthorizeAttribute(Policy = Permissions.Dashboards.View)]
        public IActionResult DashbarodView()
        {
            return View();
        }


        [Authorize(Policy = Permissions.Dashboards.Create)]
        public IActionResult Create()
        {
            return View();
        }
    }
}
