using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hf.Core.EfCore.Models
{
    public class Customer
    {

        public Customer()
        {

        }

        [Key]
        public long Id { get; set; }
        public string Data { get; set; }

    }
}
