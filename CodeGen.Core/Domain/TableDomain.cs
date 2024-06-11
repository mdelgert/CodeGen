using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeGen.Core.Domain
{
    public class TableDomain
    {
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)]
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        [Required] 
        public DateTime CreatedOn { get; set; }
    }
}

#region Notes

#endregion
