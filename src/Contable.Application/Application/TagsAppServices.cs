
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Contable.Application.InterventionPlans.Dto;
using Contable.Application.Sectors.Dto;
using Contable.Application.Tag;
using Contable.Application.Tag.Dto;
using Contable.Authorization;
using Contable.Authorization.Institution;
using Contable.Authorization.Institution.Dto;
using Contable.Authorization.Institution.InstitutionDto;
using Contable.Authorization.Roles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Maintenance_Tags)]
    public class TagsAppServices : ContableAppServiceBase, ITagsAppServices
    {
        private readonly IRepository<Tags> _tagsRepository;
        private readonly IRepository<Institutions> _institutionsRepository;
        
        public TagsAppServices( 
            IRepository <Tags> tagsRepository,
            IRepository<Institutions> institutionRepository) 
        {
            _tagsRepository = tagsRepository;
            _institutionsRepository= institutionRepository;
        }
        [AbpAuthorize(AppPermissions.Pages_Maintenance_Tags_Create)]
        public  async Task CreateOrUpdateTag(TagsGetDto input)
        {
            if (input.id > 0)
            {
                if (await _tagsRepository.CountAsync(p => p.Id == input.id) > 0)
                {
                    var DBtags = await _tagsRepository.GetAsync((int)input.id);
                    DBtags.Name = input.name;
                    DBtags.InstitutionId = input.institutionId;


                    await _tagsRepository.UpdateAsync(DBtags);
                }
            }
            else
            {
                var newTags = new Tags()
                {
                    Name = input.name,
                    InstitutionId= input.institutionId,
                };  
                 await _tagsRepository.InsertAsync(newTags);
            }

        }
        [AbpAuthorize(AppPermissions.Pages_Maintenance_Tags_Delete)]
        public async Task DeleteTag(EntityDto input)
        {
            VerifyCount(await _tagsRepository.CountAsync(p => p.Id == input.Id));
            await _tagsRepository.DeleteAsync(p => p.Id == input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Tags_Edit)]
        public  async Task<TagsGetDto> GetTagForEdit(NullableIdDto<long> input)
        {
            var output = new TagsGetDto();

            if (input.Id.HasValue)
            {
                VerifyCount(await _tagsRepository.CountAsync(p => p.Id == input.Id));

                var tags = _tagsRepository
                    .GetAll()
                    .Include(p => p.Institution)
                    .FirstOrDefault(p => p.Id == input.Id);

                output = ObjectMapper.Map<TagsGetDto> (tags);
            }
            

            return output;
        }

        public async Task<PagedResultDto<TagsGetDto>> GetTagsforInstitution(NullableIdDto input)
        {
            var query = _tagsRepository
                .GetAll()
                .Include(p=> p.Institution)
                .Where(p=> p.InstitutionId == input.Id);  
            var count = await query.CountAsync();
            var result = ObjectMapper.Map<List<TagsGetDto>>(query);

            return new PagedResultDto<TagsGetDto>(count, result);
        }
    }
}
