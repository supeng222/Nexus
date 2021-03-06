﻿using Aiursoft.Status.Data;
using Aiursoft.Status.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Aiursoft.Status.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatusDbContext _dbContext;

        public HomeController(StatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.MonitorRules.ToListAsync();
            var model = new IndexViewModel
            {
                Data = data
            };
            return View(model);
        }
    }
}
