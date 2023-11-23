using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryGovernmentTypes.Dto
{
    public class DirectoryGovernmentTypeExcelExportDto : EntityDto
    {
        public string Name { get; set; }
        public string index { get; set; }
        public string enabled { get; set; }

        public bool checkName { get; set; }
        public bool checkIndex { get; set; }
        public bool checkEnabled { get; set; }
    }
}
