namespace server.Constants
{
    public static class Messages
    {
        public static class Auth
        {
            public static string FOUND { get; } = "Usuario obtenido correctamente";
            public static string NOT_FOUND { get; } = "No se encontró el usuario";
            public static string CREATED { get; } = "Usuario registrado correctamente";
            public static string UPDATED { get; } = "Usuario actualizado exitosamente";
            public static string UPDATED_PASSWORD { get; } = "Contraseña actualizada exitosamente";
            public static string EXISTS_USERNAME { get; } = "Este usuario ya existe";
            public static string UNAUTHORIZED { get; } = "Las credenciales son inválidas";
            public static string ERROR_CONFIG { get; } = "No se encontró la configuración para generar el token";
            public static string ERROR_TOKEN { get; } = "Token inválido";
            public static string ERROR_PASSWORD_ACTUAL { get; } = "La contraseña actual no coincide";
            public static string ERROR_PASSWORD_BODY { get; } = "Las contraseña nueva no coincide";
            public static string REQUIRED { get; } = "Todos los campos son requeridos";
            public static string ERROR_PASSWORD { get; } = "Contraseña incorrecta";
             public static string NOT_FOUND_PROJECT { get; } = "No se encontró el proyecto";
        }


        public static class Usuario
        {
            public static string GET { get; } = "Usuarios obtenidos correctamente";
            public static string FIND { get; } = "Usuario obtenido correctamente";
            public static string NOTFOUND { get; } = "No se encontró el Usuario";
            public static string CREATED { get; } = "Usuario creado exitosamente";
            public static string UPDATED { get; } = "Usuario modificado exitosamente";
            public static string DELETED { get; } = "Usuario eliminado exitosamente";
            public static string EXISTS { get; } = "Ya existe una Usuario con ese nombre";
            public static string NOTCONTACTO { get; } = "No se encontro el contacto seleccionado";
            public static string NOTEMPRESA { get; } = "No se encontro la empresa seleccionada";
            public static string NOTTIPOUSUARIO { get; } = "No se encontro el tipo usuario seleccionado";


        }
        public static class RiModelo
        {
            public static string GET { get; } = "Modelos obtenidos correctamente";
            public static string FIND { get; } = "Modelo obtenido correctamente";
            public static string NOTFOUND { get; } = "No se encontró el modelo";
            public static string CREATED { get; } = "Modelo creado exitosamente";
            public static string UPDATED { get; } = "Modelo modificado exitosamente";
            public static string DELETED { get; } = "Modelo eliminado exitosamente";
            public static string EXISTS { get; } = "Ya existe un modelo con ese nombre";

        }

        public static class RiMenu
        {
            public static string GET { get; } = "Menus obtenidos correctamente";
            public static string FIND { get; } = "Menu obtenido correctamente";
            public static string NOTFOUND { get; } = "No se encontró el menu";
            public static string CREATED { get; } = "menu creado exitosamente";
            public static string UPDATED { get; } = "menu modificado exitosamente";
            public static string DELETED { get; } = "menu eliminado exitosamente";
            public static string EXISTS { get; } = "Ya existe un menu con ese nombre";
            public static string NOTPADRE { get; } = "No se encontró el menú padre seleccionado";

        }


        public static class RecGrupo
        {
            public static string GET { get; } = "Grupos obtenidos correctamente";
            public static string FIND { get; } = "Grupo obtenido correctamente";
            public static string NOTFOUND { get; } = "No se encontró el grupo";
            public static string CREATED { get; } = "Grupo creado exitosamente";
            public static string UPDATED { get; } = "Grupo modificado exitosamente";
            public static string DELETED { get; } = "Grupo eliminado exitosamente";
            public static string EXISTS { get; } = "Ya existe un grupo con ese nombre";

        }

    }
}