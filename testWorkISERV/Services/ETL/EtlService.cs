using System.Text.Json;
using testWorkISERV.Methods;
using testWorkISERV.Models;
using testWorkISERV.Services.SQL;

namespace testWorkISERV.Services.ETL
{
    public class EtlService : IDisposable
    {
        private readonly SQLRequestService _request;
        bool started;
        bool good = true;
        public EtlService()
        {
            _request = new SQLRequestService();
        }

        public Task StartAsync()
        {
            if (started)
                return Task.CompletedTask;

            started = true;
            startSearch();

            while (good)
            {
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                {
                    startSearch();
                }

                Task.Delay(60000).Wait();
            }

            return Task.CompletedTask;
        }
        private async Task startSearch()
        {
            var country = GetCountry.Get();
            _request.CreateIfNotExistsDataTable();

            Parallel.ForEach(country, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async item =>
            {
                await getData(item.Key, item.Value);
            });
        }

        private async Task getData(Guid countryID, string countryName)
        {
            string res;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://universities.hipolabs.com/search?country=" + countryName.Replace(' ', '+'));
                res = await response.Content.ReadAsStringAsync();
            }
            var getData = JsonSerializer.Deserialize<ETLData[]>(res);
            if (getData == null || getData.Length == 0)
                throw new Exception($"Неудачная попытка найти даннные по {countryName} стране");

            var dbData = (from a in getData
                          select new DBData
                          {
                              CountryID = countryID,
                              CountryName = countryName,
                              Name = a.Name,
                              WebPages = string.Join(";", a.WebPages)
                          }).ToArray();

            var sql = new SQLRequestService();
            sql.InsertData(dbData);
        }

        public void Dispose()
        {
            good = false;
        }
    }
}
