using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Contable.Application.InterventionPlans.Dto;
using Contable.Application.Tag.Dto;
using Contable.Authorization.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Application.Tag
{
    public interface ITagsAppServices : IApplicationService
    {
        Task<PagedResultDto<TagsGetDto>> GetTagsforInstitution(NullableIdDto input);

        Task<TagsGetDto> GetTagForEdit(NullableIdDto<long> input);

        Task CreateOrUpdateTag(TagsGetDto input);

        Task DeleteTag(EntityDto input);

    }
}
