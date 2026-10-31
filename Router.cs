using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpServerDemo
{
    internal class Router
    {
        public List<HTTPEndpoint> Endpoints { get; set; } = new List<HTTPEndpoint>();

        public void AddEndpoint(HTTPEndpoint endpoint)
        {
            Endpoints.Add(endpoint);
        }

        public HTTPEndpoint? GetEndpoint(string url)
        {
            // Remove query parameters
            string path = url.Split('?')[0];

            // Split URL into parts
            string[] parts = path.Split('/');

            // Check if URL has enough parts
            if (parts.Length < 3 || parts[1] != "api")
                return null;

            // Get endpoint name
            string endpointURL = parts[2];

            foreach (HTTPEndpoint endpoint in Endpoints)
            {
                if (endpoint.Url == endpointURL)
                    return endpoint;
            }

            return null;
        }
    }
}