using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;
using BCrypt.Net;

namespace SSE.BLL.Servicios
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuarioService()
        {
            _usuarioRepo = new UsuarioRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarUsuario(Usuario nuevoUsuario, string passwordPlana, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad por Rol
            if (rolUsuarioActivo != "Administrador")
            {
                return "Acceso denegado: Solo los administradores pueden registrar nuevos usuarios.";
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Username) || string.IsNullOrWhiteSpace(passwordPlana))
            {
                return "El nombre de usuario y la contraseña son obligatorios.";
            }
            if (nuevoUsuario.IdRol <= 0)
            {
                return "Debe seleccionar un rol válido.";
            }

            // 3. Valida que el username no exista
            Usuario usuarioExistente = _usuarioRepo.ObtenerPorUsername(nuevoUsuario.Username);
            if (usuarioExistente != null)
            {
                return "El nombre de usuario ya está registrado. Elija uno diferente.";
            }

            // 4. Cifra la contraseña y la guardar
            nuevoUsuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlana);
            bool exito = _usuarioRepo.Insertar(nuevoUsuario);

            return exito ? "Usuario registrado correctamente." : "Error al registrar el usuario en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Usuario> ObtenerTodosLosUsuarios(string rolUsuarioActivo)
        {
            // Barrera de Seguridad: Evita que alguien que no es admin extraiga la lista de usuarios
            if (rolUsuarioActivo != "Administrador")
            {
                // Retorna una lista vacía para no romper la interfaz gráfica si intentan consultarlo
                return new List<Usuario>();
            }

            return _usuarioRepo.ObtenerTodos();
        }

        // UPDATE (Lógica de Actualización)
        public string ActualizarUsuario(Usuario usuario, string rolUsuarioActivo)
        {
            // Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador")
            {
                return "Acceso denegado: No cuenta con privilegios para modificar usuarios.";
            }

            if (string.IsNullOrWhiteSpace(usuario.Username))
            {
                return "El nombre de usuario no puede estar vacío.";
            }

            Usuario usuarioExistente = _usuarioRepo.ObtenerPorUsername(usuario.Username);
            if (usuarioExistente != null && usuarioExistente.IdUsuario != usuario.IdUsuario)
            {
                return "El nombre de usuario ya está en uso por otra cuenta.";
            }

            bool exito = _usuarioRepo.Actualizar(usuario);
            return exito ? "Usuario actualizado correctamente." : "Error al actualizar el usuario.";
        }

        // DELETE (Lógica de Eliminación)
        public string EliminarUsuario(int idUsuarioAEliminar, string rolUsuarioActivo)
        {
            // Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador")
            {
                return "Acceso denegado: Su rol no tiene permisos para eliminar usuarios.";
            }

            if (idUsuarioAEliminar == 1)
            {
                return "No se puede eliminar la cuenta del administrador principal del sistema.";
            }

            bool exito = _usuarioRepo.Eliminar(idUsuarioAEliminar);
            return exito ? "Usuario eliminado correctamente." : "Error al eliminar el usuario.";
        }

        // UPDATE (Cambio de Contraseña)
        public string CambiarPassword(int idUsuario, string nuevaPasswordPlana, string rolUsuarioActivo)
        {
            // Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador")
            {
                return "Acceso denegado: Solo un administrador puede resetear las contraseñas.";
            }

            if (string.IsNullOrWhiteSpace(nuevaPasswordPlana) || nuevaPasswordPlana.Length < 6)
            {
                return "La contraseña no puede estar vacía y debe tener al menos 6 caracteres.";
            }

            string nuevoHash = BCrypt.Net.BCrypt.HashPassword(nuevaPasswordPlana);
            bool exito = _usuarioRepo.ActualizarPassword(idUsuario, nuevoHash);

            return exito ? "Contraseña actualizada correctamente." : "Error al actualizar la contraseña.";
        }
    }
}