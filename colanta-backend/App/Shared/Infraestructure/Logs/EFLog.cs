namespace colanta_backend.App.Shared.Infraestructure
{
    using System;
    public class EFLog
    {
        public string id;
        public string message;
        public string stack;
        public string exception;
        public DateTime throw_at;
    }
}
