using Abp.Domain.Entities.Auditing;
using Castle.MicroKernel.SubSystems.Conversion;
using Contable.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Authorization.Institution
{
    [Table("AppInstitutions")]
    public class Institutions : FullAuditedEntity
    {

        [Column(TypeName = InstitutionConst.SectorIdType)]
        [ForeignKey("Sector")]
        public int? SectorId { get; set; }
        public Sector Sector { get; set; }

        [Column(TypeName = InstitutionConst.NameType)]
        public string Name { get; set; }

        [Column(TypeName = InstitutionConst.ShortNameType)]
        public string ShortName { get; set; }

        [Column(TypeName = InstitutionConst.RucType)]
        public string Ruc { get; set; }

        [Column(TypeName = InstitutionConst.TokentType)]
        public string Tokent { get; set; }

        [Column(TypeName = InstitutionConst.ContacNameType)]
        public string ContacName { get; set; }

        [Column(TypeName = InstitutionConst.EmailAddressType)]
        public string EmailAddress { get; set; }

        [Column(TypeName = InstitutionConst.PhoneNumberType)]
        public string PhoneNumber { get; set; }


    }
}
