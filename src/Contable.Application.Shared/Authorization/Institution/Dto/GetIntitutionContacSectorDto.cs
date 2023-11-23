using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Authorization.Institution.Dto
{
    public class GetIntitutionContacSectorDto: EntityDto
    {
        public string Name { get; set; }
        public string Cargo { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool Remove { get; set; }
    }
}
