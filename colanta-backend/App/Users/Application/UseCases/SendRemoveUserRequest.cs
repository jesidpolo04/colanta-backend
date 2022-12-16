namespace colanta_backend.App.Users.Application
{
    using Shared.Domain;
    public class SendRemoveUserRequest
    {
        private EmailSender emailSender;

        public SendRemoveUserRequest(EmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public void Invoke(string email, string name, string lastName, string document, string documentType)
        {
            const string title = "Solicitud de eliminación de cuenta Pidecolanta.";
            string message = $"El usuario {name} {lastName} identificado con {documentType} número {document}. \n Solicita la eliminación de su cuenta Pidecolanta. \n Email del usuario: {email}";
            this.emailSender.sendEmailWithoutTemplate(title, message, "jesdady482@gmail.com");
        }
    }
}
