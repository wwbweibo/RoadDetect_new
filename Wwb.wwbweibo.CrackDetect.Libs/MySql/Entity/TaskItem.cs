using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Org.BouncyCastle.Asn1.Crmf;

namespace Wwbweibo.CrackDetect.Libs.MySql.Entity
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }
        public String Data { get; set; }
        public int Area { get; set; }
        public String MarkedData { get; set; }
        public String IsCrack { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid MajorTaskId { get; set; }
        [ForeignKey("MajorTaskId")]
        public Task Task { get; set; }
    }
}
