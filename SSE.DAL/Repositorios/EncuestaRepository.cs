using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class EncuestaRepository
    {
        // CREATE (Guarda Encuesta + Preguntas)
        public bool GuardarEncuestaCompleta(Encuesta encuesta, List<PreguntaEncuesta> preguntas)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // 1. Inserta la encuesta
                        string queryEncuesta = "INSERT INTO encuestas (nombre_encuesta, estado) VALUES (@nombre, @estado); SELECT LAST_INSERT_ID();";
                        int idEncuestaGenerado = 0;

                        using (MySqlCommand cmdEncuesta = new MySqlCommand(queryEncuesta, conexion, transaccion))
                        {
                            cmdEncuesta.Parameters.AddWithValue("@nombre", encuesta.NombreEncuesta);
                            cmdEncuesta.Parameters.AddWithValue("@estado", encuesta.Estado);

                            // Obtiené el ID que MariaDB le acaba de asignar a esta nueva encuesta
                            idEncuestaGenerado = Convert.ToInt32(cmdEncuesta.ExecuteScalar());
                        }

                        // 2. Inserta todas las preguntas asociadas
                        string queryPregunta = "INSERT INTO preguntas_encuesta (id_encuesta, texto_pregunta, tipo_pregunta) VALUES (@id_encuesta, @texto, @tipo)";

                        foreach (var pregunta in preguntas)
                        {
                            using (MySqlCommand cmdPregunta = new MySqlCommand(queryPregunta, conexion, transaccion))
                            {
                                cmdPregunta.Parameters.AddWithValue("@id_encuesta", idEncuestaGenerado); // Usa el ID recién creado
                                cmdPregunta.Parameters.AddWithValue("@texto", pregunta.TextoPregunta);
                                cmdPregunta.Parameters.AddWithValue("@tipo", pregunta.TipoPregunta);
                                cmdPregunta.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit(); // Confirma los cambios
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback(); // Realiza un rollback si hay error
                        return false;
                    }
                }
            }
        }

        // READ (Obtener Encuestas Activas para la UI)
        public List<Encuesta> ObtenerEncuestasActivas()
        {
            List<Encuesta> lista = new List<Encuesta>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM encuestas WHERE estado = 'activa'";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Encuesta
                            {
                                IdEncuesta = Convert.ToInt32(reader["id_encuesta"]),
                                NombreEncuesta = reader["nombre_encuesta"].ToString(),
                                Estado = reader["estado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // ==========================
        // CREATE (Transaccional: Guardar Respuestas de un Egresado)
        // ==========================
        public bool GuardarRespuestas(List<RespuestaEncuesta> listaRespuestas)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                // Iniciamos la transacción para guardar todas las respuestas juntas
                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO respuestas_encuesta 
                                         (id_egresado, id_pregunta, respuesta_texto) 
                                         VALUES (@id_egresado, @id_pregunta, @respuesta)";

                        foreach (var respuesta in listaRespuestas)
                        {
                            using (MySqlCommand cmd = new MySqlCommand(query, conexion, transaccion))
                            {
                                cmd.Parameters.AddWithValue("@id_egresado", respuesta.IdEgresado);
                                cmd.Parameters.AddWithValue("@id_pregunta", respuesta.IdPregunta);
                                cmd.Parameters.AddWithValue("@respuesta", respuesta.RespuestaTexto);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Si el ciclo termina sin errores, confirmamos todos los inserts
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre cualquier error, revertimos la transacción completa
                        transaccion.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}