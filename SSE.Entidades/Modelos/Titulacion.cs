using System;

namespace SSE.Entidades.Modelos
{
    public class Titulacion
    {
        public int IdTitulacion { get; set; }
        public int IdEgresado { get; set; }
        public int IdModalidad { get; set; }
        public DateTime FechaTitulacion { get; set; }
        public string NumActa { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Propiedades de apoyo para la UI (Lectura)
        public string NombreCompletoEgresado { get; set; }
        public string MatriculaEgresado { get; set; }
        public string NombreModalidad { get; set; }
    }
}