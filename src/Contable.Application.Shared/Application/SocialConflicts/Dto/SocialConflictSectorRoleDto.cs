using System;
using Abp.Application.Services.Dto;
using Contable.Application.SectorRoles.Dto;
using Contable.Application.Sectors.Dto;
using Newtonsoft.Json;

namespace Contable.Application.SocialConflicts.Dto
{
    public class SocialConflictSectorRoleDto : EntityDto
    {

        public int? SocialConflictId { get; set; }
        public SectorGetDto Sector { get; set; }
        public SectorRoleGetDto SectorRole { get; set; }
        public int? governmentLevel { get; set; }
        public bool Remove { get; set; }
    }
}
