using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class CarreraRepository
    {
        // CREATE (Insertar)
        public bool Insertar(Carrera carrera)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "INSERT INTO carreras (clave_carrera, nombre_carrera) VALUES (@clave, @nombre)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", carrera.ClaveCarrera);
                    cmd.Parameters.AddWithValue("@nombre", carrera.NombreCarrera);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer todas)
        public List<Carrera> ObtenerTodas()
        {
            List<Carrera> lista = new List<Carrera>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT id_carrera, clave_carrera, nombre_carrera FROM carreras";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Carrera
                            {
                                IdCarrera = Convert.ToInt32(reader["id_carrera"]),
                                ClaveCarrera = reader["clave_carrera"].ToString(),
                                NombreCarrera = reader["nombre_carrera"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // UPDATE (Actualizar)
        public bool Actualizar(Carrera carrera)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE carreras SET clave_carrera = @clave, nombre_carrera = @nombre WHERE id_carrera = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", carrera.ClaveCarrera);
                    cmd.Parameters.AddWithValue("@nombre", carrera.NombreCarrera);
                    cmd.Parameters.AddWithValue("@id", carrera.IdCarrera);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // MÉTODO AUXILIAR (Buscar por Clave para evitar duplicados)
        public Carrera ObtenerPorClave(string clave)
        {
            Carrera carrera = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM carreras WHERE clave_carrera = @clave";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            carrera = new Carrera
                            {
                                IdCarrera = Convert.ToInt32(reader["id_carrera"]),
                                ClaveCarrera = reader["clave_carrera"].ToString(),
                                NombreCarrera = reader["nombre_carrera"].ToString()
                            };
                        }
                    }
                }
            }
            return carrera;
        }
    }
}