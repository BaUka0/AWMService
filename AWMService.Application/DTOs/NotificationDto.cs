using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.DTOs
{
    public sealed record NotificationDto
    (   string Type,               
        string Title,
        string Message,
        DateTimeOffset CreatedOn,
        object? Data = null
    );
}
