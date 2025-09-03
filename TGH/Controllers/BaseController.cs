using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Controllers
{
    public class BaseController : Controller
    {
        protected bool Arabic { get; set; }
        public BaseController()
        {
            Arabic = CultureInfo.CurrentUICulture.Name == "ar";
        }
    }
}
