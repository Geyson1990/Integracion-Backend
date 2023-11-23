﻿using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table("AppSectorRoles")]
    public class SectorRole : FullAuditedEntity
    {
        [Column(TypeName = SectorConsts.NameType)]
        public string Name { get; set; }

        [Column(TypeName = SectorConsts.EnabledType)]
        public bool Enabled { get; set; }
    }
}
