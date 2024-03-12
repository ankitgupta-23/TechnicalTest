using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Name
    {

        [Key]
        public int Id {  get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Surname { get; set; }


        public string EntityId { get; set; }
        public Entity Entity { get; set; }
    }

}


