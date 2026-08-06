using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class ModalidadTitulacionService
    {
        private readonly ModalidadTitulacionRepository _modalidadRepo;

        public ModalidadTitulacionService()
        {
            _modalidadRepo = new ModalidadTitulacionRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarModalidad(ModalidadTitulacion nuevaModalidad, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para gestionar las modalidades de titulación.";
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(nuevaModalidad.NombreModalidad))
            {
                return "El nombre de la modalidad de titulación es obligatorio.";
            }

            // 3. Valida duplicidad
            ModalidadTitulacion modalidadExistente = _modalidadRepo.ObtenerPorNombre(nuevaModalidad.NombreModalidad);
            if (modalidadExistente != null)
            {
                return "Error: Ya existe una modalidad de titulación registrada con ese nombre.";
            }

            // Asegura que al crearla, inicie activa por defecto
            nuevaModalidad.Activo = true;

            // 4. Guardar
            bool exito = _modalidadRepo.Insertar(nuevaModalidad);
            return exito ? "Modalidad de titulación registrada correctamente." : "Error al registrar la modalidad.";
        }

        // READ (Lógica de Lectura)
        public List<ModalidadTitulacion> ObtenerTodasLasModalidadesParaUI()
        {
            // Este método lo usará la interfaz visual para llenar el ComboBox
            // al momento de registrar la titulación de un egresado. Solo trae las activas.
            return _modalidadRepo.ObtenerSoloActivas();
        }

        public List<ModalidadTitulacion> ObtenerCatalogoCompleto(string rolUsuarioActivo)
        {
            // Este método lo usará la interfaz de "Configuración/Administración" 
            // para ver todas (incluso las desactivadas)
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return new List<ModalidadTitulacion>();
            }
            return _modalidadRepo.ObtenerTodas();
        }

        // UPDATE (Lógica de Actualización / Borrado Lógico)
        public string ActualizarModalidad(ModalidadTitulacion modalidad, string rolUsuarioActivo)
        {
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para modificar modalidades.";
            }

            if (string.IsNullOrWhiteSpace(modalidad.NombreModalidad))
            {
                return "El nombre de la modalidad no puede estar vacío.";
            }

            // Valida choque con otro registro existente
            ModalidadTitulacion modalidadExistente = _modalidadRepo.ObtenerPorNombre(modalidad.NombreModalidad);
            if (modalidadExistente != null && modalidadExistente.IdModalidad != modalidad.IdModalidad)
            {
                return "Error: Ese nombre ya le pertenece a otra modalidad.";
            }

            bool exito = _modalidadRepo.Actualizar(modalidad);
            return exito ? "Modalidad actualizada correctamente." : "Error al actualizar la base de datos.";
        }
    }
}