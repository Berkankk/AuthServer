using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniAPP1.API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        //[Authorize(Policy="AgePolicy")]
        [Authorize(Roles = "admin",Policy ="AnkaraPolicy")] //Buraya sadece admin rolünde olanlar erişebilir
        [HttpGet]
        public IActionResult GetStock()
        {
            var userName = HttpContext.User.Identity.Name; //veritabanın da istediğim kullanıcıya ait bilgileri alabilirim bu şekilde

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            //veri tabanında userId veya userName alanları üzerinden gerekli datayı çek dedik

            return Ok($"Stok İşlemleri =>UserName:{userName}-UserId:{userId}");


        }
    }
}
