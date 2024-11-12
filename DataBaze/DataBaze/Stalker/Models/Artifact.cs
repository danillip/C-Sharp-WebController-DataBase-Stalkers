using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    public class Artifact
    {
        public int ArtifactId { get; set; } //PK
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }

    }
}

//namespace StalkerDb.Models
//{
//    public class Artifact
//    {
//        public int ArtifactId { get; set; }  // PK
//        public string? Name { get; set; }
//        public decimal Cost { get; set; }
//        public string Description { get; set; }

//        public ICollection<ArtifactAnomaly>? ArtifactAnomalies { get; set; } // Связь с аномалиями
//    }
//}
