using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI.Entities
{
    public class Album
    {
        public int albumId { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
