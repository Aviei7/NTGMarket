using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.StaticModel;

namespace Domain.Models.FiltersModel
{
    public class f_FilterInfoModel
    {
        [Key]
        public int FilterID { get; set; }

        [ForeignKey(nameof(TableInfo))]
        public int TableID { get; set; }    
        public s_TablesModel TableInfo { get; set; }
        public string FilterName { get; set; }
        public string FieldName { get; set; }

        [ForeignKey(nameof(FilterInfo))]
        public int FilterTypeID { get; set; }
        public s_FilterTypeModel FilterInfo { get; set; }

        public ICollection<f_FilterOperationModel> Filter_QueryParam { get; set; } 
    }
}
