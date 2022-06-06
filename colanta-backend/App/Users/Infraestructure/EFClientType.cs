namespace colanta_backend.App.Users.Infraestructure
{
    using Users.Domain;
    public class EFClientType
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string name { get; set; }

        public ClientType getClientTypeFromEfClientType()
        {
            ClientType clientType = new ClientType();
            clientType.id = id;
            clientType.siesa_id = siesa_id;
            clientType.name = name;

            return clientType;
        }

        public void setEfClientTypeFromClientType(ClientType clientType)
        {
            this.id = clientType.id;
            this.siesa_id = clientType.siesa_id;
            this.name = clientType.name;
        }
    }
}
