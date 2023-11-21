using Microsoft.AspNetCore.Mvc;
using testWorkISERV.Methods;
using testWorkISERV.Models;
using testWorkISERV.Services.Country;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWorkISERV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IEditCountryService _countryService;
        public CountryController(IEditCountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<DTOCountry[]>> Get()
        {
            var country = GetCountry.Get();
            if (country == null || country.Count == 0) 
                return NotFound();

            return (from a in country
                    select new DTOCountry
                    {
                        ID = a.Key,
                        Name = a.Value,
                    }).ToArray();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Попытка создать страну с пустым названием");

                _countryService.AddCountry(value);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                _countryService.RemoveCountry(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
