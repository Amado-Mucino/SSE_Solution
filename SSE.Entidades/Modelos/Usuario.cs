namespace SSE.Entidades.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }

        public int IntentosFallidos { get; set; }
        public bool CuentaBloqueada { get; set; }
    }
}