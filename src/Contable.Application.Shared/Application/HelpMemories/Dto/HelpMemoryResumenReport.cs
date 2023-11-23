using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.HelpMemories.Dto
{
    public class HelpMemoryResumenReport : EntityDto
    {
        public string AnioEmision { get; set; }
        public string MesEmision { get; set; }
        public int CantidadAyudaMemoria { get; set; }
        public int CantidadCasosConflictivos { get; set; }
        public int CantidadConflictividadSocial { get; set; }
    }
}
