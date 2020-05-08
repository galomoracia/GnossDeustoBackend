﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_CARGA.Models.Entities
{
    [Table("Sincronizacion_Repositorio")]
    public class RepositorySync
    {
        public Guid RepositoryId { get; set; }
        public string Set { get; set; }
        public DateTime UltimaFechaDeSincronizacion{ get; set; }
    }
}
