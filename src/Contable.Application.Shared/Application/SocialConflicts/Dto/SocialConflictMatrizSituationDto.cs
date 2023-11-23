using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflicts.Dto
{
    public class SocialConflictMatrizSituationDto : EntityDto
    {
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Problem { get; set; }
        public string Dialog { get; set; }
        public string DescriptionSituation { get; set; }
        public string StateSituation { get; set; }
        public string StateManager { get; set; }
        public string CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
