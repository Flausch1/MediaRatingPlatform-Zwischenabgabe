using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HttpServerDemo
{
    enum RequestMethod
    {
        GET,
        POST,
        DELETE,
        PUT
    }

    internal class Request
    {
        public RequestMethod Method { get; set; }
        public string Endpoint { get; set; }
        public string Body { get; set; }

        // Authorization header
        public string? Authorization { get; set; }

        public Request(HttpListenerRequest request)
        {
            // Method
            if (request.HttpMethod == "GET")
                Method = RequestMethod.GET;
            else if (request.HttpMethod == "POST")
                Method = RequestMethod.POST;
            else if (request.HttpMethod == "PUT")
                Method = RequestMethod.PUT;
            else if (request.HttpMethod == "DELETE")
                Method = RequestMethod.DELETE;

            // Endpoint
            Endpoint = request.RawUrl ?? "/";

            // Authorization header
            Authorization = request.Headers["Authorization"];

            // Body
            if (request.HasEntityBody)
            {
                using (var body = request.InputStream)
                using (var reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    Body = reader.ReadToEnd();
                }
            }
            else
            {
                Body = "";
            }

            PrintRequest(request);
        }

        private void PrintRequest(HttpListenerRequest req)
        {
            Console.WriteLine("---------- Received request ----------");
            Console.WriteLine($"{req.HttpMethod} {req.Url}");

            foreach (string header in req.Headers)
            {
                Console.WriteLine($"{header}: {req.Headers[header]}");
            }

            Console.WriteLine();
            Console.WriteLine(Body);
        }
    }
}