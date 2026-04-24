using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.StaticModel
{
    public class s_OperationModel
    {
        [Key]
        public int OperationID { get; set; }
        public string OperationName { get; set; }
        public string ExpressionTemplate { get; set; }
        public string OperationDescription { get; set; }
    }
}
