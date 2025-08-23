using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.DTOs
{
    public sealed record AttachmentContentDto(
        byte[] Data,
        string FileName,
        string FileType
        );
}
