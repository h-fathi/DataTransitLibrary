using DataTransit.Services;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataTransit.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService _customerService)
        {
            _logger = logger;
            customerService = _customerService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> AsyncUploadTest()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open("sampleData.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // try to upload customers from excel reader
                    await customerService.UploadCustomersTest(reader);

                }
            }
            
            return new JsonResult(new
            {
                success = true,
                message = "File uploaded"
            });
        }


        [HttpPost]
        public virtual async Task<IActionResult> AsyncUpload()
        {

            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
                return new JsonResult( new { success = false, message = "No file uploaded" });

            // try to upload customers from posted file
            await customerService.UploadCustomers(httpPostedFile);


            return new JsonResult(new
            {
                success = true,
                message= "File uploaded"
            });
        }
    }
}
