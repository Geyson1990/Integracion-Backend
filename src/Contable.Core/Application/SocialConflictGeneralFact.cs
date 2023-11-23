using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table("AppSocialConflictGeneralFacts")]
    public class SocialConflictGeneralFact : FullAuditedEntity
    {
        [Column(TypeName = SocialConflictGeneralFactConsts.SocialConflictIdType)]
        [ForeignKey("SocialConflict")]
        public int SocialConflictId { get; set; }
        public SocialConflict SocialConflict { get; set; }

        [Column(TypeName = SocialConflictGeneralFactConsts.DescriptionType)]
        public string Description { get; set; }

        [Column(TypeName = SocialConflictGeneralFactConsts.TagsIdType)]
        [ForeignKey("Tags")]
        public int? TagId { get; set; }
        public Tags Tag { get; set; }

        [Column(TypeName = SocialConflictGeneralFactConsts.FactTimeType)]
        public DateTime FactTime { get; set; }
    }
}
