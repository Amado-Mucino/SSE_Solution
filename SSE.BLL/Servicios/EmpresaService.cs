using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class EmpresaService
    {
        private readonly EmpresaRepository _empresaRepo;

        public EmpresaService()
        {
            _empresaRepo = new EmpresaRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarEmpresa(Empresa nuevaEmpresa, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para dar de alta empresas.";
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(nuevaEmpresa.Nombre))
            {
                return "El nombre de la empresa es un campo obligatorio.";
            }

            // 3. Valida duplicidad
            Empresa empresaExistente = _empresaRepo.ObtenerPorNombre(nuevaEmpresa.Nombre);
            if (empresaExistente != null)
            {
                return $"Error: La empresa '{nuevaEmpresa.Nombre}' ya se encuentra registrada en el sistema.";
            }

            // 4. Guardar
            bool exito = _empresaRepo.Insertar(nuevaEmpresa);
            return exito ? "Empresa registrada exitosamente." : "Error al registrar la empresa en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Empresa> ObtenerTodasLasEmpresas()
        {
            // Se permite la lectura a todos los roles para poder llenar los ComboBox 
            // al momento de registrar el empleo de un egresado.
            return _empresaRepo.ObtenerTodas();
        }

        // UPDATE (Lógica de Actualización)
        public string ActualizarEmpresa(Empresa empresa, string rolUsuarioActivo)
        {
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para modificar los datos de las empresas.";
            }

            if (string.IsNullOrWhiteSpace(empresa.Nombre))
            {
                return "El nombre de la empresa no puede estar vacío.";
            }

            // Valida choque con otra empresa existente
            Empresa empresaExistente = _empresaRepo.ObtenerPorNombre(empresa.Nombre);
            if (empresaExistente != null && empresaExistente.IdEmpresa != empresa.IdEmpresa)
            {
                return $"Error: El nombre '{empresa.Nombre}' ya está siendo utilizado por otro registro de empresa.";
            }

            bool exito = _empresaRepo.Actualizar(empresa);
            return exito ? "Datos de la empresa actualizados correctamente." : "Error al actualizar la base de datos.";
        }
    }
}