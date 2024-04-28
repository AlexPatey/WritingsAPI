namespace Writings.Api
{
    public class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Writings
        {
            private const string Base = $"{ApiBase}/writings";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string GetAllByYear = $"{Base}/{{year:int}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        public static class Tags
        {
            private const string Base = $"{ApiBase}/tags";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
    }
}
