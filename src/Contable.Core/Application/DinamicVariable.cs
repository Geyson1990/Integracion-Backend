﻿using Abp.Domain.Entities.Auditing;
using Contable.Authorization.Institution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application
{
    [Table("AppDinamicVariables")]
    public class DinamicVariable : FullAuditedEntity
    {
        [Column(TypeName = DinamicVariableConsts.NameType)]
        public string Name { get; set; }

        [Column(TypeName = DinamicVariableConsts.EnabledType)]
        public bool Enabled { get; set; }

        [Column(TypeName = DinamicVariableConsts.Type)]
        public DinamicVariableType Type { get; set; }

        [Column(TypeName = DinamicVariableConsts.InstitutionIdType)]
        [ForeignKey("Institutions")]
        public int? InstitutionId { get; set; }
        public Institutions Institution { get; set; }

        public List<DinamicVariableDetail> Details { get; set; }

        public List<StaticVariableOption> StaticVariableOptions { get; set; }
    }
}
