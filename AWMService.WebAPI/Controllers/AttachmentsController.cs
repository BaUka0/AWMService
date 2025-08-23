using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AWMService.WebAPI.Authorization;
using AWMService.Application.UseCases.Attachments.Queries.GetById;
using AWMService.Application.UseCases.Attachments.Queries.GetContent;
using AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForStudentWork;
using AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForWorkCheck;
using AWMService.Application.UseCases.Attachments.Commands.RenameAttachment;
using AWMService.Application.UseCases.Attachments.Commands.DeleteAttachment;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttachmentsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AttachmentsController> _logger;

        public AttachmentsController(IMediator mediator, ILogger<AttachmentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

      
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            _logger.LogInformation("GetById Attachment endpoint triggered for Id={Id}.", id);

            var result = await _mediator.Send(new GetAttachmentByIdQuery(id), ct);
            if (result.IsSuccess) return Ok(result.Value);

            _logger.LogWarning("GetById Attachment {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpGet("{id:int}/download")]

        public async Task<IActionResult> Download(int id, CancellationToken ct)
        {
            _logger.LogInformation("Download Attachment endpoint triggered for Id={Id}.", id);

            var result = await _mediator.Send(new DownloadAttachmentQuery(id), ct);
            if (result.IsSuccess)
            {
                var c = result.Value;
                var contentType = string.IsNullOrWhiteSpace(c.FileType) ? "application/octet-stream" : c.FileType;
                return File(c.Data, contentType, c.FileName);
            }

            _logger.LogWarning("Download Attachment {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }

   
        [HttpPost("student-works/{studentWorkId:int}/upload")]
        [HasPermission("manage_attachments")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadForStudentWork(
            int studentWorkId,
            [FromForm] AttachmentUploadForm form,
            CancellationToken ct)
        {
            _logger.LogInformation("UploadForStudentWork triggered for StudentWorkId={StudentWorkId}.", studentWorkId);

            if (form?.File is null || form.File.Length == 0)
                return BadRequest("Файл отсутствует или пустой.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized();

            byte[] data;
            await using (var ms = new MemoryStream())
            {
                await form.File.CopyToAsync(ms, ct);
                data = ms.ToArray();
            }

            var cmd = new UploadAttachmentForStudentWorkCommand
            {
                StudentWorkId = studentWorkId,
                FileName = form.File.FileName,
                FileType = string.IsNullOrWhiteSpace(form.File.ContentType) ? "application/octet-stream" : form.File.ContentType,
                FileData = data,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(cmd, ct);
            if (result.IsSuccess) return NoContent();

            _logger.LogWarning("UploadForStudentWork failed: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("work-checks/{workCheckId:int}/upload")]
        [HasPermission("manage_attachments")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadForWorkCheck(
            int workCheckId,
            [FromForm] AttachmentUploadForm form,
            CancellationToken ct)
        {
            _logger.LogInformation("UploadForWorkCheck triggered for WorkCheckId={WorkCheckId}.", workCheckId);

            if (form?.File is null || form.File.Length == 0)
                return BadRequest("Файл отсутствует или пустой.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized();

            byte[] data;
            await using (var ms = new MemoryStream())
            {
                await form.File.CopyToAsync(ms, ct);
                data = ms.ToArray();
            }

            var cmd = new UploadAttachmentForWorkCheckCommand
            {
                WorkCheckId = workCheckId,
                FileName = form.File.FileName,
                FileType = string.IsNullOrWhiteSpace(form.File.ContentType) ? "application/octet-stream" : form.File.ContentType,
                FileData = data,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(cmd, ct);
            if (result.IsSuccess) return NoContent();

            _logger.LogWarning("UploadForWorkCheck failed: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPatch("{id:int}/rename")]
        [HasPermission("manage_attachments")]
   
        public async Task<IActionResult> Rename(int id, [FromBody] RenameAttachmentRequest body, CancellationToken ct)
        {
            _logger.LogInformation("Rename Attachment endpoint triggered for Id={Id}.", id);

            if (body is null || string.IsNullOrWhiteSpace(body.NewFileName))
                return BadRequest("Новое имя файла не задано.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized();

            var cmd = new RenameAttachmentCommand
            {
                Id = id,
                NewFileName = body.NewFileName,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(cmd, ct);
            if (result.IsSuccess) return NoContent();

            _logger.LogWarning("Rename Attachment {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }


        [HttpDelete("{id:int}")]
        [HasPermission("manage_attachments")]

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            _logger.LogInformation("Delete Attachment endpoint triggered for Id={Id}.", id);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
                return Unauthorized();

            var cmd = new DeleteAttachmentCommand { Id = id, ActorUserId = actorUserId };
            var result = await _mediator.Send(cmd, ct);

            if (result.IsSuccess) return NoContent();

            _logger.LogWarning("Delete Attachment {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }
    }


    public sealed class AttachmentUploadForm
    {
        public IFormFile File { get; set; } = default!;

    }

    public sealed class RenameAttachmentRequest
    {
        public string NewFileName { get; set; } = default!;
    }
}
