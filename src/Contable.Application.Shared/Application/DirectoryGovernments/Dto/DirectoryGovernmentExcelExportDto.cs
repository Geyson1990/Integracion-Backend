using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryGovernments.Dto
{
    public class DirectoryGovernmentExcelExportDto : EntityDto
    {
        public string Filter { get; set; }

        public string CaseCode { get; set; }
        public string CaseName { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string page_web { get; set; }
        public string guy { get; set; }
        public string sector { get; set; }
        public string enabled { get; set; }
        public string Code { get; set; }
        public bool checkName { get; set; }
        public bool checkShortName { get; set; }
        public bool checkAddress { get; set; }
        public bool checkPhoneNumber { get; set; }
        public bool checkUrl { get; set; }
        public bool checkTipo { get; set; }
        public bool checkSector { get; set; }
        public bool checkHabilitado { get; set; }
    }
}
