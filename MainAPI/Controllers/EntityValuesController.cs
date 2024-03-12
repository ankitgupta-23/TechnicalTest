using AutoMapper;
using DataLayer.Entities;
using DataLayer.Interfaces;
using MainAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MainAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class EntityValuesController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public EntityValuesController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper  = mapper;
        }

        
        [Route("/GetAllData")]
        [HttpGet]
        public async Task<IActionResult> GetAllData()

        {   var data = await _repository.GetEntities();
            try
            {
                return Ok(_mapper.Map<List<EntityDTO>>(data));
            }
            catch
            {
                return StatusCode(500, "Something went wrong!");
            }
            
        }

       
        [Route("/GetFilteredData")]
        [HttpGet]
        public async Task<IActionResult> GetFilteredData([FromQuery] string? searchQuery, string? gender, DateTime? startDate, DateTime? endDate,[FromQuery] List<string>? countries, string? orderBy, int pageNo, int pageSize)
        {
            try
            {
                 var data = await _repository.GetFilteredEntities(searchQuery,gender, startDate, endDate, countries, orderBy, pageNo, pageSize);
                return Ok(_mapper.Map<List<EntityDTO>>(data));
            }
            catch
            {
                return StatusCode(500, "Something went wrong!");
            }
           
        }

        [Route("/AddData")]
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] EntityDTO entity)
        {
            try
            {
                var res = await _repository.CreateEntity(_mapper.Map<Entity>(entity));
                return Ok("Entity Added Successfully");
            }
            catch
            {
                return StatusCode(500, "Something Went wrong!");
            }
           
        }


        [Route("/UpdateData")]
        [HttpPost]
        public async Task<IActionResult> UpdateEntity([FromBody] EntityDTO entity)
        {
            try
            {
                var res = await _repository.UpdateEntity(_mapper.Map<Entity>(entity));
        

                if(res)
                    return Ok("Entity Updated Successfully");
                
                return StatusCode(400, "Failed to update the data");

            }
            catch
            {
                return StatusCode(500, "Something went wrong!");
            }
            
        }

        [Route("/getbyId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var res = await _repository.GetEntityById(id);

                if(res!=null)
                    return Ok(_mapper.Map<EntityDTO>(res));
                
                return StatusCode(404, $"Entity With Id = {id} not found!");
            }
            catch
            {
                return StatusCode(500, "Couldn't get the data! try again");
            }
            
        }


        [Route("/delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity_toDelete = await _repository.GetEntityById(id);

            if(entity_toDelete == null)
                return StatusCode(404, $"Entity with id = {id} is not found");
            
            var res = await _repository.DeleteEntity(entity_toDelete);

            if(res)
                return Ok($"Entity with id = {id} Deleted successfully");
            

            return StatusCode(500, "Something went wrong");

        }
    }
}
