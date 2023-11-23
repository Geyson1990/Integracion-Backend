using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.ProspestiveRisks.Dto
{
    public class ProspectiveRiskDetailDto : EntityDto
    {
        public int StaticVariableOptionId { get; set; }
        public string NameVariable { get; set; }
        public StaticVariableType TypeVariableStatic { get; set; }
        public string TypeVariable { get; set; }
        public decimal Value { get; set; }
    }
}
