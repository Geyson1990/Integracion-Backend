using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table("AppSectorContacFocal")]
    public class SectorContacFocal:FullAuditedEntity
    {
        [Column(TypeName = SectorContacFocalConsts.CommitteeCrisisIdType)]
        [ForeignKey("CrisisCommittees")]
        public int CrisisCommitteeId { get; set; }
        public CrisisCommittee CrisisCommittee { get; set; }

        [Column(TypeName = SectorContacFocalConsts.NameType)]
        public string Name { get; set; }

        [Column(TypeName = SectorContacFocalConsts.CargoType)]
        public string Cargo { get; set; }

        [Column(TypeName = SectorContacFocalConsts.EmailAddressType)]
        public string EmailAddress { get; set; }

        [Column(TypeName = SectorContacFocalConsts.PhoneNumberType)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = SectorContacFocalConsts.IndexType)]
        public int Index { get; set; }

    }
}
