namespace futr
{
    public class FutrRoles
    {
        public const string SiteEditor = "SiteEditor";
        public const string SiteAdmin = "SiteAdmin";
        public const string TokenSeparator = ",";

        public string User;
        public string[] Roles;

        public FutrRoles(string user, IEnumerable<string> roles)
        {
            User = user;
            Roles = roles.ToArray();
        }

        public static string AsString(IEnumerable<string> roles)
        {
            var trimmed = roles.Select(x => x.Trim());
            return string.Join(TokenSeparator, trimmed);
        }

        public static IEnumerable<string> FromString(string? roles)
        {
            if (roles != null) {
                return roles.Split(new[] { ',', ' ' }).Select(x => x.Trim()).ToArray();
            }

            return Array.Empty<string>();
        }
    }
}
