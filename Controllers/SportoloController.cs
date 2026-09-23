using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo.Models;
using System.Data;

namespace Sportolo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=sportolo13b;User Id=root;Password=";
        [HttpGet("SportoloGet")]
        public object SportoloGet(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            List<SportoloCL> sportolok = new();
            connector.Open();
            var sql = $"SELECT `name`, `email` FROM `sportolo` WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var sport = new SportoloCL
                {
                    name = dr.GetString("name"),
                    email = dr.GetString("email"),
                };
                sportolok.Add(sport);
            }
            connector.Close();
            return sportolok;
        }

        [HttpGet("SportoloDESC")]
        public object GetASportoloRun(string name)
        {
            var connector = new MySqlConnection(ConnectionString);
            int idf = 0;
            List<Eredmenyek> eredmenyek = new();
            connector.Open();
            var sql = $"SELECT `id` FROM sportolo WHERE `name`=@name";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", name);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                idf = dr.GetInt32("id");
            }
            connector.Close();
            connector.Open();
            var sql2 = $"SELECT `competition`, `description` FROM eredmeny WHERE `sportoloId`=@sportoloId";
            var cmd2 = new MySqlCommand(sql2, connector);
            cmd2.Parameters.AddWithValue("@sportoloId", idf);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var eredmeny = new Eredmenyek
                {
                    competition = dr2.GetString("competition"),
                    description = dr2.GetString("description"),
                };
                eredmenyek.Add(eredmeny);
            }

            connector.Close();
            return eredmenyek;
        }

        [HttpGet("EredmenyCount")]
        public string GetAllEredmeny()
        {
            int darab = 0;
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT COUNT(id) AS Eredmenyek FROM eredmeny;";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                darab = dr.GetInt32("Eredmenyek");
            }
            connector.Close();
            return $"Eredmeny tablaba ennyi eredmeny van: {darab}";
        }

        [HttpGet("SportoloEredmenyDarab")]
        public string GetASportoloEredmenyDarab(string name)
        {
            var connector = new MySqlConnection(ConnectionString);
            int idf = 0;
            int darab = 0;
            connector.Open();
            var sql = $"SELECT `id` FROM sportolo WHERE `name`=@name";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", name);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                idf = dr.GetInt32("id");
            }
            connector.Close();
            connector.Open();
            var sql2 = $"SELECT COUNT(*) AS eredmenyekSzama FROM eredmeny WHERE sportoloId = @sportoloId;";
            var cmd2 = new MySqlCommand(sql2, connector);
            cmd2.Parameters.AddWithValue("@sportoloId", idf);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                darab = dr2.GetInt32("eredmenyekSzama");
            }

            connector.Close();
            return $"Ennyi eredménye van {name} : {darab} ";
        }
    }
}
