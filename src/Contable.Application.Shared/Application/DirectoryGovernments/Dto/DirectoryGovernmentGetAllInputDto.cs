using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryGovernments.Dto
{
    public class DirectoryGovernmentGetAllInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Name ASC";
            }
        }

        public bool CheckName { get; set; }
        public bool CheckShortName { get; set; }
        public bool CheckAddress { get; set; }
        public bool CheckPhoneNumber { get; set; }
        public bool CheckUrl { get; set; }
        public bool CheckTipo { get; set; }
        public bool CheckSector { get; set; }
        public bool CheckHabilitado { get; set; }
    }
}
