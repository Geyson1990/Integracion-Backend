using Abp.Application.Services.Dto;
using Contable.Application;

using Contable.Authorization.Institution.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Contable.Application.Sectors.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contable.Authorization.Institution.InstitutionDto
{
    public class GetInstitutionDto: EntityDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string ruc { get; set; }
        public string contacName { get; set; }
        public string EmailAddress { get; set; }      
        public string PhoneNumber { get; set; }
        public string tokent { get; set; }

        public SectorGetDto sector { get; set; }

    }
}
