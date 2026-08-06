using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class HistorialService
    {
        private readonly HistorialRepository _historialRepo;

        public HistorialService()
        {
            _historialRepo = new HistorialRepository();
        }

        // CREATE (Guardar cambio en la bitácora)
        public bool RegistrarCambio(HistorialActualizacion nuevoCambio)
        {
            // Validaciones lógicas para evitar basura en el log
            if (nuevoCambio.IdEgresado <= 0 || nuevoCambio.IdUsuario <= 0)
            {
                return false; // Error silencioso para no interrumpir el flujo principal
            }

            if (string.IsNullOrWhiteSpace(nuevoCambio.CampoModificado))
            {
                return false;
            }

            // Evita guardar si el valor viejo y nuevo son exactamente el mismo
            if (nuevoCambio.ValorAnterior == nuevoCambio.ValorNuevo)
            {
                return false;
            }

            return _historialRepo.Insertar(nuevoCambio);
        }

        // READ (Consultar bitácora de un egresado)
        public List<HistorialActualizacion> ObtenerHistorialDeEgresado(int idEgresado)
        {
            if (idEgresado <= 0)
            {
                return new List<HistorialActualizacion>();
            }

            // Retorna los registros ordenados del más reciente al más antiguo
            return _historialRepo.ObtenerPorEgresado(idEgresado);
        }
    }
}