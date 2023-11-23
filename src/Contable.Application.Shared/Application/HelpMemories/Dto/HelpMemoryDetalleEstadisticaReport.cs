using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.HelpMemories.Dto
{
    public class HelpMemoryDetalleEstadisticaReport : EntityDto
    {
        public string FechaEmision { get; set; }
        public string AnioEmision { get; set; }
        public string MesEmision { get; set; }
        public string codigo { get; set; }
        public string entidadSolicitante { get; set; }
        public string personaSolicitante { get; set; }
        public string codigoConflicto { get; set; }
        public string nombreConflicto { get; set; }
        public string estadoConflicto { get; set; }
        public string unidadTerritorialConflicto { get; set; }
        public string tipologiaConflicto { get; set; }
        public string codigoEspacio { get; set; }
        public string denominacionEspacio { get; set; }
        public string tipoEspacio { get; set; }

    }
}
