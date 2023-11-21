using testWorkISERV.Models;

namespace testWorkISERV.Services.Data
{
    public interface IDataService
    {
        public DTOData[] GetData(Guid? countryId = null);
        public DTOData[] GetData(string search, Guid? countryId = null);
    }
}
