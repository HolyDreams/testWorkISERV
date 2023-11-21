using Npgsql;
using System.Data;
using testWorkISERV.Methods;
using testWorkISERV.Models;

namespace testWorkISERV.Services.SQL
{
    public class SQLRequestService : IDisposable
    {
        private readonly string _con = "connect";
        private NpgsqlConnection nc;
        public SQLRequestService()
        {
            if (nc == null)
                createConnect();
        }

        private void createConnect()
        {
            nc = new NpgsqlConnection(_con);
        }

        private void openConnect()
        {
            if (nc.State != ConnectionState.Open)
                nc.Open();
        }
        public void CreateIfNotExistsDataTable()
        {
            openConnect();
            var request = $@"CREATE TABLE IF NOT EXISTS public.iserv_data 
                             (data_id uuid NOT NULL DEFAULT gen_random_uuid(),
	                         country_id uuid NOT NULL,
	                         ""name"" text NOT NULL,
	                         web_site text NULL,
                             delete_state_code int2 NOT NULL DEFAULT 0,
	                         CONSTRAINT iserv_test_data_pk PRIMARY KEY (data_id),
	                         CONSTRAINT iserv_test_data_fk FOREIGN KEY (country_id) REFERENCES public.iserv_country(country_id),
                             CONSTRAINT iserv_test_data_un UNIQUE (""name""));";
            openConnect();
            var command = nc.CreateCommand();
            command.CommandText = request;
            command.ExecuteNonQuery();
        }

        public DataTable GetData(string sqlQuery)
        {
            openConnect();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlQuery, nc);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void InsertData(DBData[] data)
        {
            openConnect();
            for (int i = 0; i < data.Length; i++)
            {
                var command = nc.CreateCommand();

                string sql = $@"INSERT INTO iserv_data (country_id, name, web_site)
                                VALUES ('{data[i].CountryID}', '{data[i].Name.Replace("'", "")}', {(data[i].WebPages == null ? "null" : $@"'{data[i].WebPages.Replace("'", "")}'")})
                                ON CONFLICT (name)
                                DO UPDATE SET web_site = EXCLUDED.web_site";
                command.CommandText = sql;

                command.ExecuteNonQuery();
            }
        }

        public void InsertData(string sqlQuery)
        {
            openConnect();
            var command = nc.CreateCommand();
            command.CommandText = sqlQuery;
            command.ExecuteNonQuery();
        }
        public void Dispose()
        {
            nc.Dispose();
        }
    }
}
