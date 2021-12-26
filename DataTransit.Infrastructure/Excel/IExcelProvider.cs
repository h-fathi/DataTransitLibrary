using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataTransit.Infrastructure.Excel
{
    public interface IExcelProvider
    {
        Task<List<CustomerDTO>> Read(byte[] myByteArray);
        Task<List<CustomerDTO>> Read(IExcelDataReader excelDataReader);
    }
}
