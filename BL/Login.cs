using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Login
    {
        private readonly DL.PruebaTecnicaContext _context;

        public Login(DL.PruebaTecnicaContext context)
        {
            _context = context;
        }

        public ML.Result GetUserByEmail(ML.Login login)
        {
            ML.Result result = new ML.Result();

            try
            {
                var resultQuery = _context.LoginDTO
                .FromSqlInterpolated($"EXEC GetUserByEmail {login.Email}")
                .AsEnumerable()
                .SingleOrDefault();

                if (resultQuery != null)
                {

                    // comparar las contraseñas 

                    if (login.Password == resultQuery.Password)
                    {
                        ML.Usuario usuarioObj = new ML.Usuario();

                        usuarioObj.UserName = resultQuery.UserName;
                        usuarioObj.Email = resultQuery.Email;

                        usuarioObj.Rol = new ML.Rol();
                        usuarioObj.Rol.Name = resultQuery.RolName;

                        result.Object = usuarioObj;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Las credenciales son incorrectas.";
                    }

                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Las credenciales son incorrectas.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
