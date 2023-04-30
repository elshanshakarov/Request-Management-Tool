using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class CommentDto
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
    }
}
