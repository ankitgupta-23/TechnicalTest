

using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string EntityId { get; set; }
        public Entity Entity { get; set; }
    }
};


