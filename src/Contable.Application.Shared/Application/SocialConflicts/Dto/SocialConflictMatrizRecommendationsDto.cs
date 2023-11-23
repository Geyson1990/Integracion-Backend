using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflicts.Dto
{
    public class SocialConflictMatrizRecommendationsDto : EntityDto
    {
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
        public string Problem { get; set; }
        public string Dialog { get; set; }
        public string Recommendations { get; set; }
        public string CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
