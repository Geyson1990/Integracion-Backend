using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static Contable.ContableDashboardCustomizationConsts.Widgets;
using System.Xml.Linq;
using System.Security.Policy;
using NUglify.JavaScript;

namespace Contable.Application.DirectoryDialogs.Dto
{
    public class DirectoryDialogExcelExportDto : EntityDto
    {
        public string name { get; set; }
        public string last_name { get; set; }
        public string mothers_last_name { get; set; }
        public string post { get; set; }
        public string entity { get; set; }
        public string web { get; set; }
        public string landline { get; set; }
        public string cell_phone { get; set; }
        public string enabled { get; set; }
        public bool checkName { get; set; }
        public bool checkLast_name { get; set; }
        public bool checkMothers_last_name { get; set; }
        public bool checkPost { get; set; }
        public bool checkEntity { get; set; }
        public bool checkWeb { get; set; }
        public bool checkLandline { get; set; }
        public bool checkCell_phone { get; set; }
        public bool checkEnabled { get; set; }
    }
}
