using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wwbweibo.CrackDetect.Libs.MySql.Entity
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }
        [Column("data")]
        public String Data { get; set; }
        [Column("area")]
        public double Area { get; set; }
        [Column("marked_data")]
        public String MarkedData { get; set; }
        [Column("is_crack")]
        public bool IsCrack { get; set; }
        [Column("longitude")]
        public double Longitude { get; set; }
        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("major_task_id")]
        public Guid MajorTaskId { get; set; }
        [ForeignKey("MajorTaskId")]
        public Task Task { get; set; }
    }
}
