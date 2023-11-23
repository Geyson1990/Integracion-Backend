using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table("AppSocialConflictAlertSources")]
    public class SocialConflictAlertSources: FullAuditedEntity
    {
        [Column(TypeName = SocialConflictAlertSourcesConsts.SocialConflictAlertIdType)]
        [ForeignKey("SocialConflictAlert")]
        public int SocialConflictAlertId { get; set; }
        public SocialConflictAlert SocialConflictAlert { get; set; }

        [Column(TypeName = SocialConflictAlertSourcesConsts.SourceType)]
        public string Source { get; set; }

        [Column(TypeName = SocialConflictAlertSourcesConsts.SourceTypeType)]
        public string SourceType { get; set; }

        [Column(TypeName = SocialConflictAlertSourcesConsts.LinkType)]
        public string Link{ get; set; }
    }
}
