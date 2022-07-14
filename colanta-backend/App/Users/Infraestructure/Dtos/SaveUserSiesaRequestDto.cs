namespace colanta_backend.App.Users.Infraestructure
{
    public class SaveUserSiesaRequestDto
    {
        public string documento { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string direccion { get; set; }
        public string pais { get; set; }
        public string depto { get; set; }
        public string ciudad { get; set; }
        public string barrio { get; set; }
        public string celular { get; set; }
        public string fechanacimiento { get; set; }
        public string negocio { get; set; }

    }
}
