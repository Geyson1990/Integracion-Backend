using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.SectorMeets.Dto
{
    public class SectorMeetResumenReport : EntityDto
    {
        public string AnioEmision { get; set; }
        public string MesEmision { get; set; }
        public int CantidadSectorMeet { get; set; }
        public int CantidadCasosConflictivos { get; set; }
        public int CantidadPreCaso { get; set; }
    }
}
