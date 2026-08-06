namespace SSE.Entidades.Modelos
{
    public class Carrera
    {
        // Identificador único de la carrera (INT, PK, AI)
        public int IdCarrera { get; set; }

        // Clave oficial de la carrera (ej. 'ISC', 'LDE')
        public string ClaveCarrera { get; set; }

        // Nombre completo de la carrera
        public string NombreCarrera { get; set; }
    }
}