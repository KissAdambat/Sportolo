using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo.Models;
using System.Data;
using Sportolo.Models.DTOs;

namespace Sportolo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyekController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=sportolo13b;User Id=root;Password=";
        [HttpGet("GETALL")]
        public List<Eredmenyek> GetAllEredmeny()
        {
            List<Eredmenyek> eredmenyek = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM `eredmeny` WHERE 1";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var eredmeny = new Eredmenyek
                {
                    id = dr.GetInt32("Id"),
                    competition = dr.GetString("competition"),
                    description = dr.GetString("description"),
                    resultTime = dr.GetDateTime("resultTime"),
                    updateTime = dr.GetDateTime("updateTime"),
                    sportoloId = dr.GetInt32("sportoloId"),
                };
                eredmenyek.Add(eredmeny);
            }
            connector.Close();
            return eredmenyek;
        }

        [HttpGet("EredmenyGet")]
        public object EredmenyGet(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            List<Eredmenyek> eredmenyek = new();
            connector.Open();
            var sql = $"SELECT * FROM `eredmeny` WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var eredmeny = new Eredmenyek
                {
                    id = dr.GetInt32("Id"),
                    competition = dr.GetString("competition"),
                    description = dr.GetString("description"),
                    resultTime = dr.GetDateTime("resultTime"),
                    updateTime = dr.GetDateTime("updateTime"),
                    sportoloId = dr.GetInt32("sportoloId"),
                };
                eredmenyek.Add(eredmeny);
            }
            connector.Close();
            return eredmenyek;
        }

        [HttpPost("EredmenyPost")]
        public object AddNewEredmeny(AddEredmenyDTOs eredmeny)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var erd = new Eredmenyek
            {
                competition = eredmeny.competition,
                description = eredmeny.description,
                resultTime = DateTime.Now,
                updateTime = DateTime.Now,
                sportoloId = eredmeny.sportoloId
            };

            var sql = $"INSERT INTO `eredmeny`(`competition`, `description`, `resultTime`, `updateTime`, `sportoloId`) VALUES (@comp,@desc,@res,@upd,@spid)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@comp", erd.competition);
            cmd.Parameters.AddWithValue("@desc", erd.description);
            cmd.Parameters.AddWithValue("@res", erd.resultTime);
            cmd.Parameters.AddWithValue("@upd", erd.updateTime);
            cmd.Parameters.AddWithValue("@spid", erd.sportoloId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return erd;
        }
    }
}
