using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace UserHubAPI.Entities
{
    public class Base
    {
        /// <summary>
        /// to show field only in GET Method of swagger
        /// </summary>
        /// <value>[SwaggerSchema(ReadOnly = true)]</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue(1)]
        public int Autokey { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public String? CreatedBy { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public String? ModifiedBy { get; set; }

        public Boolean IsActive { get; set; }

        public int RecordStatus { get; set; }
    }
}