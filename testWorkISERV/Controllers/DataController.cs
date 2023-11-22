using Microsoft.AspNetCore.Mvc;
using testWorkISERV.Methods;
using testWorkISERV.Models;
using testWorkISERV.Services.Data;
using testWorkISERV.Services.ETL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWorkISERV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IEtlService _etlService;
        public DataController(IDataService dataService, IEtlService etlService)
        {
            _dataService = dataService;
            _etlService = etlService;
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
