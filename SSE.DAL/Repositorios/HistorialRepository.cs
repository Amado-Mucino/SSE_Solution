using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class HistorialRepository
    {
        // CREATE (Insertar Log)
        public bool Insertar(HistorialActualizacion historial)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = @"INSERT INTO historial_actualizaciones 
                                (id_egresado, id_usuario, campo_modificado, valor_anterior, valor_nuevo, fecha_modificacion) 
                                VALUES 
                                (@egresado, @usuario, @campo, @anterior, @nuevo, @fecha)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@egresado", historial.IdEgresado);
                    cmd.Parameters.AddWithValue("@usuario", historial.IdUsuario);
                    cmd.Parameters.AddWithValue("@campo", historial.CampoModificado);
                    cmd.Parameters.AddWithValue("@anterior", historial.ValorAnterior ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nuevo", historial.ValorNuevo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer por Egresado para la UI)
        public List<HistorialActualizacion> ObtenerPorEgresado(int idEgresado)
        {
            List<HistorialActualizacion> lista = new List<HistorialActualizacion>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Une con la tabla usuarios para obtener el 'username' del responsable
                string query = @"SELECT h.*, u.username 
                                 FROM historial_actualizaciones h
                                 INNER JOIN usuarios u ON h.id_usuario = u.id_usuario
                                 WHERE h.id_egresado = @id_egresado
                                 ORDER BY h.fecha_modificacion DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id_egresado", idEgresado);
                    conexion.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new HistorialActualizacion
                            {
                                IdHistorial = Convert.ToInt32(reader["id_historial"]),
                                IdEgresado = Convert.ToInt32(reader["id_egresado"]),
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                CampoModificado = reader["campo_modificado"].ToString(),
                                ValorAnterior = reader["valor_anterior"].ToString(),
                                ValorNuevo = reader["valor_nuevo"].ToString(),
                                FechaModificacion = Convert.ToDateTime(reader["fecha_modificacion"]),
                                // Propiedad extraída del JOIN
                                NombreUsuario = reader["username"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}