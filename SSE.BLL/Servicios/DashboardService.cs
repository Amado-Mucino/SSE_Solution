using System;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class DashboardService
    {
        private readonly DashboardRepository _dashboardRepo;

        public DashboardService()
        {
            _dashboardRepo = new DashboardRepository();
        }

        public DashboardResumen ObtenerResumenGlobal()
        {
            // 1. Obtiene los conteos brutos desde la base de datos
            DashboardResumen resumen = _dashboardRepo.ObtenerDatosDashboard();

            // 2. Cálculo de porcentajes
            if (resumen.TotalEgresados > 0)
            {
                // Calcula el % de titulados y lo redonde a 2 decimales
                double porcentajeTit = ((double)resumen.TotalTitulados / resumen.TotalEgresados) * 100;
                resumen.PorcentajeTitulados = Math.Round(porcentajeTit, 2);

                // Calcula el % de empleados y lo redondea a 2 decimales
                double porcentajeEmp = ((double)resumen.EgresadosEmpleados / resumen.TotalEgresados) * 100;
                resumen.PorcentajeEmpleados = Math.Round(porcentajeEmp, 2);
            }
            else
            {
                // Evita la división entre cero si el sistema está recién instalado y vacío
                resumen.PorcentajeTitulados = 0;
                resumen.PorcentajeEmpleados = 0;
            }

            return resumen;
        }
    }
}