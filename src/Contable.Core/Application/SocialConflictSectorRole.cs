using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{

    [Table("AppSocialConflictSectorRoles")]
    public class SocialConflictSectorRole : FullAuditedEntity
    {
        [Column(TypeName = SocialConflictSectorRolesConsts.integerType)]
        [ForeignKey("SocialConflict")]
        public int? SocialConflictId { get; set; }
        public SocialConflict SocialConflict { get; set; }


        [Column(TypeName = SocialConflictSectorRolesConsts.integerType)]
        [ForeignKey("SectorRole")]
        public int? SectorRolesId { get; set; }
        public SectorRole SectorRole { get; set; }


        [Column(TypeName = SocialConflictSectorRolesConsts.integerType)]
        public int? SectorId { get; set; }
        public Sector Sector { get; set; }


        [Column(TypeName = SocialConflictSectorRolesConsts.integerType)]
        public int? GovernmentLevel { get; set; }

    }
}
