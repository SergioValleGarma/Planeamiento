namespace Common.Auth
{
    public class AccessTokenUserInformation
    {
        public string unique_name { get; set; }
        public string nbf { get; set; }
        public int exp { get; set; }
        public string iat { get; set; }
    }
}