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
            const string title = "Pidecolanta.com: Solicitud de eliminación de cuenta de usuario";
            string message = $"El usuario {name} {lastName} ha solicitado la eliminación de su cuenta en PideColanta.com.\nDatos del usuario:\n- Email: {email}\n- Cédula: {document}\nAtentamente,\nEquipo Pidecolanta.com";
            this.emailSender.sendEmailWithoutTemplate(
                title, 
                message,
                "jesdady482@gmail.com;pidecolanta@colanta.com.co;williamre@colanta.com.co;cristianro@colanta.com.co;mauriciosp@colanta.com.co"
             );
        }
    }
}
