using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.ProspestiveRisks.Dto
{
    public class ProspectiveRiskDto : NullableIdDto
    {

        public int Id  {get;set;}
        public DateTime? CreationTime { get; set; }
        public DateTime? EvaluatedTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public decimal FixRate { get; set; }
        public decimal Value { get; set; }
        public List<ProspectiveRiskDetailDto> Details { get; set; }
        public List<ProspectiveRiskTerritorialUnitDto> TerritorialUnits { get; set; }
    }
}
