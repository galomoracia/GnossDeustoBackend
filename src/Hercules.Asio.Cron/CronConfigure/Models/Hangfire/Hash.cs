// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto H�rcules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Clase usada por Hangfire para el guardado de las tareas
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CronConfigure.Models.Hangfire
{    
    ///<summary>
    ///Clase usada por Hangfire para el guardado de las tareas
    ///</summary>
    [Table("hash", Schema = "hangfire")]
    [ExcludeFromCodeCoverage]
    public partial class Hash
    {
        /// <summary>
        /// Key
        /// </summary>
        [Column(Order = 0)]
        [StringLength(100)]
        public string Key { get; set; }

        /// <summary>
        /// Field
        /// </summary>
        [Column(Order = 1)]
        [StringLength(100)]
        public string Field { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// ExpireAt
        /// </summary>
        public DateTime? ExpireAt { get; set; }
    }
}
