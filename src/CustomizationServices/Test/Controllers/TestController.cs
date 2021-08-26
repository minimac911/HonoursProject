using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Test.Infastrucutre.Helper;

namespace Test.Controllers
{
    [Route("customization")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Index()
        {
            var viewHtml = await RenderViewHelper.RenderViewAsync(this, "Index", new object { });
            return viewHtml;
        }

        [HttpPost]
        public bool TestPostForm()
        {
            return true;
        }
    }

    public class TestViewData
    {
        public string temp { get; set; }
    }
}


