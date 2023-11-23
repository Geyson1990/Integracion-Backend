﻿using Abp.Application.Services.Dto;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Contable.Application.Parameters.Dto;
using System;
using System.Collections.Generic;

namespace Contable.Application.SocialConflictSensibles.Dto
{
    public class SocialConflictSensibleGetDto : EntityDto
    {
        public DateTime? LastModificationTime { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Problem { get; set; }
        public GeographycType GeographicType { get; set; }
        public string CaseNameVerificationState { get; set; }
        public bool CaseNameVerification { get; set; }
        public string ProblemVerificationState { get; set; }
        public bool ProblemVerification { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Published { get; set; }
        public ParameterDto Status { get; set; }
        public SocialConflictSensiblePersonDto Analyst { get; set; }
        public SocialConflictSensiblePersonDto Coordinator { get; set; }
        public SocialConflictSensiblePersonDto Manager { get; set; }
        public SocialConflictSensibleTypologyDto Typology { get; set; }
        public List<SocialConflictSensibleGeneralFactDto> GeneralFacts { get; set; }
        public List<SocialConflictSensibleActorLocationDto> Actors { get; set; }
        public List<SocialConflictSensibleRiskLocationDto> Risks { get; set; }
        public List<SocialConflictSensibleInterventionPlanDto> InterventionPlans { get; set; } 
        public List<SocialConflictSensibleCrisisCommitteeDto> CrisisCommittees { get; set; }
        public List<SocialConflictSensibleLocationDto> Locations { get; set; }
        public List<SocialConflictSensibleSugerenceDto> Sugerences { get; set; }
        public List<SocialConflictSensibleManagementLocationDto> Managements { get; set; }
        public List<SocialConflictSensibleStateDto> States { get; set; }
        public List<SocialConflictSensibleConditionDto> Conditions { get; set; }
        public List<SocialConflictSensibleResourceDto> Resources { get; set; }
        public List<SocialConflictSensibleNoteLocationDto> Notes { get; set; }
        public SocialConflictSensibleUserDto CreatorUser { get; set; }
        public SocialConflictSensibleUserDto EditUser { get; set; }
    }
}
