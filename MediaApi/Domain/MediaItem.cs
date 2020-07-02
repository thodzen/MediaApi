using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.Domain
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string Title{ get; set; }
        public string Kind { get; set; }
        public string RecommendedBy { get; set; }
        public bool Consumed { get; set; }
        public DateTime? DateConsumed { get; set; } // "?" allows the struct to be nullable (same as writing "Nullable<DateTime>"
        public bool Removed { get; set; }
    }
}
