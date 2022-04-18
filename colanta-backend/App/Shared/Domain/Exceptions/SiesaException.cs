namespace colanta_backend.App.Shared.Domain
{
    using System;
    public class SiesaException : Exception
    {
        public int status;
        public SiesaException(int status, string message)
            : base(message)
        {
            this.status = status;
        }
    }
}
