namespace SSE.Entidades.Modelos
{
    public class ModalidadTitulacion
    {
        public int IdModalidad { get; set; }
        public string NombreModalidad { get; set; }
        public bool Activo { get; set; } // Representa el TINYINT(1) de la base de datos
    }
}