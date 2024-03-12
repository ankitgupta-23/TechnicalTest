using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
        public class Entity
        {
                [Key]   
                public string Id { get; set; }
                public List<Address>? Addresses { get; set; }
                public List<Date> Dates { get; set; }
                public bool Deceased { get; set; }
                public string? Gender { get; set; }
                public List<Name> Names { get; set; }
        }
}   
