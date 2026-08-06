using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class EncuestaService
    {
        private readonly EncuestaRepository _encuestaRepo;

        public EncuestaService()
        {
            _encuestaRepo = new EncuestaRepository();
        }

        // CREATE (Lógica de Creación de Encuesta)
        public string CrearNuevaEncuesta(Encuesta nuevaEncuesta, List<PreguntaEncuesta> listaPreguntas, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para diseñar y publicar nuevas encuestas.";
            }

            // 2. Validaciones Lógicas
            if (string.IsNullOrWhiteSpace(nuevaEncuesta.NombreEncuesta))
            {
                return "Debe asignar un nombre o título a la encuesta.";
            }

            if (listaPreguntas == null || listaPreguntas.Count == 0)
            {
                return "La encuesta debe contener al menos una pregunta.";
            }

            // Por defecto, una encuesta nueva empieza activa
            nuevaEncuesta.Estado = "activa";

            // 3. Manda a guardar
            bool exito = _encuestaRepo.GuardarEncuestaCompleta(nuevaEncuesta, listaPreguntas);

            return exito ? "Encuesta y preguntas registradas exitosamente en el sistema."
                         : "Error al guardar la encuesta en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Encuesta> ObtenerEncuestasDisponibles()
        {
            return _encuestaRepo.ObtenerEncuestasActivas();
        }

        // CREATE (Lógica para Aplicar Encuestas)
        public string RegistrarRespuestasEgresado(List<RespuestaEncuesta> respuestas, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo == "Consultor (Solo Lectura)")
            {
                return "Acceso denegado: Su rol es de solo lectura y no tiene permisos para aplicar encuestas.";
            }

            // 2. Validaciones Lógicas
            if (respuestas == null || respuestas.Count == 0)
            {
                return "Error: No se puede guardar una encuesta sin respuestas.";
            }

            // Verifica de forma rápida que ninguna respuesta obligatoria venga vacía
            foreach (var resp in respuestas)
            {
                if (string.IsNullOrWhiteSpace(resp.RespuestaTexto) || resp.IdEgresado <= 0)
                {
                    return "Error: Faltan datos requeridos o el egresado no es válido en una o más respuestas.";
                }
            }

            // 3. Manda a guardar
            bool exito = _encuestaRepo.GuardarRespuestas(respuestas);

            return exito ? "Las respuestas del egresado han sido registradas exitosamente."
                         : "Error grave al intentar guardar las respuestas en la base de datos.";
        }
    }
}