using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo
{
    internal abstract class HTTPEndpoint
    {
        public string Url { get; set; }

        public HTTPEndpoint(string url)
        {
            Url = url;
        }

        public abstract void HandleRequest(Request request, Response response);
    }
}
