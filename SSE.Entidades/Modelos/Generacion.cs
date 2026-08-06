namespace SSE.Entidades.Modelos
{
    public class Generacion
    {
        public int IdGeneracion { get; set; }

        // Se usa string o int (año), aquí usaremos int para validaciones matemáticas fáciles
        public int AñoIngreso { get; set; }
        public int AñoEgreso { get; set; }

        public int IdCarrera { get; set; }

        // Propiedad de apoyo (obtenida mediante un JOIN)
        public string NombreCarrera { get; set; }
    }
}