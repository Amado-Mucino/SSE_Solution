using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class RolRepository
    {
        // READ (Leer todos los roles)
        public List<Rol> ObtenerTodos()
        {
            List<Rol> listaRoles = new List<Rol>();

            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Consulta sencilla para traer el catálogo completo
                string query = "SELECT id_rol, nombre_rol FROM roles";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaRoles.Add(new Rol
                            {
                                IdRol = Convert.ToInt32(reader["id_rol"]),
                                NombreRol = reader["nombre_rol"].ToString()
                            });
                        }
                    }
                }
            }
            return listaRoles;
        }
    }
}