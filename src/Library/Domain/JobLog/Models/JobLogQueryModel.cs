using System;
using System.ComponentModel.DataAnnotations;
using NetModular.Lib.Data.Query;

namespace NetModular.Module.Quartz.Domain.JobLog.Models
{
    public class JobLogQueryModel : QueryModel
    {
        /// <summary>
        /// ������
        /// </summary>
        [Required(ErrorMessage = "��ѡ������")]
        public Guid JobId { get; set; }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
