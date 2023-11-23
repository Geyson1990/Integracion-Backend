﻿using Abp.Application.Services.Dto;
using Contable.Application.SectorMeetSessions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SectorMeets.Dto
{
    public class SectorMeetGetDto : EntityDto
    {
        public string Code { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public string MeetName { get; set; }
        public SectorMeetTerritorialUnitRelationDto TerritorialUnit { get; set; }
        public SectorMeetSocialConflictRelationDto SocialConflict { get; set; }
        public List<SectorMeetSessionRiskFactorsRelationDto> RiskFactors { get; set; }
        public List<SectorMeetResourceRelationDto> Resources { get; set; }

        public int? Modality { get; set; }
        public int? MeetType { get; set; }
        public int? RiskLevel { get; set; }
        public string Object { get; set; }
        public int? RolId { get; set; }
        public int? State { get; set; }
        public string ResponsibleName { get; set; }

        public SectorMeetGetDto()
        {
            Resources = new List<SectorMeetResourceRelationDto>();
        }
    }
}
