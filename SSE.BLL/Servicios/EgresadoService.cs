using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class EgresadoService
    {
        private readonly EgresadoRepository _egresadoRepo;

        public EgresadoService()
        {
            _egresadoRepo = new EgresadoRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarEgresado(Egresado nuevoEgresado, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad por Rol
            if (rolUsuarioActivo == "Consultor (Solo Lectura)")
            {
                return "Acceso denegado: Los consultores no tienen permisos para registrar egresados.";
            }

            // 2. Validaciones básicas de campos obligatorios
            if (string.IsNullOrWhiteSpace(nuevoEgresado.Matricula) ||
                string.IsNullOrWhiteSpace(nuevoEgresado.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoEgresado.Curp))
            {
                return "La matrícula, el nombre y el CURP son campos obligatorios.";
            }

            if (nuevoEgresado.IdCarrera <= 0 || nuevoEgresado.IdGeneracion <= 0)
            {
                return "Debe seleccionar una carrera y una generación válidas.";
            }

            // 3. Valida que la matrícula no exista (Integridad de datos papi, para que veas que sí le sé)
            Egresado egresadoExistente = _egresadoRepo.ObtenerPorMatricula(nuevoEgresado.Matricula);
            if (egresadoExistente != null)
            {
                return $"Error: La matrícula {nuevoEgresado.Matricula} ya se encuentra registrada en el sistema.";
            }

            // 4. Manda a guardar al DAL
            bool exito = _egresadoRepo.Insertar(nuevoEgresado);

            return exito ? "Egresado registrado correctamente." : "Error al registrar al egresado en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Egresado> ObtenerTodosLosEgresados(string rolUsuarioActivo)
        {
            // Según el documento de Jorge, todos los roles pueden consultar.
            return _egresadoRepo.ObtenerTodos();
        }

        // UPDATE (Lógica de Actualización)
        public string ActualizarEgresado(Egresado egresado, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            // El consultor no puede editar, los demás sí.
            if (rolUsuarioActivo == "Consultor (Solo Lectura)")
            {
                return "Acceso denegado: Su rol es de solo lectura y no tiene permisos para editar egresados.";
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(egresado.Matricula) || string.IsNullOrWhiteSpace(egresado.Nombre))
            {
                return "La matrícula y el nombre no pueden estar vacíos.";
            }

            // 3. Valida que si cambiaron la matrícula, la nueva no le pertenezca a otro
            Egresado egresadoExistente = _egresadoRepo.ObtenerPorMatricula(egresado.Matricula);
            if (egresadoExistente != null && egresadoExistente.IdEgresado != egresado.IdEgresado)
            {
                return "Error: Esa matrícula ya está registrada y pertenece a otro egresado.";
            }

            // 4. Manda a actualizar
            bool exito = _egresadoRepo.Actualizar(egresado);
            return exito ? "Datos del egresado actualizados correctamente." : "Error al actualizar la base de datos.";
        }

        // DELETE (Lógica de Eliminación)
        public string EliminarEgresado(int idEgresado, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            // Solo Administrador y Coordinador pueden dar de baja.
            if (rolUsuarioActivo == "Capturista" || rolUsuarioActivo == "Consultor (Solo Lectura)")
            {
                return "Acceso denegado: Su rol no cuenta con los privilegios necesarios para eliminar registros de egresados.";
            }

            // 2. Manda a eliminar
            bool exito = _egresadoRepo.Eliminar(idEgresado);

            // Nota: Si el egresado ya tiene titulaciones o encuestas asociadas, 
            // MariaDB podría bloquear el delete por integridad referencial (resumen: Ni le muevan).
            return exito ? "Egresado eliminado exitosamente del sistema." : "Error al eliminar. Es posible que el egresado tenga encuestas o titulaciones vinculadas.";
        }
    }
}