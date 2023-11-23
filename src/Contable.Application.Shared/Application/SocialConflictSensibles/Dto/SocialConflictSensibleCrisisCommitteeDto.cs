using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflictSensibles.Dto
{
    public class SocialConflictSensibleCrisisCommitteeDto: EntityDto
    {
        
        public DateTime CrisisComiteStartTime { get; set; }
        public DateTime CrisisComiteEndTime { get; set; }
        public string CaseName { get; set; }
        
    }
}
