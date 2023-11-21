using System.Linq;
using testWorkISERV.Services.SQL;

namespace testWorkISERV.Methods
{
    public class GetCountry
    {
        static Dictionary<Guid, string> Countrys;
        public static Dictionary<Guid, string> Get()
        {
            if (Countrys == null || Countrys.Count == 0)
                setCountry();

            return Countrys;
        }
        public static Guid? Get(string name)
        {
            if (Countrys == null || Countrys.Count == 0)
                setCountry();

            return Countrys.FirstOrDefault(a => a.Value == name).Key;
        }
        public static string? Get(Guid id)
        {
            if (Countrys == null || Countrys.Count == 0)
                setCountry();

            return Countrys[id] ?? null;
        }
        private static void setCountry()
        {
            Countrys = new Dictionary<Guid, string>();
            var sqlQuery = $@"SELECT *
                              FROM iserv_country
                              WHERE delete_state_code = 0";
            var SQL = new SQLRequestService();
            var res = SQL.GetData(sqlQuery);
            if (res == null || res.Rows.Count == 0)
                return;

            for (int i = 0; i < res.Rows.Count; i++)
            {
                Countrys.Add((Guid)res.Rows[i][0], (string)res.Rows[i][1]);
            }
        }
    }
}
