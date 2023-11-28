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
        Patch,
        Head,
        Error
    }
    internal class Utility
    {
        public static Method GetMethod(string method)
        {
            method = method.ToLower();
            return method switch
            {
                "put" => Method.Put,
                "post" => Method.Post,
                "get" => Method.Get,
                "patch" => Method.Patch,
                "head" => Method.Head,
                "delete" => Method.Delete,
                _ => Method.Error
            };
        }
    }
}
