using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server
{
    public enum Status
    {
        InternalServerError = 500,
        NotImplemented = 501,
        HttpVersionNotSupported = 505,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        Conflict = 409,
        Ok = 200,
        Created = 201,
        NoContent = 204
    }
    public enum Method
    {
        Get,
        Post,
        Put,
        Delete,
        Error
    }
    public class Utility
    {
        public Method GetMethod(string method)
        {
            switch (method.ToLower())
            {
                case "put": return Method.Put;
                case "post": return Method.Post;
                case "get": return Method.Get;
                case "delete": return Method.Delete;
                default: return Method.Error;
            }
        }
        public string ExtractTokenFromString(string authorizationHeader)
        {
            string[] parts = authorizationHeader.Split(' ');
            if (parts.Length != 2)
            {
                //thought the curl script using "Bearer username-mtcgToken" would mean that "bearer is also in the string however it is not so i do not need this function
                //since i would have to rewrite a lot of methods a bit to fix this i will simply return the whole string here
                return authorizationHeader;
                //throw new ArgumentOutOfRangeException();
            }
            return parts[1];
        }
        public string ExtractUsernameFromToken(string token)
        {
            int tokenIndex = token.IndexOf('-');
            return token[..tokenIndex];
        }
    }
}
