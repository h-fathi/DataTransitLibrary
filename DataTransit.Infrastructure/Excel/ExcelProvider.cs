using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataTransit.Infrastructure.Excel
{
    public class ExcelProvider : IExcelProvider
    {
        public async Task<List<CustomerDTO>> Read(byte[] myByteArray)
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var data = new List<CustomerDTO>();

            using (var stream = new MemoryStream(myByteArray))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read()) //Each ROW
                        {
                            data.Add(new CustomerDTO
                            {
                                Data = reader.GetValue(0).ToString(),
                            });
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }
            return await Task.FromResult(data);
        }

        public async Task<List<CustomerDTO>> Read(IExcelDataReader excelDataReader)
        {
            var data = new List<CustomerDTO>();
            long count = 1;
            try
            {
                do
                {
                    while (excelDataReader.Read()) //Each ROW
                    {
                        data.Add(new CustomerDTO
                        {
                            Data = excelDataReader.GetValue(0).ToString(),
                        });
                        count++;
                    }
                } while (excelDataReader.NextResult()); //Move to NEXT SHEET
            }
            catch(Exception ex)
            {
               var i = count;

            }
            return await Task.FromResult(data);
        }

    }
}
