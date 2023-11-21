using Microsoft.AspNetCore.Mvc;
using testWorkISERV.Methods;
using testWorkISERV.Models;
using testWorkISERV.Services.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWorkISERV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<DTOData[]>> Get(string? country = null, string? search = null)
        {
            try
            {
                Guid? countryID = null;
                if (country != null)
                    countryID = GetCountry.Get(country);

                return search == null ? _dataService.GetData(countryID) : _dataService.GetData(search, countryID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
