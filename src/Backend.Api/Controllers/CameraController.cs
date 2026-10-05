using Backend.Api.Attributes;
using Backend.Application.Common;
using Backend.Application.Interfaces;
using Backend.Infrustructure.Helpers;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Backend.Api.Controllers
{
    [Route("api/camera")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMediaService _mediaService;

        public CameraController(IMediator mediator, IMediaService mediaService)
        {
            _mediator = mediator;
            _mediaService = mediaService;
        }

        [HttpPost("add-rtsp-camera/")]
        public async Task<IActionResult> AddCameraAsync([FromBody] AddCameraDto dto)
        {
            var result = await _mediator.Send(new CreateCameraCommand(dto.FacilityId, dto.Name, dto.Url, dto.CameraSourceType, dto.Type));
            return Ok(result);
        }

        [HttpPost("add-file-as-camera/")]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> AddFilesAsCameraAsync()
        {

            MediaUploadResult mediaUploadResult = null;
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest("Request is not multipart.");
            }

            try
            {
                mediaUploadResult = await _mediaService.UploadAsync(Request.Body, Request.ContentType);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(400, ex.Message);
            }

            var result = await _mediator.Send(new CreateCameraCommand(
                mediaUploadResult.FacilityId,
                mediaUploadResult.Name,
                mediaUploadResult.TempStreamUrl,
                CameraSourceType.File,
                mediaUploadResult.CameraType));

            return Ok(result);
        }


        [HttpGet("all/")]
        public async Task<IActionResult> GetCamerasAsync()
        {
            var result = await _mediator.Send(new GetCamerasQuery());
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCameraAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteCameraCommand(id));
            return Ok("Deleted.");
        }



        [HttpPost("start/{id}")]
        public async Task<IActionResult> StartCameraAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new StartProcessCommand(id));
            return Ok(result);
        }

        [HttpPost("stop/{id}")]
        public async Task<IActionResult> StopCameraAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new StopProcessCommand(id));
            return Ok("Stopped");
        }

        [HttpGet("get-running-process/")]
        public async Task<IActionResult> GetRunningProcessAsync()
        {
            var result = await _mediator.Send(new GetRunningCamerasQuery());
            return Ok(result);
        }


    }
}
