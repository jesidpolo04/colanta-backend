namespace colanta_backend.App.CustomerCredit.Domain
{
    using System;
    public class InvalidOperationException : Exception
    {
        public string operation;
        public InvalidOperationException(string message, string operation) : base(message)
        {
            this.operation = operation;
        }
    }
}
