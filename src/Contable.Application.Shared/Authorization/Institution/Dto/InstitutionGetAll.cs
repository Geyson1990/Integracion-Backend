using Abp.Application.Services.Dto;
using Contable.Authorization.Institution.InstitutionDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Authorization.Institution.Dto
{
    public class InstitutionGetAll:EntityDto
    {
        public List<GetInstitutionDto> Institutions { get; set; } 
    }
}
