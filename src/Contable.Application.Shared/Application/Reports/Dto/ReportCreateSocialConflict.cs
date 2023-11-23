using System;
using Abp.Application.Services.Dto;

namespace Contable.Application.Reports.Dto
{
    public class ReportCreateSocialConflict : EntityDto
    {
        public ReportType Type { get; set; }
        public bool bolNameCase { get; set; }
        public bool bolLocation { get; set; }
        public bool bolBackground { get; set; }
        public bool bolDemand { get; set; }
        public bool bolAccions { get; set; }
        public bool bolCurrentSituation { get; set; }
        public bool bolRecommendations { get; set; }
        public bool bolCommitments { get; set; }
        public bool bolSectors { get; set; }
        public bool bolRiskLevels { get; set; }
        public bool bolMeetings { get; set; }
    }
}
