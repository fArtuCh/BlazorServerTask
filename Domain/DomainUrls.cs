
namespace Domain;

public static class DomainUrls
{

    public const string BaseUrl = "/";

    public static class HttpCodes
    {
        private const string Base = BaseUrl + "Errors/";
        public const string NotFound = Base + "404";
        public const string NotAuthorized = Base + "401";
    }


    public static class UnLogged
    {
        private const string UnLoggedUrl = BaseUrl + "";

        public const string Home = UnLoggedUrl + "";

        public const string SecondPage = UnLoggedUrl + "DiffrentPage/";
    }

    public static class Logged
    {

        private const string UnLoggedUrl = BaseUrl + "Logged/";
    }


}
