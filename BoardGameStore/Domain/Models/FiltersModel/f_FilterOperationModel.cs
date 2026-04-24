using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.StaticModel;

namespace Domain.Models.FiltersModel
{
    public class f_FilterOperationModel
    {
        public int ID { get; set; }

        [ForeignKey(nameof(FilterInfo))]
        public int FilterID { get; set; }
        public f_FilterInfoModel FilterInfo { get; set; }

        [ForeignKey(nameof(OperationInfo))]
        public int OperationID { get; set; }
        public s_OperationModel OperationInfo { get; set; }
        public string OperationName { get; set; }

        public string QueryParam { get; set; }

    }
}
