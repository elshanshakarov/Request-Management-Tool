using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class RequestByIdDto
    {
        public string Status { get; set; }
        public RequestByIdRequestDto RequestByIdRequestDto { get; set; }
        public RequestByIdCommentDto RequestByIdCommentDto { get; set; }
        public RequestByIdHistoryDto RequestByIdHistoryDto { get; set; } = null;
        public RequestByIdRequestInfoDto RequestByIdRequestInfoDto { get; set; } = null;

    }
}
