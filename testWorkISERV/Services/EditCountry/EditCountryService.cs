using testWorkISERV.Services.SQL;

namespace testWorkISERV.Services.Country
{
    public class EditCountryService : IEditCountryService
    {
        private readonly SQLRequestService _sql;
        public EditCountryService()
        {
            _sql = new SQLRequestService();
        }

        public void AddCountry(string name)
        {
            var sqlQuery = $@"INSERT INTO iserv_country (name)
                              VALUES ('{name}')
                              ON CONFLICT (name)
                              DO NOTHING";
            _sql.InsertData(sqlQuery);
        }
        public void RemoveCountry(Guid id)
        {
            var sqlQuery = $@"UPDATE iserv_country
                              SET delete_state_code = 1
                              WHERE country_id = '{id}'";
            _sql.InsertData(sqlQuery);
        }
    }
}
