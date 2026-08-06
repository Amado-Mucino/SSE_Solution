using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class EmpresaRepository
    {
        // CREATE (Insertar)
        public bool Insertar(Empresa empresa)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "INSERT INTO empresas (nombre, sector, ubicacion) VALUES (@nombre, @sector, @ubicacion)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", empresa.Nombre);
                    cmd.Parameters.AddWithValue("@sector", empresa.Sector ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ubicacion", empresa.Ubicacion ?? (object)DBNull.Value);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer Todas)
        public List<Empresa> ObtenerTodas()
        {
            List<Empresa> lista = new List<Empresa>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM empresas ORDER BY nombre ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Empresa
                            {
                                IdEmpresa = Convert.ToInt32(reader["id_empresa"]),
                                Nombre = reader["nombre"].ToString(),
                                Sector = reader["sector"].ToString(),
                                Ubicacion = reader["ubicacion"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // UPDATE (Actualizar)
        public bool Actualizar(Empresa empresa)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE empresas SET nombre = @nombre, sector = @sector, ubicacion = @ubicacion WHERE id_empresa = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", empresa.Nombre);
                    cmd.Parameters.AddWithValue("@sector", empresa.Sector ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ubicacion", empresa.Ubicacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", empresa.IdEmpresa);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // MÉTODO AUXILIAR (Buscar por nombre para evitar duplicados)
        public Empresa ObtenerPorNombre(string nombre)
        {
            Empresa empresa = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM empresas WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empresa = new Empresa
                            {
                                IdEmpresa = Convert.ToInt32(reader["id_empresa"]),
                                Nombre = reader["nombre"].ToString(),
                                Sector = reader["sector"].ToString(),
                                Ubicacion = reader["ubicacion"].ToString()
                            };
                        }
                    }
                }
            }
            return empresa;
        }
    }
}