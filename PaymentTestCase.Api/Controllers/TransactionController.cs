﻿using Microsoft.AspNetCore.Mvc;

namespace PaymentTestCase.Api.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
