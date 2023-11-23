using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflictSensibles.Dto
{
    public class SocialConflictSensibleInterventionPlanDto:EntityDto
    {
        
        public string CaseName { get; set; }
        public DateTime InterventionPlanTime { get; set; }
    }
}
