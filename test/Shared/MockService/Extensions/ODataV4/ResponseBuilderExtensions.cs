namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class ResponseBuilderExtensions
    {
        public static MockService RespondWithCreateEntity(this ResponseBuilder responseBuilder, string entitySetName, object response = null)
        {

            return responseBuilder.RespondWith((c, b) =>
            {
                c.Response.StatusCode = 201;
                c.Response.WithDefaultODataHeaders();
                c.Response.WithODataEntityResponseBody(b, entitySetName, response);
            });
        }

        public static MockService RespondWithGetEntity(this ResponseBuilder responseBuilder, string entitySetName, object response = null)
        {

            return responseBuilder.RespondWith((c, b) =>
            {
                c.Response.StatusCode = 200;
                c.Response.WithDefaultODataHeaders();
                c.Response.WithODataEntityResponseBody(b, entitySetName, response);
            });
        }

        public static MockService RespondWithODataOk(this ResponseBuilder responseBuilder)
        {
            return responseBuilder
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                });
        }

        public static MockService RespondWithODataText(this ResponseBuilder responseBuilder, string text)
        {
            return responseBuilder
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                    c.Response.ContentType = "text/plain";
                    c.Response.Write(text);
                });
        }
    }
}