using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Org.BouncyCastle.Asn1.Crmf;

namespace Wwbweibo.CrackDetect.Libs.MySql.Entity
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DataTotalCount { get; set; }
        public int CrackTotalCount { get; set; }

        [NotMapped]
        public List<TaskItem> Items { get; set; }
    }
}
