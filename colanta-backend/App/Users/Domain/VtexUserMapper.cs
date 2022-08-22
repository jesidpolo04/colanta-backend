namespace colanta_backend.App.Users.Domain
{
    public class VtexUserMapper
    {
        public static User getUserFromVtexUser(VtexUser vtexUser)
        {
            User user = new User();
            user.document = vtexUser.document;
            user.email = vtexUser.email;
            user.name = vtexUser.firstName;
            user.last_name = getFirstLastname(vtexUser.lastName);
            user.second_last_name = getSecondLastName(vtexUser.lastName);
            user.born_date = "";
            user.document_type = getDocumentType(vtexUser.documentType);
            user.telephone = vtexUser.homePhone;
            user.phone = vtexUser.phone;
            user.address = "";
            return user;
        }

        private static string getFirstLastname(string fullLastName)
        {
            return fullLastName.Split(" ")[0];
        }

        private static string getSecondLastName(string fullLastName)
        {
            string[] lastNames = fullLastName.Split(" ");
            if(lastNames.Length > 1) return lastNames[1];
            else return "";
        }
        private static string getDocumentType(string? documenType)
        {
            if (documenType == null) return null;
            if (documenType == "cedulaCOL") return "CC";
            else return "OTRO";
        }

    }
}
