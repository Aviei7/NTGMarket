using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.FilterDTO
{
    public class FilterListDto
    {
        public int ID { get; set; }
        public string FilterName { get; set; }
        public string FieldName { get; set; }
        public string FilterType { get; set; }
        public ICollection<QueryParamDto> ParamList { get; set; }
    }
}
