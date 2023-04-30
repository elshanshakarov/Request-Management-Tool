using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class RequestByIdCommentDto
    {
        public List<CommentDto> CommentsList { get; set; }
        public string Comment { get; set; }
    }
}
