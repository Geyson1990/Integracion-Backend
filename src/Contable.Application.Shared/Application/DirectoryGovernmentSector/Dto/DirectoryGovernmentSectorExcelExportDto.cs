using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryGovernmentSectors.Dto
{
    public class DirectoryGovernmentSectorExcelExportDto : EntityDto
    {
        public string Name { get; set; }
        public string enabled { get; set; }

        public bool checkName { get; set; }
        public bool checkEnabled { get; set; }
    }
}
