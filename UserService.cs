using HttpServerDemo.Data;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;



namespace HttpServerDemo
{
    internal class UserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository repo)
        {
            userRepository = repo;
        }

        public bool Register(User user)
        {
            User? existingUser = userRepository.GetByUsername(user.Username);

            if (existingUser != null)
                return false;

            user.Password = HashPassword(user.Password);

            return userRepository.Create(user);
        }

        public User? Login(User user)
        {
            User? userInDatabase = userRepository.GetByUsername(user.Username);

            if (userInDatabase == null)
                return null;

            bool passwordCorrect = VerifyPassword(
                user.Password,
                userInDatabase.Password
            );

            if (passwordCorrect)
                return userInDatabase;

            return null;
        }

        private string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32
            );

            return Convert.ToBase64String(salt) + "." +
                   Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            string[] parts = storedPassword.Split('.');

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            byte[] enteredHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32
            );

            return CryptographicOperations.FixedTimeEquals(
                enteredHash,
                storedHash
            );
        }
    }
}