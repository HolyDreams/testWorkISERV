using testWorkISERV.Methods;
using testWorkISERV.Services.ETL;
using testWorkISERV.Services.SQL;

namespace testWorkISERV.Services.Country
{
    public class EditCountryService : IEditCountryService
    {
        private readonly SQLRequestService _sql;
        private readonly IEtlService _etlService;
        public EditCountryService(IEtlService etlService)
        {
            _sql = new SQLRequestService();
            _etlService = etlService;
        }

        public void AddCountry(string name)
        {
            var sqlQuery = $@"INSERT INTO iserv_country (name)
                              VALUES ('{name}')
                              ON CONFLICT (name)
                              DO UPDATE SET delete_state_code = 0
                              RETURNING country_id";
            var res = _sql.GetData(sqlQuery);

            GetCountry.AddCountry((Guid)res.Rows[0][0], name);
            _etlService.StartUpdate();
        }
        public void RemoveCountry(Guid id)
        {
            var sqlQuery = $@"UPDATE iserv_country
                              SET delete_state_code = 1
                              WHERE country_id = '{id}'";
            _sql.InsertData(sqlQuery);
            GetCountry.RemoveCountry(id);
        }
    }
}
