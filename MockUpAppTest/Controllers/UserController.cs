using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using MockUpAppTest.Models;

namespace MockUpAppTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration config) {
            _configuration = config;
        }

        [HttpGet]
        public JsonResult Get() {
            string query = @"select * from tbl_user";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MockupAppCon");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(sqlDataSource)) { 
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query,con)) {
                    reader = cmd.ExecuteReader();  
                    dt.Load(reader);    
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(dt); 
        }

        [HttpPost]
        public JsonResult Post(User usr)
        {
            string query = @"insert into tbl_user(namalengkap,username,password,status) values(@namalengkap,@username,@password,@status)";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MockupAppCon");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(sqlDataSource))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@namalengkap",usr.namalengkap);
                    cmd.Parameters.AddWithValue("@username", usr.username);
                    cmd.Parameters.AddWithValue("@password", usr.password);
                    cmd.Parameters.AddWithValue("@status", usr.status);
                    reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Insert Success!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from tbl_user where userid = @id";
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MockupAppCon");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(sqlDataSource))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Delete Success!");
        }
    }
}
