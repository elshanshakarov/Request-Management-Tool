using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto.Response
{
    public class RespRequestDto
    {
        public int Id { get; set; }
        public string RequestCreatorName { get; set; }
        public string Tittle { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string? RequestExecutorName { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
