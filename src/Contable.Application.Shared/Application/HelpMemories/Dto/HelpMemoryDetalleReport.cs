using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.HelpMemories.Dto
{
    public class HelpMemoryDetalleReport : EntityDto
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
        public string cantidadEspaciosDialogo { get; set; }
        public string codigoEspaciosDialogo { get; set; }
        public string espaciosDialogo { get; set; }
        public string unidadTerritorialConflicto { get; set; }
        public string tipologia { get; set; }
        public string tipologiaDetallada { get; set; }
        public string codigoSensible { get; set; }
        public string nombreSensible { get; set; }
        public string estadoSensible { get; set; }
        public string unidadTerritorialSensible { get; set; }
        public string tipologiaSensible { get; set; }


    }
}
