using HttpServerDemo.Data;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace HttpServerDemo
{
    internal class UserEndpoint : HTTPEndpoint
    {
        private UserService userService;

        private TokenService tokenService;
        private Dictionary<EndpointData, Action<Request, Response>> urlEndpoints;

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public UserEndpoint(string url, UserService service, TokenService tokenService) : base(url)
        {
            userService = service;

            this.tokenService = tokenService;

            urlEndpoints = new Dictionary<EndpointData, Action<Request, Response>>();

            urlEndpoints.Add(new EndpointData("/api/users/login", RequestMethod.POST), Login);
            urlEndpoints.Add(new EndpointData("/api/users/register", RequestMethod.POST), Register);
        }

        public override void HandleRequest(Request request, Response response)
        {
            foreach (KeyValuePair<EndpointData, Action<Request, Response>> item in urlEndpoints)
            {
                if (item.Key.Url == request.Endpoint && item.Key.Method == request.Method)
                {
                    item.Value.Invoke(request, response);
                    return;
                }
            }

            response.SendResponse(404, "application/json", "{\"error\":\"Endpoint not found\"}");
        }


        private void Login(Request request, Response response)
        {
            User? user = JsonSerializer.Deserialize<User>(request.Body, jsonOptions);

            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                response.SendResponse(400, "application/json", "{\"error\":\"Invalid user data\"}");
                return;
            }

            // Check login data
            User? loggedInUser = userService.Login(user);

            if (loggedInUser != null)
            {
                // Create token
                Token token = tokenService.CreateToken(loggedInUser.Id);

                string json = JsonSerializer.Serialize(new
                {
                    message = "Login successful",
                    token = token.Value
                });

                response.SendResponse(200, "application/json", json);
            }
            else
            {
                response.SendResponse(401, "application/json", "{\"error\":\"Invalid username or password\"}");
            }
        }


        private void Register(Request request, Response response)
        {
            User? user = JsonSerializer.Deserialize<User>(request.Body, jsonOptions);

            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                response.SendResponse(400, "application/json", "{\"error\":\"Invalid user data\"}");
                return;
            }

            bool success = userService.Register(user);

            if (success)
            {
                response.SendResponse(201, "application/json", "{\"message\":\"User registered successfully\"}");
            }
            else
            {
                response.SendResponse(409, "application/json", "{\"error\":\"Username already exists\"}");
            }
        }
    }
}