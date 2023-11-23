using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application.SocialConflictSensibles.Dto
{
    public class SocialConflictSensibleSituacionesRecomendacionesExportDto : EntityDto
    {

        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Filter { get; set; }
        public string Problem { get; set; }
        public string Dialog { get; set; }
        public string GestionDescription { get; set; }
        public string NroCivilMen { get; set; }
        public string NroCivilWoMen { get; set; }
        public string NroStateMen { get; set; }
        public string NroStateWoMen { get; set; }
        public string NroCompanyMen { get; set; }
        public string NroCompanyWoMen { get; set; }
        public string CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }


    }
}
