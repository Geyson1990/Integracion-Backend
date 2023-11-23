using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application.SocialConflictSensibles.Dto
{
    public class SocialConflictSensibleSituacionesConditionsExportDto : EntityDto
    {
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Problem { get; set; }
        public string Filter { get; set; }
        public string DescriptionCondition { get; set; }
        public string CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
