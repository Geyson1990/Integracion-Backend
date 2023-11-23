using Abp.Application.Services.Dto;
using Contable.Application.Sectors.Dto;
using Contable.Authorization.Institution.InstitutionDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Authorization.Institution.Dto
{
    public class GetCreateOrUpdateInstitutionDto:EntityDto
    {
        public GetInstitutionDto  institution { get; set; }
        public List<SectorGetDto> sectors { get; set; }


    }
}
