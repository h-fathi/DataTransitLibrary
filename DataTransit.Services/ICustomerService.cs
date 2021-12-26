using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DataTransit.Services
{
    public interface ICustomerService
    {
        Task UploadCustomersTest(IExcelDataReader excelDataReader);
        Task UploadCustomers(IFormFile formFile);
    }
}
