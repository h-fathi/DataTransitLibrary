using DataTransit.Infrastructure;
using DataTransit.Infrastructure.Caching;
using DataTransit.Infrastructure.Excel;
using Hf.Core.EfCore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Linq;
using Hf.Core.EfCore.GenericRepoitory;
using System.Collections.Generic;
using ExcelDataReader;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DataTransit.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICache distributedCache;
        private readonly IExcelProvider excelProvider;
        private readonly IRepository customerRepository;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(IExcelProvider _excelProvider,
            ICache _distributedCache,
            IRepository _customerRepository,
            ILogger<CustomerService> _logger)
        {
            excelProvider = _excelProvider;
            distributedCache = _distributedCache;
            customerRepository = _customerRepository;
            logger = _logger;
        }


        public virtual async Task UploadCustomers(IFormFile formFile)
        {
            var dataArray = await CommonHelper.GetDownloadBitsAsync(formFile);

            //get customer from excel
            var customers = await excelProvider.Read(dataArray);
            if(customers.Count > 0)
            {
                //get distinct customers
                var distinctCustomers = customers.Distinct().Select(s => new Customer
                {
                    Data = s.Data,
                });

                // save into redis
                await distributedCache.SetAsync("customer.all", distinctCustomers);


                // load from redis
                var redisCustomers = await distributedCache.GetAsync<IEnumerable<Customer>>("customer.all");

                // insert into db
                await customerRepository.AddBulkDataAsync<Customer>(redisCustomers);
            }

        }


        // that's for test and u can delete after test
        public virtual async Task UploadCustomersTest(IExcelDataReader excelDataReader)
        {

            #region load Data
            var stopwatch = Stopwatch.StartNew();


            //get customer from excel reader
            var customers = await excelProvider.Read(excelDataReader);


            stopwatch.Stop();
            logger.LogInformation("Load excel file in: {0} ms" , stopwatch.ElapsedMilliseconds);
            #endregion

            if (customers.Count > 0)
            {
                //get distinct customers
                var distinctCustomers = customers.Distinct().Select(s => new Customer
                {
                    Data = s.Data,
                });


                #region save into redis
                stopwatch = Stopwatch.StartNew();


                // save into redis
                await distributedCache.SetAsync("customer.all", distinctCustomers);


                stopwatch.Stop();
                logger.LogInformation("save into redis in: {0} ms", stopwatch.ElapsedMilliseconds);
                #endregion

                #region load from redis
                stopwatch = Stopwatch.StartNew();

                // load from redis
                var redisCustomers = await distributedCache.GetAsync<IEnumerable<Customer>>("customer.all");


                stopwatch.Stop();
                logger.LogInformation("load from redis in: {0} ms", stopwatch.ElapsedMilliseconds);
                #endregion

                #region insert into db
                stopwatch = Stopwatch.StartNew();
                // insert into db
                await customerRepository.AddBulkDataAsync<Customer>(redisCustomers);


                stopwatch.Stop();
                logger.LogInformation("insert into db in: {0} ms", stopwatch.ElapsedMilliseconds);
                #endregion

            }

        }
    }
}
