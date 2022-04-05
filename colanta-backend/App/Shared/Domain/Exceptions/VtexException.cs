namespace colanta_backend.App.Shared.Domain
{
    using System;
    public class VtexException : Exception
    {
        public VtexException(string Message)
            :base(Message)
        {

        }
    }
}
