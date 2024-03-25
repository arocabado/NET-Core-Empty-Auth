namespace server.Dtos
{
    public class UsuarioDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public string? Telefono { get; set; }
        /*         public bool? Verificado { get; set; }
                public string? CodigoSecreto { get; set; }
                public string? Firma { get; set; } 
                public string? Notificacion { get; set; }*/
    }

    public class UsuarioGruposDTO
    {
        public required List<Guid> Idsgrupo { get; set; }
    }
    /*     public class RecUsuarioGruposDTO
        {
            public required List<int> Idsgrupo { get; set; }
        } */
}
