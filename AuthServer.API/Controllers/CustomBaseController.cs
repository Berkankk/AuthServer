using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Respo;

namespace AuthServer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
       //Burası diğer controller da kod tekrarının önüne geçmek için oluşturduğumuz base controllerdır 
       

        public IActionResult ActionResultInstance<T>(Response<T> response)where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode, //uygun olan response ı dönecek yani 200 400 404 401 gibi...
            };
        }


    }
}
