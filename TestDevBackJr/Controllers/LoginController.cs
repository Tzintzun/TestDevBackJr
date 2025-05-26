using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDevBackJr.Data;
using TestDevBackJr.Models;
using TestDevBackJr.Models.Dto;


namespace TestDevBackJr.Controllers
{
    [ApiController]
    [Route("/logins")]
    public class LoginController : Controller
    {
        private readonly CCenterRIAContext _centerRIAContext;

        public LoginController(CCenterRIAContext centerRIAContext)
        {
            _centerRIAContext = centerRIAContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogins()
        {
            try
            {
                var logins = await _centerRIAContext.Logins.Select(l => new LoginDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Extension = l.Extension,
                    TipoMov = l.TipoMov,
                    Fecha = l.Fecha,
                }).ToListAsync();

                if (logins == null || !logins.Any())
                {
                    return NotFound("No se encontraron registros.");
                }

                return Ok(logins);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        [HttpPost]
        public async Task<IActionResult> NewLog([FromBody] LoginDto loginDto)
        {
            
            if ( (await _centerRIAContext.Users.FindAsync(loginDto.UserId)) == null)
            {
                return NotFound("Usuario no encontrado");
            }

            Login lasLog = await this.GetLasLog((int)loginDto.UserId);
            if (lasLog == null)
            {
                if(loginDto.TipoMov == 0)
                {
                    return BadRequest($"El usuario {loginDto.UserId} no cuenta con registros de Login");
                }
            }
            else
            {
                Debug.WriteLine(lasLog.Fecha.ToString());
                if (lasLog.TipoMov == loginDto.TipoMov)
                {
                    return BadRequest($"El usuario ya cuenta con una sesión {loginDto.UserId} {(loginDto.TipoMov == 1 ? "abierta" : "cerrada")}");
                }

                if (DateTime.Compare(lasLog.Fecha, (DateTime)loginDto.Fecha) >= 0)
                {
                    return BadRequest($"El usuario cuenta con un registro posterior a {loginDto.Fecha.ToString()}");
                }
            }

            

            Login newLog = new Login
            {
                UserId = (int)loginDto.UserId,
                Extension = (int)loginDto.Extension,
                TipoMov = (int)loginDto.TipoMov,
                Fecha = (DateTime)loginDto.Fecha,
            };
            _centerRIAContext.Logins.Add(newLog);
            try
            {
                int filasAgregadas = await _centerRIAContext.SaveChangesAsync();
                if ( filasAgregadas > 0)
                {
                    loginDto.Id = newLog.Id;
                    return Ok(loginDto);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
            return StatusCode(500, $"Error al registrar la sessión");

        }

        [HttpPut("{logId}")]
        public async Task<IActionResult> UpdateLog(int logId, [FromBody] LoginDto updateLogDto )
        {


            Login login = await _centerRIAContext.Logins.FindAsync(logId);
            if (login == null)
            {
                return NotFound($"El Id {logId} del login no se econtro.");
            }
            if ((await _centerRIAContext.Users.FindAsync(updateLogDto.UserId)) == null)
            {
                return NotFound("Usuario no encontrado");
            }
            

            login.TipoMov = (int)updateLogDto.TipoMov;
            login.Extension = (int)updateLogDto.Extension;
            login.Fecha = (DateTime)updateLogDto.Fecha;
            try
            {
                if ((await _centerRIAContext.SaveChangesAsync()) > 0)
                {
                    return Ok(new LoginDto { 
                        Id = login.Id,
                        UserId = login.UserId,
                        Extension = login.Extension,
                        TipoMov = login.TipoMov,
                        Fecha = login.Fecha
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

            return StatusCode(500, $"Error al actualizar el registro {logId}");


        }

        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteLog(int logId)
        {
            Login log = await _centerRIAContext.Logins.FindAsync(logId);
            if ( log == null)
            {
                return NotFound($"Registro {logId} no encontrado.");
            }

            _centerRIAContext.Logins.Remove(log);
            try
            {
                if (await _centerRIAContext.SaveChangesAsync() > 0)
                {
                    return Ok($"Registro {logId} eliminado correctamente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return StatusCode(500, $"Error al eliminar el registro {logId}");
        }
        private  async Task<Login?> GetLasLog(int userId)
        {

            /*User user = _centerRIAContext.Users.Find(userId);
            Login lasLogAttemp = _centerRIAContext.Logins.Where(l => l.Fecha == user.LastLoginAttempt && l.UserId == userId).First();
            if (lasLogAttemp != null) {
                return lasLogAttemp;
            }*/
            return await _centerRIAContext.Logins.Where(l => l.UserId == userId).OrderByDescending(l => l.Fecha).FirstOrDefaultAsync();
        }
    }
}
