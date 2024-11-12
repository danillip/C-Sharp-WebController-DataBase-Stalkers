using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DataBaze.Stalker.Models
{
    public class ArtifactAnomaly
    {
        public int ArtifactAnomalyId { get; set; } //PK

        public int ArtifactId { get; set; } //FK
        public Artifact Artifact { get; set; } // Навигационное свойство для связи с Artifact

        public int AnomalyId { get; set; } //FK
        public Anomaly Anomaly { get; set; } // Навигационное свойство для связи с Anomaly

        public DateTime AppearanceDate { get; set; }

    }
}

//namespace StalkerDb.Models
//{
//    public class ArtifactAnomaly
//    {
//        public int ArtifactAnomalyId { get; set; }  // PK
//        public int ArtifactId { get; set; } // FK
//        public int AnomalyId { get; set; } // FK
//        public DateTime AppearanceDate { get; set; }

//        public Artifact? Artifact { get; set; }
//        public Anomaly? Anomaly { get; set; }
//    }
//}
