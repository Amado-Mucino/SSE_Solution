using System;

namespace SSE.Entidades.Modelos
{
    public class HistorialActualizacion
    {
        public int IdHistorial { get; set; }
        public int IdEgresado { get; set; }
        public int IdUsuario { get; set; } // El usuario que realizó el cambio
        public string CampoModificado { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Propiedades de apoyo para la UI (Lectura)
        public string NombreUsuario { get; set; } // Obtenido con un JOIN
    }
}