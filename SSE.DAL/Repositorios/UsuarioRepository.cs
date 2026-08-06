using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class UsuarioRepository
    {
        // CREATE (Insertar)
        public bool Insertar(Usuario usuario)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Solo inserta username, password_hash y id_rol. El id_usuario es Auto Incremental
                string query = "INSERT INTO usuarios (username, password_hash, id_rol) VALUES (@username, @password_hash, @id_rol)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", usuario.Username);
                    cmd.Parameters.AddWithValue("@password_hash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.IdRol);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer todos)
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Hace un JOIN con la tabla roles para obtener el nombre del rol
                string query = @"SELECT u.id_usuario, u.username, u.id_rol, r.nombre_rol 
                                 FROM usuarios u 
                                 INNER JOIN roles r ON u.id_rol = r.id_rol";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaUsuarios.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                Username = reader["username"].ToString(),
                                IdRol = Convert.ToInt32(reader["id_rol"]),
                                NombreRol = reader["nombre_rol"].ToString()
                                // Omite traer el PasswordHash por seguridad al listar usuarios
                            });
                        }
                    }
                }
            }
            return listaUsuarios;
        }

        // UPDATE (Actualizar)
        public bool Actualizar(Usuario usuario)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Nota: Aquí solo actualizamos el username y el rol.
                string query = "UPDATE usuarios SET username = @username, id_rol = @id_rol WHERE id_usuario = @id_usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", usuario.Username);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // UPDATE (Solo Contraseña)
        public bool ActualizarPassword(int idUsuario, string nuevoPasswordHash)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE usuarios SET password_hash = @password_hash WHERE id_usuario = @id_usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@password_hash", nuevoPasswordHash);
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int idUsuario)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "DELETE FROM usuarios WHERE id_usuario = @id_usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // Método auxiliar para buscar por username
        public Usuario ObtenerPorUsername(string username)
        {
            Usuario usuario = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Agrega las nuevas columnas al SELECT
                string query = @"SELECT u.id_usuario, u.username, u.password_hash, u.id_rol, 
                                r.nombre_rol, u.intentos_fallidos, u.cuenta_bloqueada
                         FROM usuarios u 
                         INNER JOIN roles r ON u.id_rol = r.id_rol 
                         WHERE u.username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                Username = reader["username"].ToString(),
                                PasswordHash = reader["password_hash"].ToString(),
                                IdRol = Convert.ToInt32(reader["id_rol"]),
                                NombreRol = reader["nombre_rol"].ToString(),

                                IntentosFallidos = Convert.ToInt32(reader["intentos_fallidos"]),
                                CuentaBloqueada = Convert.ToBoolean(reader["cuenta_bloqueada"])
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        // UPDATE (Seguridad y Bloqueos)
        public void ActualizarIntentosYBloqueo(int idUsuario, int intentos, bool bloqueada)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE usuarios SET intentos_fallidos = @intentos, cuenta_bloqueada = @bloqueada WHERE id_usuario = @id_usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@intentos", intentos);
                    cmd.Parameters.AddWithValue("@bloqueada", bloqueada);
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}