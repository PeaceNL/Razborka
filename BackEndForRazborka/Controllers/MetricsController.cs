//using Microsoft.AspNetCore.Mvc;
//using Prometheus;

//namespace BackEndForRazborka.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class MetricsController: ControllerBase
//    {

//        [HttpGet]
//        public IActionResult GetDefaultMetrics()
//        {
//            var responseStream = HttpContext.Response.Body;
//            HttpContext.Response.ContentType = "text/plain; version=0.0.4";

//            var metricTasks = Metrics.DefaultRegistry.CollectAndExportAsTextAsync(responseStream);
//            return Ok(metricTasks);
//        }
//    }
//}
