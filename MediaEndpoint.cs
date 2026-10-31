using System;

using System.Collections.Generic;

using System.Text;
using System.Text.Json;

namespace HttpServerDemo
{
    internal class MediaEndpoint : HTTPEndpoint
    {
        private TokenService tokenService;
        private MediaService mediaService;

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MediaEndpoint(string url, TokenService tokenService, MediaService mediaService) : base(url)
        {
            this.tokenService = tokenService;
            this.mediaService = mediaService;
        }

        public override void HandleRequest(Request request, Response response)
        {
            // Check token
            Token? token = GetValidToken(request);

            if (token == null)
            {
                response.SendResponse(401, "application/json", "{\"error\":\"Unauthorized\"}");
                return;
            }

            // Get all media
            if (request.Method == RequestMethod.GET && request.Endpoint == "/api/media")
            {
                List<Media> mediaList = mediaService.GetAll();

                string json = JsonSerializer.Serialize(mediaList);

                response.SendResponse(200, "application/json", json);
                return;
            }

            // Create media
            if (request.Method == RequestMethod.POST && request.Endpoint == "/api/media")
            {
                Media? media = JsonSerializer.Deserialize<Media>(request.Body, jsonOptions); ;

                if (media == null)
                {
                    response.SendResponse(400, "application/json", "{\"error\":\"Invalid media data\"}");
                    return;
                }

                bool success = mediaService.Create(media, token.UserId);

                if (success)
                    response.SendResponse(201, "application/json", "{\"message\":\"Media created successfully\"}");
                else
                    response.SendResponse(500, "application/json", "{\"error\":\"Could not create media\"}");

                return;
            }

            // Get media by ID
            if (request.Method == RequestMethod.GET && request.Endpoint.StartsWith("/api/media/"))
            {
                string idText = request.Endpoint.Substring("/api/media/".Length);

                if (!int.TryParse(idText, out int id))
                {
                    response.SendResponse(400, "application/json", "{\"error\":\"Invalid media ID\"}");
                    return;
                }

                Media? media = mediaService.GetById(id);

                if (media != null)
                {
                    string json = JsonSerializer.Serialize(media);
                    response.SendResponse(200, "application/json", json);
                }
                else
                {
                    response.SendResponse(404, "application/json", "{\"error\":\"Media not found\"}");
                }

                return;
            }

            // Update media
            if (request.Method == RequestMethod.PUT && request.Endpoint.StartsWith("/api/media/"))
            {
                string idText = request.Endpoint.Substring("/api/media/".Length);

                if (!int.TryParse(idText, out int id))
                {
                    response.SendResponse(400, "application/json", "{\"error\":\"Invalid media ID\"}");
                    return;
                }

                Media? media = JsonSerializer.Deserialize<Media>(request.Body, jsonOptions);

                if (media == null)
                {
                    response.SendResponse(400, "application/json", "{\"error\":\"Invalid media data\"}");
                    return;
                }

                media.Id = id;

                bool success = mediaService.Update(media, token.UserId);

                if (success)
                    response.SendResponse(200, "application/json", "{\"message\":\"Media updated successfully\"}");
                else
                    response.SendResponse(404, "application/json", "{\"error\":\"Media not found or not owned by user\"}");

                return;
            }

            // Delete media
            if (request.Method == RequestMethod.DELETE && request.Endpoint.StartsWith("/api/media/"))
            {
                string idText = request.Endpoint.Substring("/api/media/".Length);

                if (!int.TryParse(idText, out int id))
                {
                    response.SendResponse(400, "application/json", "{\"error\":\"Invalid media ID\"}");
                    return;
                }

                bool success = mediaService.DeleteById(id, token.UserId);

                if (success)
                    response.SendResponse(200, "application/json", "{\"message\":\"Media deleted successfully\"}");
                else
                    response.SendResponse(404, "application/json", "{\"error\":\"Media not found or not owned by user\"}");

                return;
            }

            response.SendResponse(404, "application/json", "{\"error\":\"Endpoint not found\"}");
        }

        private Token? GetValidToken(Request request)
        {
            if (string.IsNullOrEmpty(request.Authorization))
                return null;

            if (!request.Authorization.StartsWith("Bearer "))
                return null;

            string tokenValue = request.Authorization.Substring(7);

            return tokenService.ValidateToken(tokenValue);
        }
    }
}