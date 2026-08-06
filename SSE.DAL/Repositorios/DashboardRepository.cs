using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class DashboardRepository
    {
        public DashboardResumen ObtenerDatosDashboard()
        {
            DashboardResumen resumen = new DashboardResumen();
            resumen.TitulacionesPorModalidad = new List<ItemGrafica>();

            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                // 1. Total de Egresados
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM egresados WHERE activo = 1", conexion))
                {
                    resumen.TotalEgresados = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Total de Titulados
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM egresados WHERE titulado = 1 AND activo = 1", conexion))
                {
                    resumen.TotalTitulados = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 3. Egresados Empleados
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM egresados WHERE estado_laboral = 'Empleado' AND activo = 1", conexion))
                {
                    resumen.EgresadosEmpleados = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 4. Encuestas Activas
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM encuestas WHERE estado = 'activa'", conexion))
                {
                    resumen.EncuestasActivas = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 5. Datos para la Gráfica de Modalidades (Agrupación)
                string queryGrafica = @"SELECT m.nombre_modalidad, COUNT(t.id_titulacion) as total 
                                        FROM titulaciones t 
                                        INNER JOIN modalidades_titulacion m ON t.id_modalidad = m.id_modalidad 
                                        GROUP BY m.nombre_modalidad";

                using (MySqlCommand cmd = new MySqlCommand(queryGrafica, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resumen.TitulacionesPorModalidad.Add(new ItemGrafica
                            {
                                Etiqueta = reader["nombre_modalidad"].ToString(),
                                Valor = Convert.ToInt32(reader["total"])
                            });
                        }
                    }
                }
            }

            return resumen;
        }
    }
}