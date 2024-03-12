

namespace MainAPI.DTOs
{
    public class EntityDTO
    {
        public string Id { get; set; }
        public List<AddressDTO>? Addresses { get; set; }
        public List<DatesDTO> Dates { get; set; }
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
        public List<NameDTO> Names { get; set; }
    }
}