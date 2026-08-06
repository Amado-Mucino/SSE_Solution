using System;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;
using BCrypt.Net;

namespace SSE.BLL.Servicios
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AuthService()
        {
            _usuarioRepo = new UsuarioRepository();
        }

        // Usamos 'out string mensaje' para comunicar a la UI el motivo exacto del error
        public Usuario Autenticar(string username, string passwordPlana, out string mensaje)
        {
            mensaje = string.Empty;

            // 1. Busca al usuario en la BD
            Usuario usuario = _usuarioRepo.ObtenerPorUsername(username);

            if (usuario == null)
            {
                mensaje = "Usuario o contraseña incorrectos.";
                return null;
            }

            // 2. Verifica si la cuenta ya está bloqueada
            if (usuario.CuentaBloqueada)
            {
                mensaje = "Su cuenta ha sido bloqueada por múltiples intentos fallidos. Contacte al administrador.";
                return null;
            }

            // 3. Verifica la contraseña
            bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(passwordPlana, usuario.PasswordHash);

            if (passwordCorrecta)
            {
                // Si entra con éxito, reseteamos sus intentos fallidos a 0 (si tenía alguno)
                if (usuario.IntentosFallidos > 0)
                {
                    _usuarioRepo.ActualizarIntentosYBloqueo(usuario.IdUsuario, 0, false);
                }

                usuario.PasswordHash = string.Empty; // Limpiamos el hash por seguridad
                mensaje = "Inicio de sesión exitoso.";
                return usuario;
            }
            else
            {
                // 4. Aumenta intentos fallidos
                int nuevosIntentos = usuario.IntentosFallidos + 1;
                bool debeBloquearse = nuevosIntentos >= 3;

                // Actualiza la base de datos con el nuevo conteo
                _usuarioRepo.ActualizarIntentosYBloqueo(usuario.IdUsuario, nuevosIntentos, debeBloquearse);

                if (debeBloquearse)
                {
                    mensaje = "Ha superado los 3 intentos permitidos. Su cuenta ha sido bloqueada.";
                }
                else
                {
                    int intentosRestantes = 3 - nuevosIntentos;
                    mensaje = $"Usuario o contraseña incorrectos. Le quedan {intentosRestantes} intento(s).";
                }

                return null;
            }
        }
    }
}