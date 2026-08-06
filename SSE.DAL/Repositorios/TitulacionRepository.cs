using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class TitulacionRepository
    {
        // CREATE
        public bool RegistrarTitulacionTransaccional(Titulacion titulacion)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();
                // Inicia una transacción SQL
                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Paso 1: Inserta el acta de titulación
                        string queryTitulacion = @"INSERT INTO titulaciones 
                                                  (id_egresado, id_modalidad, fecha_titulacion, num_acta, observaciones, fecha_registro) 
                                                  VALUES 
                                                  (@egresado, @modalidad, @fecha, @acta, @observaciones, @fecha_reg)";

                        using (MySqlCommand cmdTitulacion = new MySqlCommand(queryTitulacion, conexion, transaccion))
                        {
                            cmdTitulacion.Parameters.AddWithValue("@egresado", titulacion.IdEgresado);
                            cmdTitulacion.Parameters.AddWithValue("@modalidad", titulacion.IdModalidad);
                            cmdTitulacion.Parameters.AddWithValue("@fecha", titulacion.FechaTitulacion);
                            cmdTitulacion.Parameters.AddWithValue("@acta", titulacion.NumActa);
                            cmdTitulacion.Parameters.AddWithValue("@observaciones", titulacion.Observaciones);
                            cmdTitulacion.Parameters.AddWithValue("@fecha_reg", DateTime.Now);

                            cmdTitulacion.ExecuteNonQuery();
                        }

                        // Paso 2: Actualiza el estado del egresado a Titulado = 1
                        string queryEgresado = "UPDATE egresados SET titulado = 1 WHERE id_egresado = @id_egresado";
                        using (MySqlCommand cmdEgresado = new MySqlCommand(queryEgresado, conexion, transaccion))
                        {
                            cmdEgresado.Parameters.AddWithValue("@id_egresado", titulacion.IdEgresado);
                            cmdEgresado.ExecuteNonQuery();
                        }

                        // Si ambos pasos salen bien, confirma la transacción
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Si algo falla, revierte cualquier cambio
                        transaccion.Rollback();
                        return false;
                    }
                }
            }
        }

        // READ
        public Titulacion ObtenerPorEgresado(int idEgresado)
        {
            Titulacion tit = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Busca si el egresado ya tiene un registro en esta tabla
                string query = "SELECT * FROM titulaciones WHERE id_egresado = @id_egresado LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id_egresado", idEgresado);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tit = new Titulacion
                            {
                                IdTitulacion = Convert.ToInt32(reader["id_titulacion"]),
                                IdEgresado = Convert.ToInt32(reader["id_egresado"]),
                                NumActa = reader["num_acta"].ToString()
                            };
                        }
                    }
                }
            }
            return tit;
        }

        // READ (Leer Todas para la UI)
        public List<Titulacion> ObtenerTodas()
        {
            List<Titulacion> lista = new List<Titulacion>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Une la tabla Egresados y Modalidades para traer los textos amigables
                string query = @"SELECT t.*, e.matricula, e.nombre, e.apellido_paterno, m.nombre_modalidad 
                                 FROM titulaciones t
                                 INNER JOIN egresados e ON t.id_egresado = e.id_egresado
                                 INNER JOIN modalidades_titulacion m ON t.id_modalidad = m.id_modalidad";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Titulacion
                            {
                                IdTitulacion = Convert.ToInt32(reader["id_titulacion"]),
                                IdEgresado = Convert.ToInt32(reader["id_egresado"]),
                                IdModalidad = Convert.ToInt32(reader["id_modalidad"]),
                                FechaTitulacion = Convert.ToDateTime(reader["fecha_titulacion"]),
                                NumActa = reader["num_acta"].ToString(),
                                Observaciones = reader["observaciones"].ToString(),
                                // Datos extra del JOIN
                                MatriculaEgresado = reader["matricula"].ToString(),
                                NombreCompletoEgresado = $"{reader["nombre"]} {reader["apellido_paterno"]}",
                                NombreModalidad = reader["nombre_modalidad"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}