using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SectorMeets.Dto
{
    public class SectorMeetDetalleReport : EntityDto
    {
        public string FechaEmision { get; set; }
        public string AnioEmision { get; set; }
        public string MesEmision { get; set; }
        public string codigo { get; set; }
        public string nombreReunion { get; set; }
        public string unidadTerritorial { get; set; }
        public string codigoConflicto { get; set; }
        public string nombreConflicto { get; set; }
        public string estadoConflicto { get; set; }
        public string cantidadEspaciosDialogo { get; set; }
        public string codigoEspaciosDialogo { get; set; }
        public string unidadTerritorialConflicto { get; set; }
        public string tipologia { get; set; }
        public string tipologiaDetallada { get; set; }

    }
}
