using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Abp.Authorization;
using Contable.Authorization.Institution;
using Contable.Authorization.Institution.InstitutionDto;
using Contable.Authorization.Users.Dto;
using Abp.Domain.Repositories;
using PayPalCheckoutSdk.Orders;
using Contable.Authorization.Institution.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Extensions;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Z.EntityFramework.Plus;
using Abp.Linq.Extensions;
using Contable.Application;
using Contable.Application.Sectors.Dto;
using Abp.Authorization.Users;
using Abp.Runtime.Session;
using Abp.UI;
using NPOI.OpenXmlFormats.Shared;

namespace Contable.Authorization.Institution
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Institution)]
    public class InstitutionAppServices : ContableAppServiceBase, IInstitutionAppService
    {

        private readonly IRepository<Institutions> _institutionsRepository;
        private readonly IRepository<Sector> _sectorRepository;

        public InstitutionAppServices(
            IRepository<Institutions> institutionRepository,
            IRepository<Sector> sectorRepository
           
            )
        {
            _institutionsRepository = institutionRepository;
            _sectorRepository = sectorRepository;
            
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_Institutions_Create)]
        public async Task CreateOrUpdateInstitution(GetInstitutionDto input)
        {
            if (input.id > 0)
            {
                if(await _institutionsRepository.CountAsync(p=> p.Id == input.id)>0)
                {
                    var DBInstitution=await _institutionsRepository.GetAsync(input.id);
                    DBInstitution.Name = input.name;
                    DBInstitution.ShortName = input.shortName;
                    DBInstitution.Ruc=input.ruc;
                    DBInstitution.Tokent = input.tokent;
                    DBInstitution.ContacName = input.contacName;
                    DBInstitution.EmailAddress = input.EmailAddress;
                    DBInstitution.PhoneNumber= input.PhoneNumber;

                    await _institutionsRepository.UpdateAsync(DBInstitution);
                }
            }
            else{
                var newInstitution = new Institutions
                {
                    Name = input.name,
                    ShortName = input.shortName,
                    Ruc = input.ruc,
                    Tokent = input.tokent,
                    ContacName = input.contacName,
                    EmailAddress = input.EmailAddress,
                    PhoneNumber = input.PhoneNumber,
                    SectorId = input.sector.Id
                };
                await _institutionsRepository.InsertAsync(newInstitution);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Institutions_Delete)]
        public async Task DeleteInstitution(EntityDto<long> input)
        {
            VerifyCount(await _institutionsRepository.CountAsync(p => p.Id == input.Id));
            await _institutionsRepository.DeleteAsync(p => p.Id == input.Id);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Institution)]
        public async Task<PagedResultDto<GetInstitutionDto>> GetInstitution()
        {
            var query = _institutionsRepository
                .GetAll();

            var count = await query.CountAsync();
            var output = ObjectMapper.Map<List<GetInstitutionDto>>(query);

            return new PagedResultDto<GetInstitutionDto>(count, output);

        }

        public async Task<GetCreateOrUpdateInstitutionDto> GetInstitutionForEdit(NullableIdDto<long> input)
        {
            var output = new GetCreateOrUpdateInstitutionDto
            {
                institution = new GetInstitutionDto()
            };

            if (input.Id.HasValue)
            {
                VerifyCount(await _institutionsRepository.CountAsync(p => p.Id == input.Id));
                
                var institution = _institutionsRepository
                    .GetAll()
                    .Include(p => p.Sector)
                    .FirstOrDefault(p => p.Id == input.Id);

                output.institution = ObjectMapper.Map<GetInstitutionDto>(institution);
            }
            output.sectors = ObjectMapper.Map<List<SectorGetDto>>(_sectorRepository
                .GetAll()
                .OrderBy(p=>p.Name)
                .ToList());

            return output;
        }
    }
}
