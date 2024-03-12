using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Date
    {
        [Key]
        public int Id { get; set; }
        public string? DateType { get; set; }

        public DateTime? _Date { get; set; }

        public string EntityId { get; set; }

        public Entity Entity { get; set; }
    }
}
