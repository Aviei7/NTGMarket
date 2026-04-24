using Domain.Models.FiltersModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.StaticModel
{
    public class s_TablesModel
    {
        [Key]
        public int TableID { get; set; }

        public string TableName { get; set; }
        public string Description { get; set; }

        public ICollection<f_FilterInfoModel> Filters { get; set; }
    }
}
