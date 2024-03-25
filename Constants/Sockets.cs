namespace server.Constants
{
    public static class Sockets
    {
        public static class Types
        {
            public static string CONNECT { get; } = "connect";
            public static string COUNT { get; } = "count";
            public static string DISCONNECT { get; } = "disconnect";
            public static string MESSAGE { get; } = "message";
            public static string USERMODIFY { get; } = "userModify";
            public static string USERGROUP { get; } = "userGroup";
        }
    }
}
