using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Input
{
    public class FilterValueDto
    {
        public string FieldName { get; set; } = string.Empty;
        public  int FilterValue { get; set; } 
        public string? QueryParam { get; set; } = string.Empty;
    }
}
