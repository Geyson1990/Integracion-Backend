using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryDialogs.Dto
{
    public class DirectoryDialogGetAllInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Name ASC";
            }
        }
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
