using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflicts.Dto
{
    public class SocialConflictGetAllDto : EntityDto
    {
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public int ActorsCount { get; set; }
        public int GeneralFactsCount { get; set; }
        public int SugerencesCount { get; set; }
        public int AcceptedSugerencesCount { get; set; }
        public int PendingSugerencesCount { get; set; }
        public int CompromiseCount { get; set; }
        public int CompromiseComplimentCount { get; set; }
        public int ManagementsCount { get; set; }
        public int NotesCount { get; set; }
        public int StatesCount { get; set; }
        public int ViolenceFactsCount { get; set; }
        public int ResourcesCount { get; set; }
        public bool Generation { get; set; }
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
        public string Dialog { get; set; }
        public string TerritorialUnits { get; set; }
        public ConflictVerification Verification { get; set; }
        public bool CaseNameVerification { get; set; }
        public bool DescriptionVerification { get; set; }
        public bool ProblemVerification { get; set; }
        public bool RiskVerification { get; set; }
        public bool ManagementVerification { get; set; }
        public bool StateVerification { get; set; }
        public bool ConditionVerification { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Published { get; set; }
        public int Type { get; set; }
        public List<SocialConflictLocationDto> Locations { get; set; }
        public SocialConflictUserDto CreatorUser { get; set; }
        public SocialConflictUserDto EditUser { get; set; }
        public List<SocialConflictActorLocationDto> Actors { get; set; }
        public List<SocialConflictGeneralFactDto> GeneralFacts { get; set; }
        public List<SocialConflictSugerenceDto> Sugerences { get; set; }
        public List<SocialConflictCompromiseGetAllDto> Compromises { get; set; }
        public List<SocialConflictManagementLocationDto> Managements { get; set; }
        public List<SocialConflictStateDto> States { get; set; }
        public List<SocialConflictViolenceFactDto> ViolenceFacts { get; set; }
        public List<SocialConflictNoteLocationDto> Notes { get; set; }
        public List<SocialConflictResourceDto> Resources { get; set; }
    }
}
