using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo
{
    internal class EndpointData
    {
        public string Url { get; set; }
        public RequestMethod Method { get; set; }

        public EndpointData(string url, RequestMethod method)
        {
            Url = url;
            Method = method;
        }
    }
}
