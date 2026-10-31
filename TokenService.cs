using HttpServerDemo.Data;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HttpServerDemo
{
    internal class TokenService
    {
        private ITokenRepository tokenRepository;

        public TokenService(ITokenRepository repository)
        {
            tokenRepository = repository;
        }

        public Token CreateToken(int userId)
        {
            // Generate a random token
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
            string tokenValue = Convert.ToBase64String(randomBytes);

            // Set expiration date (24 hours)
            Token token = new Token
            {
                Value = tokenValue,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            // Save token in database
            tokenRepository.Create(token);

            return token;
        }

        public Token? ValidateToken(string tokenValue)
        {
            // Find token in database
            Token? token = tokenRepository.GetByValue(tokenValue);

            if (token == null)
                return null;

            // Check expiration date
            if (token.ExpiresAt <= DateTime.UtcNow)
            {
                tokenRepository.DeleteByValue(tokenValue);
                return null;
            }

            return token;
        }

        public bool DeleteToken(string tokenValue)
        {
            return tokenRepository.DeleteByValue(tokenValue);
        }
    }
}