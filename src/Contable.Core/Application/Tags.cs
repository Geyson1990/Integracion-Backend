using Abp.Domain.Entities.Auditing;
using Contable.Authorization.Institution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table ("AppTags")]
    public class Tags :FullAuditedEntity
    {
        [Column(TypeName = TagsConsts.InstitutionIdType)]
        [ForeignKey("Institutions")]
        public int InstitutionId { get; set; }
        public Institutions Institution { get; set; }

        [Column(TypeName = TagsConsts.NameType)]
        public string Name { get; set; }


    }
}
