using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class EgresadoRepository
    {
        // CREATE (Insertar)
        public bool Insertar(Egresado egresado)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = @"INSERT INTO egresados 
                                (matricula, nombre, apellido_paterno, apellido_materno, curp, sexo, 
                                 fecha_nacimiento, correo, telefono, domicilio, id_carrera, id_generacion, 
                                 fecha_egreso, titulado, estado_laboral, fecha_registro, activo) 
                                VALUES 
                                (@matricula, @nombre, @apellido_paterno, @apellido_materno, @curp, @sexo, 
                                 @fecha_nacimiento, @correo, @telefono, @domicilio, @id_carrera, @id_generacion, 
                                 @fecha_egreso, @titulado, @estado_laboral, @fecha_registro, @activo)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@matricula", egresado.Matricula);
                    cmd.Parameters.AddWithValue("@nombre", egresado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido_paterno", egresado.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@apellido_materno", egresado.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@curp", egresado.Curp);
                    cmd.Parameters.AddWithValue("@sexo", egresado.Sexo);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", egresado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@correo", egresado.Correo);
                    cmd.Parameters.AddWithValue("@telefono", egresado.Telefono);
                    cmd.Parameters.AddWithValue("@domicilio", egresado.Domicilio);
                    cmd.Parameters.AddWithValue("@id_carrera", egresado.IdCarrera);
                    cmd.Parameters.AddWithValue("@id_generacion", egresado.IdGeneracion);
                    cmd.Parameters.AddWithValue("@fecha_egreso", egresado.FechaEgreso);
                    cmd.Parameters.AddWithValue("@titulado", egresado.Titulado);
                    cmd.Parameters.AddWithValue("@estado_laboral", egresado.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@activo", true);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // Método Auxiliar de Búsqueda
        public Egresado ObtenerPorMatricula(string matricula)
        {
            Egresado egresado = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM egresados WHERE matricula = @matricula";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            egresado = new Egresado
                            {
                                IdEgresado = Convert.ToInt32(reader["id_egresado"]),
                                Matricula = reader["matricula"].ToString()
                                // Aquí se agregarían el resto de campos en caso de ser necesarios en el fúturo.
                            };
                        }
                    }
                }
            }
            return egresado;
        }

        // READ (Leer todos)
        public List<Egresado> ObtenerTodos()
        {
            List<Egresado> lista = new List<Egresado>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Se hace un JOIN con carreras para obtener el nombre textual de la carrera
                string query = @"SELECT e.*, c.nombre_carrera 
                                 FROM egresados e 
                                 INNER JOIN carreras c ON e.id_carrera = c.id_carrera";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Egresado
                            {
                                IdEgresado = Convert.ToInt32(reader["id_egresado"]),
                                Matricula = reader["matricula"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                ApellidoPaterno = reader["apellido_paterno"].ToString(),
                                ApellidoMaterno = reader["apellido_materno"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                IdCarrera = Convert.ToInt32(reader["id_carrera"]),
                                NombreCarrera = reader["nombre_carrera"].ToString(), // Dato del JOIN
                                Titulado = Convert.ToBoolean(reader["titulado"]),
                                EstadoLaboral = reader["estado_laboral"].ToString(),
                                Activo = Convert.ToBoolean(reader["activo"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // UPDATE (Actualizar)
        public bool Actualizar(Egresado egresado)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Se actualizan los campos principales. Nota: La matrícula generalmente no se edita 
                // para mantener la integridad, pero lo puse por si hubo un error de captura.
                string query = @"UPDATE egresados SET 
                                 matricula = @matricula, nombre = @nombre, 
                                 apellido_paterno = @apellido_paterno, apellido_materno = @apellido_materno, 
                                 curp = @curp, correo = @correo, telefono = @telefono, 
                                 id_carrera = @id_carrera, id_generacion = @id_generacion, 
                                 estado_laboral = @estado_laboral 
                                 WHERE id_egresado = @id_egresado";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@matricula", egresado.Matricula);
                    cmd.Parameters.AddWithValue("@nombre", egresado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido_paterno", egresado.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@apellido_materno", egresado.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@curp", egresado.Curp);
                    cmd.Parameters.AddWithValue("@correo", egresado.Correo);
                    cmd.Parameters.AddWithValue("@telefono", egresado.Telefono);
                    cmd.Parameters.AddWithValue("@id_carrera", egresado.IdCarrera);
                    cmd.Parameters.AddWithValue("@id_generacion", egresado.IdGeneracion);
                    cmd.Parameters.AddWithValue("@estado_laboral", egresado.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@id_egresado", egresado.IdEgresado);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // DELETE (Eliminar)
        public bool Eliminar(int idEgresado)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                // Eliminación física del registro
                string query = "DELETE FROM egresados WHERE id_egresado = @id_egresado";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id_egresado", idEgresado);
                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}