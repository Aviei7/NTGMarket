using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.StaticModel
{
    public class s_FilterTypeModel
    {
        [Key]
        public int FilterTypeID { get; set; }
        public string ComponentName { get; set; }
        public string ComponentHint { get; set; } /*Тип компонента*/
        public bool HasRange { get; set; } /*Есть ли диапазон*/
        public bool HasMultiple { get; set; } /*Множественный выбор*/
    }
}
