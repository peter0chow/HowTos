using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kunskapsbanken.Api.Models
{
    public class HowTo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string Department { get; set; }



    }
}