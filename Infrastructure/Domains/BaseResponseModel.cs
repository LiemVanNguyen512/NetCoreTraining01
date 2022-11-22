using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domains
{
    public class BaseResponseModel
    {
        public int Status { get; set; } //Status
        public string Message { get; set; }
        public string Source { get; set; }
        public string XRequestId { set; get; }
        public object XData { set; get; } //Orginal Data ouput from api
    }
}
