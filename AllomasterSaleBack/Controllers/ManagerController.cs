using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AlloMasterSale.Services;
using System;
using System.Threading.Tasks;

namespace AlloMasterSale.Controllers
{
    [ApiController]
    [Route("api/manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public ManagerController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet("requests")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                Console.WriteLine("Получен запрос на получение списка заявок");
                var requests = await _requestService.GetAllRequestsAsync();
                Console.WriteLine($"Успешно получено {requests.Count} заявок");
                return Ok(requests);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении заявок: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Произошла ошибка при получении заявок", details = ex.Message });
            }
        }

        [HttpPost("approve/{requestId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            try
            {
                Console.WriteLine($"Получен запрос на одобрение заявки с Id={requestId}");
                await _requestService.ApproveRequestAsync(requestId);
                Console.WriteLine("Заявка успешно одобрена");
                return Ok(new { message = "Заявка одобрена, подписка активирована" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при одобрении заявки: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reject/{requestId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            try
            {
                Console.WriteLine($"Получен запрос на отклонение заявки с Id={requestId}");
                await _requestService.RejectRequestAsync(requestId);
                Console.WriteLine("Заявка успешно отклонена");
                return Ok(new { message = "Заявка отклонена" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отклонении заявки: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}