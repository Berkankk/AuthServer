using Microsoft.AspNetCore.Http;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Respo
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        [JsonIgnore] //Serilaze ettirmeyiz bu şekilde , yoksayılacak bu
        public bool IsSuccessFull { get; private set; } //kendi iç yapımda kullanacağım bunu
        public ErrorDto ErrorDto { get; private set; }



        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessFull = true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessFull = true }; //Boş data döndük
        }

        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<T> { ErrorDto = errorDto, StatusCode = statusCode, IsSuccessFull = false };
        }
        public static Response<T> Fail(string errorMessage, bool isShow, int statusCode)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> { ErrorDto = errorDto, StatusCode = statusCode, IsSuccessFull = false };
        }
    }
}
