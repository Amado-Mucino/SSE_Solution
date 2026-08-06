using System;

namespace SSE.Entidades.Modelos
{
    public class Egresado
    {
        // Datos de Identificación
        public int IdEgresado { get; set; }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Curp { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }

        // Datos de Contacto
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string Fotografia { get; set; }

        // Datos Académicos
        public int IdCarrera { get; set; }
        public int IdGeneracion { get; set; }
        public DateTime FechaEgreso { get; set; }
        public bool Titulado { get; set; }

        // Datos Laborales
        public string EstadoLaboral { get; set; }
        public int? IdEmpresa { get; set; } // int? porque puede ser NULL en la BD
        public string Puesto { get; set; }
        public bool TrabajoRelacionado { get; set; }

        // Metadatos
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Propiedades de apoyo para la UI (Lectura)
        public string NombreCarrera { get; set; }
        public string NombreEmpresa { get; set; }
    }
}