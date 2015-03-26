using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Microsoft.MockService.Middleware
{
    class BufferedBodyMiddleware : OwinMiddleware
    {
        public BufferedBodyMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            if (context.Request.Body != null)
            {
                var requestBodyBuffer = new MemoryStream();
                await context.Request.Body.CopyToAsync(requestBodyBuffer);
                requestBodyBuffer.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyBuffer;
            }

            var responseBodyStream = context.Response.Body;
            var responseBodyBuffer = new MemoryStream();
            context.Response.Body = responseBodyBuffer;

            await Next.Invoke(context);

            responseBodyBuffer.Seek(0, SeekOrigin.Begin);
            await responseBodyBuffer.CopyToAsync(responseBodyStream);
        }
    }
}