﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CronConfigure.ViewModels
{
    public class CreateRecurringJobModel
    {
        [Required]
        public string id_repository { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha { get; set; }
        public string set { get; set; }
        [Required]
        public string nombre_job { get; set; }
        [Required]
        public string cron_expression { get; set; }
    }
}