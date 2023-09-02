namespace WebClient
{
    public class CustomerCreateRequest
    {
        public CustomerCreateRequest()
        {
        }

        public CustomerCreateRequest(
            string FirstName,
            string LastName)
        {
            Firstname = FirstName;
            Lastname = LastName;
        }

        public string Firstname { get; init; }

        public string Lastname { get; init; }
    }
}