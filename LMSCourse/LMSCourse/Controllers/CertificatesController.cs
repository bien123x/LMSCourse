using LMSCourse.DTOs.Certificate;
using LMSCourse.Services;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;
using System.Threading.Tasks;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateService _certService;
        public CertificatesController(ICertificateService certService)
        {
            _certService = certService;
        }

        [HttpPost("generate-certificate")]
        public async Task<IActionResult> GenerateCertificate([FromBody] CertificateDto dto)
        {
            try
            {
                await _certService.CreateCertificate(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }

        [HttpGet("by-user/{userId:int}")]
        [Authorize(Policy = PERMISSION.Students.Certificates.Module)]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var certificateView = await _certService.GetCertificatesByUserId(userId);
            return Ok(certificateView);
        }
    }
}
