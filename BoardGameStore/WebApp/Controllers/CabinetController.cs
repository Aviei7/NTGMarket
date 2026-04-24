using Application.DTO.Input;
using Application.DTO.Output;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Cabinet;
using Microsoft.AspNetCore.Authorization;
using Application.DTO.Output.Users;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CabinetController : ControllerBase
    {

        private readonly ICabinetServices _cabinetServices;

        public CabinetController(ICabinetServices cabinetServices)
        {
            _cabinetServices = cabinetServices;
        }


        //[HttpGet("InfoCabinetByUser/{email}")]
        //public async Task<ActionResult<UserInfoDTO>> GetInfoCabinetByUser(string email, CancellationToken ct)
        //{
        //    var get = await _cabinetServices.GetInfoByUser(email);

        //    return Ok(get);
        //}

        //[HttpPost("InfoOrdersByUser")]
        //public async Task<ActionResult<PageResultDto<ListOrdersDTO>>> PostInfoOrdersByUser( CancellationToken ct)
        //{

        //}
    }
}
