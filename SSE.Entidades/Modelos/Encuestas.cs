namespace SSE.Entidades.Modelos
{
    // 1. Representa la tabla 'encuestas'
    public class Encuesta
    {
        public int IdEncuesta { get; set; }
        public string NombreEncuesta { get; set; }
        public string Estado { get; set; } // 'activa' o 'cerrada'
    }

    // 2. Representa la tabla 'preguntas_encuesta'
    public class PreguntaEncuesta
    {
        public int IdPregunta { get; set; }
        public int IdEncuesta { get; set; }
        public string TextoPregunta { get; set; }
        public string TipoPregunta { get; set; } // 'abierta', 'opcion_multiple', 'escala'
    }

    // 3. Representa la tabla 'respuestas_encuesta'
    public class RespuestaEncuesta
    {
        public int IdRespuesta { get; set; }
        public int IdEgresado { get; set; }
        public int IdPregunta { get; set; }
        public string RespuestaTexto { get; set; }
    }
}