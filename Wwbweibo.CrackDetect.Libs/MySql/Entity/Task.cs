using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Wwbweibo.CrackDetect.Libs.MySql.Entity
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        [Column("create_time")]
        public DateTime CreateTime { get; set; }
        [Column("end_time")]
        public DateTime EndTime { get; set; }
        [Column("data_total_count")]
        public int DataTotalCount { get; set; }
        [Column("crack_total_count")]
        public int CrackTotalCount { get; set; }

    }
}
