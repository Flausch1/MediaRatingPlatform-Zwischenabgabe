// File: Program.cs
using HttpServerDemo;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SimpleHttpServer
{
    Router router = new Router();

    internal void AddEndpoint(HTTPEndpoint endpoint)
    {
        router.AddEndpoint(endpoint);
    }

    public async Task Run()
    {
        var listener = new HttpListener();

        listener.Prefixes.Add("http://localhost:8080/");

        listener.Start();

        Console.WriteLine("Listening on http://localhost:8080/ (Press Ctrl+C to exit)");
        Console.WriteLine();

        try
        {
            while (true)
            {
                var ctx = await listener.GetContextAsync();
                _ = HandleRequest(ctx);
            }
        }
        finally
        {
            listener.Stop();
            listener.Close();
        }
    }

    private async Task HandleRequest(HttpListenerContext ctx)
    {
        Response response = new Response(ctx.Response);

        try
        {
            Request request = new Request(ctx.Request);

            HTTPEndpoint? endpoint = router.GetEndpoint(request.Endpoint);

            if (endpoint != null)
            {
                endpoint.HandleRequest(request, response);
            }
            else
            {
                await response.SendResponse(
                    404,
                    "application/json",
                    "{\"error\":\"Endpoint not found\"}"
                );
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);

            await response.SendResponse(
                500,
                "application/json",
                "{\"error\":\"Internal server error\"}"
            );
        }

        Console.WriteLine("================================");
    }
}