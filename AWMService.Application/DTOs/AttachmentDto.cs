using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.DTOs
{
    public sealed record AttachmentDto(
        int Id,
        string FileName,
        string FileType,
        long FileSize,
        int? StudentWorkId,
        int? WorkCheckId,
        int UploadedBy,
        DateTime UploadedOn,
        int? ModifiedBy,
        DateTime? ModifiedOn);
}
