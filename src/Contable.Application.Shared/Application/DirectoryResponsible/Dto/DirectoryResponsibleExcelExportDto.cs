using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryResponsibles.Dto
{
    public class DirectoryResponsibleExcelExportDto : EntityDto
    {
        public string Name { get; set; }
        public string enabled { get; set; }

        public bool checkName { get; set; }
        public bool checkEnabled { get; set; }
    }
}
