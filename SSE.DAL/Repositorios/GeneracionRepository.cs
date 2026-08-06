using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class GeneracionRepository
    {
        // CREATE (Insertar)
        public bool Insertar(Generacion generacion)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Nota: Usamos año_ingreso y año_egreso para agregar a la BD
                string query = "INSERT INTO generaciones (año_ingreso, año_egreso, id_carrera) VALUES (@ingreso, @egreso, @id_carrera)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@ingreso", generacion.AñoIngreso);
                    cmd.Parameters.AddWithValue("@egreso", generacion.AñoEgreso);
                    cmd.Parameters.AddWithValue("@id_carrera", generacion.IdCarrera);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer todas con JOIN)
        public List<Generacion> ObtenerTodas()
        {
            List<Generacion> lista = new List<Generacion>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Hace un JOIN con carreras para obtener el nombre textual
                string query = @"SELECT g.id_generacion, g.año_ingreso, g.año_egreso, g.id_carrera, c.nombre_carrera 
                                 FROM generaciones g 
                                 INNER JOIN carreras c ON g.id_carrera = c.id_carrera";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Generacion
                            {
                                IdGeneracion = Convert.ToInt32(reader["id_generacion"]),
                                AñoIngreso = Convert.ToInt32(reader["año_ingreso"]),
                                AñoEgreso = Convert.ToInt32(reader["año_egreso"]),
                                IdCarrera = Convert.ToInt32(reader["id_carrera"]),
                                NombreCarrera = reader["nombre_carrera"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // UPDATE (Actualizar)
        public bool Actualizar(Generacion generacion)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE generaciones SET anio_ingreso = @ingreso, año_egreso = @egreso, id_carrera = @id_carrera WHERE id_generacion = @id_generacion";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@ingreso", generacion.AñoIngreso);
                    cmd.Parameters.AddWithValue("@egreso", generacion.AñoEgreso);
                    cmd.Parameters.AddWithValue("@id_carrera", generacion.IdCarrera);
                    cmd.Parameters.AddWithValue("@id_generacion", generacion.IdGeneracion);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // MÉTODO AUXILIAR (Buscar duplicados exactos)
        public Generacion ObtenerExacta(int anioIngreso, int anioEgreso, int idCarrera)
        {
            Generacion gen = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM generaciones WHERE año_ingreso = @ingreso AND año_egreso = @egreso AND id_carrera = @id_carrera";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@ingreso", anioIngreso);
                    cmd.Parameters.AddWithValue("@egreso", anioEgreso);
                    cmd.Parameters.AddWithValue("@id_carrera", idCarrera);

                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            gen = new Generacion
                            {
                                IdGeneracion = Convert.ToInt32(reader["id_generacion"]),
                                AñoIngreso = Convert.ToInt32(reader["año_ingreso"]),
                                AñoEgreso = Convert.ToInt32(reader["año_egreso"]),
                                IdCarrera = Convert.ToInt32(reader["id_carrera"])
                            };
                        }
                    }
                }
            }
            return gen;
        }
    }
}