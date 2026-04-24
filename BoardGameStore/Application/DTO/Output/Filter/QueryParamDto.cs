using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.FilterDTO
{
    public class QueryParamDto
    {
        public int ParamID { get; set; }
        public string QueryParam { get; set; }
        public string OperationName { get; set; }
    }
}
