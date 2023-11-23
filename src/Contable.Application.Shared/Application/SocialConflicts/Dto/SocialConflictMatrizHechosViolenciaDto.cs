using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SocialConflicts.Dto
{
    public class SocialConflictMatrizGestionesRealizadasDto : EntityDto
    {
        public string Code { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
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
