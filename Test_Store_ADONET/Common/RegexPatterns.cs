namespace Test_Store_ADONET.Common
{
    public static class RegexPatterns
    {
        public const string BrandName = @"^(?!\s*$)[a-zA-Z0-9\s\-]+$"; // Allows letters, numbers, spaces, and hyphens, must contain at least one non-whitespace character

    }
}
