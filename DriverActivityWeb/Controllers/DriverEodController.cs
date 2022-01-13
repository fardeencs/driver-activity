using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DriverActivityWeb.Controllers
{
    public class DriverEodController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAppUserFileService _appUserFileService;
        private readonly IDriverCommonService driverCommonService;
        private readonly IDriverEODService driverEODService;

        public DriverEodController(ILogger<HomeController> logger,
            ApplicationDbContext dbContext,
            IAppUserFileService appUserFileService,
            IDriverCommonService driverCommonService,
            IDriverEODService driverEODService)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this._appUserFileService = appUserFileService;
            this.driverCommonService = driverCommonService;
            this.driverEODService = driverEODService;
        }

       

        public IActionResult Index()
        {
            return View();
        }

        

    }
}