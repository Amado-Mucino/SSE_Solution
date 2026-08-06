using System.Collections.Generic;

namespace SSE.Entidades.Modelos
{
    // Clase auxiliar para transportar datos de las gráficas (Eje X y Eje Y)
    public class ItemGrafica
    {
        public string Etiqueta { get; set; }
        public int Valor { get; set; }
    }

    public class DashboardResumen
    {
        // Tarjetas de Indicadores Clave (KPIs)
        public int TotalEgresados { get; set; }
        public int TotalTitulados { get; set; }
        public double PorcentajeTitulados { get; set; }
        public int EgresadosEmpleados { get; set; }
        public double PorcentajeEmpleados { get; set; }
        public int EncuestasActivas { get; set; }

        // Listas para alimentar las gráficas de LiveCharts2
        public List<ItemGrafica> TitulacionesPorModalidad { get; set; }
    }
}