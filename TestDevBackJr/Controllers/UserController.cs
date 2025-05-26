using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDevBackJr.Data;
using TestDevBackJr.Models;



namespace TestDevBackJr.Controllers
{


    [Route("/users")]
    [ApiController]
    public class UserController : Controller
    {
       private readonly CCenterRIAContext _context;

        public UserController(CCenterRIAContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}/reporte")]
        public async Task<IActionResult> GetReporte(int? userId)
        {
            if (userId == null)
            {
                return BadRequest("El parametro userId debe ser un numero entero");
            }

            User user = await _context.Users.Include(u => u.Area).FirstOrDefaultAsync(u => u.Id == userId);

            
            if (user == null)
            {
                return NotFound($"No se encontro al usuario: {userId}");
            }


            var horas = await _context.UserHoras.FromSqlRaw($"EXEC ObtenerHorasTotalesUser {user.Id}").ToListAsync();

            if (horas == null)
            {
                return StatusCode(500, "No se pudo obtener el numero de horas.");
            }

            StringBuilder csvString = new StringBuilder();
            csvString.AppendLine("Nombre de usuario,Nombre Completo, Área, Total de horas trabajadas");

            csvString.AppendLine($"{user.Login},{user.Nombres} {user.ApellidoPaterno} {(user.ApellidoMaterno != null ? user.ApellidoMaterno: " ")}, {user.Area.Name}, {horas.First().Horas}");
            try
            {
                UTF8Encoding encoder = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                byte[] csvBytes =encoder.GetBytes(csvString.ToString());
                return File(csvBytes, "text/csv", $"Reporte_{user.Id}_{DateTime.Today.ToString("yyyy_MM_dd")}.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return StatusCode(500, "Error al general el archivo.");
        }
    }
}
