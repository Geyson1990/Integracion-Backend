using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.CrisisCommittees.Dto
{
    public class CrisisCommitteeSectorRelationDto : EntityDto
    {
        public CrisisCommitteeDirectoryGovernmentRelationDto DirectoryGovernment { get; set; }
        public CrisisCommitteeSectorContacFocalDto sectorContacFocal { get; set; } 
        public List<CrisisCommitteeDirectoryGovernmentRelationDto> DirectoryGovernments { get; set; }
        public bool Remove { get; set; }
    }
}
