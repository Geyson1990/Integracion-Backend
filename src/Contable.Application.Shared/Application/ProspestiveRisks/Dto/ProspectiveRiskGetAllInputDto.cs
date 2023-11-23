using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.ProspestiveRisks.Dto
{
    public class ProspectiveRiskGetAllInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public int Id { get; set; }
        public string Filter { get; set; }
        public int InstitutionId { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Department.Name ASC, Province.Name ASC";
            }
        }
    }
}
