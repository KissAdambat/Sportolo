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
                    email = dr.GetString("description"),
                };
                sportolok.Add(sport);
            }
            connector.Close();
            return sportolok;
        }
    }
}
