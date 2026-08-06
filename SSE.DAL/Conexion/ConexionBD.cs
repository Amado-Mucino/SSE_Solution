using System;
using MySqlConnector;

namespace SSE.DAL.Conexion
{
    public static class ConexionBD
    {
        // Cadena de conexión centralizada.
        private static readonly string cadenaConexion = "Server=127.0.0.1;Database=sse_desktop_v1;Uid=root;Pwd=yL[Y73j//2-%;";

        // Este método devuelve un objeto de conexión listo para usarse en los repositorios
        public static MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}