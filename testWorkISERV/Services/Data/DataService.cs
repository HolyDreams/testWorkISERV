using System.Data;
using testWorkISERV.Models;
using testWorkISERV.Services.SQL;

namespace testWorkISERV.Services.Data
{
    public class DataService : IDataService
    {
        private readonly SQLRequestService _sql;
        public DataService()
        {
            _sql = new SQLRequestService();
        }

        public DTOData[] GetData(Guid? countryId = null)
        {
            var sqlQuery = $@"SELECT c.name AS country,
                                     d.name,
                                     d.web_site
                              FROM iserv_country AS c INNER JOIN
                                     iserv_data AS d ON c.country_id = d.country_id AND
                                                        c.delete_state_code = 0 AND
                                                        d.delete_state_code = 0 {(countryId == null ? "" : $@"AND
                                                        c.country_id = '{countryId.Value}'")}";
            var res = _sql.GetData(sqlQuery);
            if (res == null || res.Rows.Count == 0)
                throw new Exception("Не найденно никаких данных!");

            return getResult(res);
        }

        public DTOData[] GetData(string search, Guid? countryId = null)
        {
            search = search.ToLower();
            var sqlQuery = $@"SELECT c.name AS country,
                                     d.name,
                                     d.web_site
                              FROM iserv_country AS c INNER JOIN
                                     iserv_data AS d ON c.country_id = d.country_id AND
                                                        c.delete_state_code = 0 AND
                                                        d.delete_state_code = 0 AND
                                                        LOWER(d.name) LIKE '%{search}%' {(countryId == null ? "" : $@"AND
                                                        c.country_id = '{countryId.Value}'")}";
            var res = _sql.GetData(sqlQuery);
            if (res == null || res.Rows.Count == 0)
                new List<DTOData>();

            return getResult(res);
        }

        private DTOData[] getResult(DataTable res)
        {
            return (from DataRow a in res.Rows
                    select new DTOData
                    {
                        CountryName = (string)a["country"],
                        Name = (string)a["Name"],
                        WebPages = a["web_site"] == DBNull.Value ? null : (string?)a["web_site"]
                    }).OrderBy(a => a.CountryName).ThenBy(a => a.Name).ToArray();
        }
    }
}
