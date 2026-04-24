using System.ComponentModel.DataAnnotations;

namespace Domain.Models.StaticModel
{
    public class s_DeliveryTypesModel
    {
        [Key]
        public int DeliveryId { get; set; }
        public string DeliveryName { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
