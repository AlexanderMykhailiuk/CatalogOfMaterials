using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.DataTransferObjects;
using WEBLayer.Models;
using PagedList;
using static WEBLayer.Mapping.MappingConfigs;

namespace WEBLayer.Controllers
{
    public class HomeController : Controller
    {
        readonly IContentService contentService;
        
        public HomeController(IContentService contentService)
        {
            this.contentService = contentService;            
        }

        

        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNumber = (page ?? 1);

            var mapper = ContentDTOtoShortContentModelConfig.CreateMapper();
            
            return View(mapper.Map<IEnumerable<ShortContentModel>>(contentService.GetAllContents()).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}