namespace Ferreteria.BL
{
    public class Clientes
    {
        public int? Id { get; set; }
        public string? Cedula { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }
        public int? Edad { get; set; }
        public Ciudad? Ciudad { get; set; }
        

    }
}