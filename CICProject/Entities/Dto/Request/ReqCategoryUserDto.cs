using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto.Request
{
    public class ReqCategoryUserDto
    {
        public short CategoryId { get; set; }
        public bool CreatePermission { get; set; }
        public bool ExecutePermission { get; set; }
    }
}
