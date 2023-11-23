using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Contable.Authorization.Institution.InstitutionDto;

namespace Contable.Application.Tag.Dto
{
    public class TagsGetDto: EntityDto
    {
        public int? id {  get; set; }
        public string name { get; set; }
        public int institutionId  { get; set; }
        public GetInstitutionDto Institution { get; set; }


    }
}
