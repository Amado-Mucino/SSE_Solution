using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class CarreraService
    {
        private readonly CarreraRepository _carreraRepo;

        public CarreraService()
        {
            _carreraRepo = new CarreraRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarCarrera(Carrera nuevaCarrera, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad (Solo Administrador y Coordinador)
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para gestionar el catálogo de carreras.";
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(nuevaCarrera.ClaveCarrera) || string.IsNullOrWhiteSpace(nuevaCarrera.NombreCarrera))
            {
                return "La clave y el nombre de la carrera son obligatorios.";
            }

            // 3. Valida duplicidad de la Clave
            Carrera carreraExistente = _carreraRepo.ObtenerPorClave(nuevaCarrera.ClaveCarrera);
            if (carreraExistente != null)
            {
                return $"Error: La clave '{nuevaCarrera.ClaveCarrera}' ya está registrada en otra carrera.";
            }

            // 4. Manda a guardar
            bool exito = _carreraRepo.Insertar(nuevaCarrera);
            return exito ? "Carrera registrada correctamente." : "Error al registrar la carrera en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Carrera> ObtenerTodasLasCarreras()
        {
            // Todos los roles necesitan poder leer las carreras.
            return _carreraRepo.ObtenerTodas();
        }

        // UPDATE (Lógica de Actualización)
        public string ActualizarCarrera(Carrera carrera, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para modificar el catálogo de carreras.";
            }

            if (string.IsNullOrWhiteSpace(carrera.ClaveCarrera) || string.IsNullOrWhiteSpace(carrera.NombreCarrera))
            {
                return "La clave y el nombre de la carrera no pueden estar vacíos.";
            }

            // Valida que la nueva clave no choque con otra carrera distinta
            Carrera carreraExistente = _carreraRepo.ObtenerPorClave(carrera.ClaveCarrera);
            if (carreraExistente != null && carreraExistente.IdCarrera != carrera.IdCarrera)
            {
                return $"Error: La clave '{carrera.ClaveCarrera}' ya pertenece a otra carrera en el sistema.";
            }

            bool exito = _carreraRepo.Actualizar(carrera);
            return exito ? "Carrera actualizada correctamente." : "Error al actualizar la carrera.";
        }
    }
}