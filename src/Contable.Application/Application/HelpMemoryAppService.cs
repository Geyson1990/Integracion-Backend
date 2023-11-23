using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.HelpMemories;
using Contable.Application.HelpMemories.Dto;
using Contable.Application.Extensions;
using Contable.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contable.Application.Uploaders.Dto;
using Abp.UI;
using Contable.Authorization.Users;
using Contable.Dto;
using Contable.Application.Exporting;
using System.Globalization;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory)]
    public class HelpMemoryAppService : ContableAppServiceBase, IHelpMemoryAppService
    {
        private readonly IRepository<HelpMemory> _helpMemoryRepository;
        private readonly IRepository<HelpMemoryResource> _helpMemoryResourceRepository;
        private readonly IRepository<DirectoryGovernment> _directoryGovernmentRepository;
        private readonly IRepository<SocialConflict> _socialConflictRepository;
        private readonly IRepository<SocialConflictSensible> _socialConflictSensibleRepository;
        private readonly IRepository<SocialConflictLocation> _socialConflictLocationRepository;
        private readonly IRepository<SocialConflictSensibleLocation> _socialConflictSensibleLocationRepository;
        private readonly IRepository<Typology> _typologyRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IHelpMemoryExcelExporter _helpMemoryExcelExporter;
        private readonly IRepository<SocialConflictCondition> _socialConflictConditionRepository;
        private readonly IRepository<SubTypology> _subtypologyRepository;
        private readonly IRepository<SocialConflictSensibleCondition> _socialConflictSensibleConditionRepository;
        private readonly IRepository<DialogSpace> _dialogSpaceRepository;

        public HelpMemoryAppService(
            IRepository<HelpMemory> helpMemoryRepository, 
            IRepository<HelpMemoryResource> helpMemoryResourceRepository,
            IRepository<Typology> typologyRepository,
            IRepository<SubTypology> subtypologyRepository,
            IRepository<DirectoryGovernment> directoryGovernmentRepository,
            IRepository<SocialConflict> socialConflictRepository, 
            IRepository<SocialConflictSensible> socialConflictSensibleRepository,
            IRepository<SocialConflictSensibleCondition> socialConflictSensibleConditionRepository,
            IRepository<SocialConflictLocation> socialConflictLocationRepository,
            IRepository<SocialConflictSensibleLocation> socialConflictSensibleLocationRepository,
            IRepository<DialogSpace> dialogSpaceRepository,
            IRepository<User, long> userRepository,
            IRepository<SocialConflictCondition> socialConflictConditionRepository,
            IHelpMemoryExcelExporter helpMemoryExcelExporter)
        {
            _helpMemoryRepository = helpMemoryRepository;
            _helpMemoryResourceRepository = helpMemoryResourceRepository;
            _directoryGovernmentRepository = directoryGovernmentRepository;
            _socialConflictRepository = socialConflictRepository;
            _socialConflictSensibleRepository = socialConflictSensibleRepository;
            _socialConflictLocationRepository = socialConflictLocationRepository;
            _userRepository = userRepository;
            _socialConflictConditionRepository = socialConflictConditionRepository;
            _helpMemoryExcelExporter = helpMemoryExcelExporter;
            _typologyRepository = typologyRepository;
            _socialConflictSensibleConditionRepository = socialConflictSensibleConditionRepository;
            _socialConflictSensibleLocationRepository = socialConflictSensibleLocationRepository;
            _dialogSpaceRepository = dialogSpaceRepository;
            _subtypologyRepository = subtypologyRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory_Create)]
        public async Task<EntityDto> Create(HelpMemoryCreateDto input)
        {
            var helpMemoryId = await _helpMemoryRepository.InsertOrUpdateAndGetIdAsync(await ValidateEntity(
                input: ObjectMapper.Map<HelpMemory>(input),
                socialConflictId: input.SocialConflict == null ? -1 : input.SocialConflict.Id,
                socialConflictSensibleId: input.SocialConflictSensible == null ? -1 : input.SocialConflictSensible.Id,
                directoryGovernmentId: input.DirectoryGovernment == null ? -1 : input.DirectoryGovernment.Id,
                resources: input.Resources,
                uploads: input.UploadFiles));

            await CurrentUnitOfWork.SaveChangesAsync();

            await FunctionManager.CallCreateHelpMemoryCodeProcess(helpMemoryId);

            return new EntityDto(helpMemoryId);
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _helpMemoryRepository.CountAsync(p => p.Id == input.Id));

            await _helpMemoryRepository.DeleteAsync(p => p.Id == input.Id);
            await _helpMemoryResourceRepository.DeleteAsync(p => p.HelpMemoryId == input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory)]
        public async Task<HelpMemoryGetDto> Get(HelpMemoryGetDto input)
        {
            VerifyCount(await _helpMemoryRepository.CountAsync(p => p.Id == input.Id));

            var helpMemory = _helpMemoryRepository
                .GetAll()
                .Include(p => p.SocialConflict)
                .Include(p => p.SocialConflictSensible)
                .Include(p => p.DirectoryGovernment)
                .Include(p => p.Resources)
                .Where(p => p.Id == input.Id)
                .FirstOrDefault();

            var output = ObjectMapper.Map<HelpMemoryGetDto>(helpMemory);

            foreach(var resource in output.Resources)
            {
                resource.Name = helpMemory.Code;
            }

            output.CreatorUser = helpMemory.CreatorUserId.HasValue == false ? null : ObjectMapper.Map<HelpMemoryUserDto>(_userRepository
            .GetAll()
            .Where(p => p.Id == helpMemory.CreatorUserId.Value)
            .FirstOrDefault());

            output.EditionUser = helpMemory.LastModifierUserId.HasValue == false ? null : ObjectMapper.Map<HelpMemoryUserDto>(_userRepository
                .GetAll()
                .Where(p => p.Id == helpMemory.LastModifierUserId.Value)
                .FirstOrDefault());

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory)]
        public async Task<PagedResultDto<HelpMemoryGetAllDto>> GetAll(HelpMemoryGetAllInputDto input)
        {
            var query = _helpMemoryRepository
                .GetAll()
                .Include(p => p.SocialConflict)
                .Include(p => p.SocialConflictSensible)
                .Include(p => p.DirectoryGovernment)
                .Include(p => p.Resources)
                .WhereIf(input.SocialConflictId.HasValue, p => p.SocialConflictId == input.SocialConflictId.Value)
                .WhereIf(input.SocialConflictSensibleId.HasValue, p => p.SocialConflictSensibleId == input.SocialConflictSensibleId.Value)
                .WhereIf(input.DirectoryGovernmentId.HasValue, p => p.DirectoryGovernmentId == input.DirectoryGovernmentId.Value)
                .LikeAllBidirectional(input
                    .SocialConflictCode
                    .SplitByLike()
                    .Select(word => (Expression<Func<HelpMemory, bool>>)(expression => expression.SocialConflict != null && EF.Functions.Like(expression.SocialConflict.Code, $"%{word}%")))
                    .ToArray())
                .LikeAllBidirectional(input
                    .SocialConflictSensibleCode
                    .SplitByLike()
                    .Select(word => (Expression<Func<HelpMemory, bool>>)(expression => expression.SocialConflictSensible != null && EF.Functions.Like(expression.SocialConflictSensible.Code, $"%{word}%")))
                    .ToArray())
                .LikeAllBidirectional(input.HelpMemoryRequest.SplitByLike(), nameof(HelpMemory.Request));

            var count = await query.CountAsync();
            var result = query.OrderBy(input.Sorting).PageBy(input);

            return new PagedResultDto<HelpMemoryGetAllDto>(count, ObjectMapper.Map<List<HelpMemoryGetAllDto>>(result));
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory)]
        public async Task<FileDto> GetExportHelpMemory(HelpMemoryGetAllInputDto input)
        {



            var helpMemomry = _helpMemoryRepository
                .GetAll()
                .Include(p => p.SocialConflict)
                .Include(p => p.SocialConflictSensible)
                .Include(p => p.DirectoryGovernment)
                .WhereIf(input.SocialConflictId.HasValue, p => p.SocialConflictId == input.SocialConflictId.Value)
                .WhereIf(input.SocialConflictSensibleId.HasValue, p => p.SocialConflictSensibleId == input.SocialConflictSensibleId.Value)
                .WhereIf(input.DirectoryGovernmentId.HasValue, p => p.DirectoryGovernmentId == input.DirectoryGovernmentId.Value)
                .LikeAllBidirectional(input
                    .SocialConflictCode
                    .SplitByLike()
                    .Select(word => (Expression<Func<HelpMemory, bool>>)(expression => expression.SocialConflict != null && EF.Functions.Like(expression.SocialConflict.Code, $"%{word}%")))
                    .ToArray())
                .LikeAllBidirectional(input
                    .SocialConflictSensibleCode
                    .SplitByLike()
                    .Select(word => (Expression<Func<HelpMemory, bool>>)(expression => expression.SocialConflictSensible != null && EF.Functions.Like(expression.SocialConflictSensible.Code, $"%{word}%")))
                    .ToArray())
                .LikeAllBidirectional(input.HelpMemoryRequest.SplitByLike(), nameof(HelpMemory.Request));


            var data = helpMemomry.GroupBy(b => new
            {
                b.RequestTime.Year,
                b.RequestTime.Month
            }).Select(obj => new
            {
                group = obj.Key,
                datos = obj.Count()
            }).ToList();




            var query = helpMemomry.GroupBy(b => new
                                                {
                                                    b.RequestTime.Year,
                                                    b.RequestTime.Month,
                                                    b.SocialConflictId,
                                                    b.SocialConflictSensibleId
                                                 })
                                            .Select(groupedUsers => new
                                            {
                                                groupHelpMemory = groupedUsers.Key,
                                                count = groupedUsers.Count()
                                            }.groupHelpMemory).ToList();


            var CountSocialConflic = query.Where(sc => sc.SocialConflictId != null).Count();
            var CountSensible = query.Where(sc => sc.SocialConflictSensibleId != null).Count();

            var resultConflict = query.Where(sc => sc.SocialConflictId != null)
                .GroupBy(d => new
            {
                d.Year,
                d.Month
            }).Select(data => new
            {
                llave = data.Key,
                cantidad = data.Count()
            });

            var resultSensible = query.Where(sc => sc.SocialConflictSensibleId != null)
                .GroupBy(d => new
                {
                    d.Year,
                    d.Month
                }).Select(data => new
                {
                    llave = data.Key,
                    cantidad = data.Count()
                });

            var resultFinal = resultConflict.Concat(resultSensible);

            var resultResum = new List<HelpMemoryResumenReport>();
            foreach (var datos in data)
            {
                var item = new HelpMemoryResumenReport();
                item.AnioEmision = datos.group.Year.ToString();
                item.MesEmision = ObtenerNombreMes(datos.group.Month);
                item.CantidadAyudaMemoria = datos.datos;
                item.CantidadCasosConflictivos = resultConflict.Where(x => x.llave.Year == datos.group.Year).Where(x => x.llave.Month == datos.group.Month).Sum(c => c.cantidad);
                item.CantidadConflictividadSocial = resultSensible.Where(x => x.llave.Year == datos.group.Year).Where(x => x.llave.Month == datos.group.Month).Sum(c => c.cantidad);

                resultResum.Add(item);
            }


            var resultDetail = new List<HelpMemoryDetalleReport>();
            foreach (var dataDetalle in helpMemomry)
            {
                var detail = new HelpMemoryDetalleReport();
                detail.FechaEmision = dataDetalle.RequestTime.ToString();
                detail.AnioEmision = dataDetalle.RequestTime.Year.ToString();
                detail.MesEmision = ObtenerNombreMes(dataDetalle.RequestTime.Month);
                detail.codigo = dataDetalle.Code;
                detail.entidadSolicitante = dataDetalle.DirectoryGovernment.Name;
                detail.personaSolicitante = dataDetalle.Request;
                detail.codigoConflicto = dataDetalle.SocialConflictId == null ? " _ " : dataDetalle.SocialConflict.Code;
                detail.nombreConflicto = dataDetalle.SocialConflictId == null ? " _ " : dataDetalle.SocialConflict.CaseName;

                if (dataDetalle.SocialConflictId == null)
                {
                    detail.estadoConflicto = " _ ";
                    detail.unidadTerritorialConflicto = " _ ";
                    detail.tipologia = " _ ";
                    detail.tipologiaDetallada = " _ ";
                    detail.cantidadEspaciosDialogo = "0";
                    detail.codigoEspaciosDialogo = " _ ";
                } else
                {

                    var location = _socialConflictLocationRepository
                       .GetAll()
                       .Include(x => x.SocialConflict)
                       .Include(x => x.TerritorialUnit)
                       .Include(x => x.Department)
                       .Include(x => x.Province)
                       .Include(x => x.District)
                       .Where(x => x.SocialConflict.Id == dataDetalle.SocialConflictId)
                       .Where(x => x.IsDeleted == false);

                    detail.unidadTerritorialConflicto = "";
                    foreach (var into in location)
                    {
                        detail.unidadTerritorialConflicto = detail.unidadTerritorialConflicto + into.TerritorialUnit.Name + "\n";
                    }

                    if (dataDetalle.SocialConflict.TypologyId == null)
                    {
                        detail.tipologia = " _ ";
                        detail.tipologiaDetallada  = " _ ";
                    } else {
                        var tipo = _typologyRepository
                                    .GetAll()
                                    .Where(a => a.Id == dataDetalle.SocialConflict.TypologyId)
                                    .FirstOrDefault();

                        detail.tipologia = tipo.Name;

                        var tipoSub = _subtypologyRepository
                            .GetAll()
                            .Where(a => a.TypologyId == tipo.Id).ToList();

                        detail.tipologiaDetallada = "";
                        foreach (var into in tipoSub)
                        {
                            detail.tipologiaDetallada = detail.tipologiaDetallada + into.Name + "\n";
                        }

                    }

                    if (dataDetalle.SocialConflict.Verification == ConflictVerification.DENIED)
                    {
                        detail.estadoConflicto = "No aprobado";
                    }
                    else if (dataDetalle.SocialConflict.Verification == ConflictVerification.PROCESS)
                    {
                        detail.estadoConflicto = "En Proceso";
                    }
                    else if (dataDetalle.SocialConflict.Verification == ConflictVerification.ACCEPTED)
                    {
                        detail.estadoConflicto = "Aprobado";
                    }

                    var spaceDialog = _dialogSpaceRepository
                       .GetAll()
                       .Where(x => x.SocialConflictId == dataDetalle.SocialConflictId)
                       .Where(x => x.IsDeleted == false)
                       .ToList();

                    if (spaceDialog.Count == 0)
                    {
                        detail.cantidadEspaciosDialogo = "0";
                        detail.codigoEspaciosDialogo = " _ ";
                    }
                    else
                    {
                        detail.cantidadEspaciosDialogo = spaceDialog.Count().ToString();
                        detail.codigoEspaciosDialogo = "";
                        foreach (var item in spaceDialog)
                        {
                            detail.codigoEspaciosDialogo = detail.codigoEspaciosDialogo + item.Code + "\n";
                        }
                    }

                }

                detail.codigoSensible = dataDetalle.SocialConflictSensibleId == null ? " _ " : dataDetalle.SocialConflictSensible.Code;
                detail.nombreSensible = dataDetalle.SocialConflictSensibleId == null ? " _ " : dataDetalle.SocialConflictSensible.CaseName;

                if (dataDetalle.SocialConflictSensibleId == null)
                {
                    detail.estadoSensible= " _ ";
                    detail.unidadTerritorialSensible = " _ ";
                    detail.tipologiaSensible = " _ ";
                } else
                {
                    var conditionSensibleLast = _socialConflictSensibleConditionRepository
                                        .GetAll()
                                        .Where(x => x.Id == dataDetalle.SocialConflictSensible.LastSocialConflictSensibleConditionId.Value)
                                        .FirstOrDefault();

                    detail.estadoSensible = conditionSensibleLast.Description;

                    var locationSensible = _socialConflictSensibleLocationRepository
                               .GetAll()
                               .Include(x => x.SocialConflictSensible)
                               .Include(x => x.TerritorialUnit)
                               .Include(x => x.Department)
                               .Include(x => x.Province)
                               .Include(x => x.District)
                               .Where(x => x.SocialConflictSensible.Id == dataDetalle.SocialConflictSensible.Id);

                    detail.unidadTerritorialSensible = "";
                    foreach (var into in locationSensible)
                    {
                        detail.unidadTerritorialSensible = detail.unidadTerritorialSensible + into.TerritorialUnit.Name + "\n";
                    }

                    if (dataDetalle.SocialConflictSensible.TypologyId == null)
                    {
                        detail.tipologiaSensible = " _ ";
                    }
                    else
                    {
                        var tipoSens = _typologyRepository
                                .GetAll()
                                .Where(a => a.Id == dataDetalle.SocialConflictSensible.TypologyId)
                                .FirstOrDefault();
                        detail.tipologiaSensible = tipoSens.Name;
                    }
                }

                resultDetail.Add(detail);

            }

            var estadistica = helpMemomry
                                .Where(x => x.SocialConflictId != null)
                                .Include(x => x.SocialConflict).ToList();

            var resultEstadistica = new List<HelpMemoryDetalleEstadisticaReport>();
            foreach (var dataDetalle in estadistica)
            {
                var entity = new HelpMemoryDetalleEstadisticaReport();

                entity.FechaEmision = dataDetalle.RequestTime.ToString();
                entity.AnioEmision = dataDetalle.RequestTime.Year.ToString();
                entity.MesEmision = ObtenerNombreMes(dataDetalle.RequestTime.Month);
                entity.codigo = dataDetalle.Code;
                entity.entidadSolicitante = dataDetalle.DirectoryGovernment.Name;
                entity.personaSolicitante = dataDetalle.Request;

                var sensible = _socialConflictRepository
                    .GetAll()
                    .Where(x => x.Id == dataDetalle.SocialConflictId)
                    .FirstOrDefault();

                entity.codigoConflicto = dataDetalle.SocialConflict.Code;
                entity.nombreConflicto = dataDetalle.SocialConflict.CaseName;
                if (dataDetalle.SocialConflict.Verification == ConflictVerification.DENIED)
                {
                    entity.estadoConflicto = "No aprobado";
                }
                else if (dataDetalle.SocialConflict.Verification == ConflictVerification.PROCESS)
                {
                    entity.estadoConflicto = "En Proceso";
                }
                else if (dataDetalle.SocialConflict.Verification == ConflictVerification.ACCEPTED)
                {
                    entity.estadoConflicto = "Aprobado";
                }

                var locationEstadistica = _socialConflictLocationRepository
                   .GetAll()
                   .Include(x => x.SocialConflict)
                   .Include(x => x.TerritorialUnit)
                   .Include(x => x.Department)
                   .Include(x => x.Province)
                   .Include(x => x.District)
                   .Where(x => x.SocialConflict.Id == dataDetalle.SocialConflictId);



                entity.unidadTerritorialConflicto = "";
                foreach (var into in locationEstadistica)
                {
                    entity.unidadTerritorialConflicto = entity.unidadTerritorialConflicto + into.TerritorialUnit.Name + "\n";
                }
                if (dataDetalle.SocialConflict.TypologyId == null)
                {
                    entity.tipologiaConflicto = " _ ";
                }
                else
                {
                    var tipoEstad = _typologyRepository
                            .GetAll()
                            .Where(a => a.Id == dataDetalle.SocialConflict.TypologyId)
                            .FirstOrDefault();
                    entity.tipologiaConflicto = tipoEstad.Name;
                }



                var dialogRespo = _dialogSpaceRepository
                    .GetAll()
                    .Include(x => x.DialogSpaceType)
                    .Where(x => x.SocialConflictId == dataDetalle.SocialConflictId).FirstOrDefault();

                if(dialogRespo != null)
                {
                    entity.codigoEspacio = dialogRespo.Code;
                    entity.denominacionEspacio = dialogRespo.CaseName;
                    entity.tipoEspacio = dialogRespo.DialogSpaceType.Name;
                } else
                {
                    entity.codigoEspacio = " _ ";
                    entity.denominacionEspacio = " _ ";
                    entity.tipoEspacio = " _ ";
                }


                resultEstadistica.Add(entity);

            }


                return _helpMemoryExcelExporter.ExportHelpMemory(resultResum, resultDetail, resultEstadistica);


        }
        private string ObtenerNombreMes(int numero)
        {
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
             
            return formatoFecha.GetMonthName(numero).ToUpperInvariant();
        }

        [AbpAuthorize(AppPermissions.Pages_Application_HelpMemory_Edit)]
        public async Task<EntityDto> Update(HelpMemoryUpdateDto input)
        {
            VerifyCount(await _helpMemoryRepository.CountAsync(p => p.Id == input.Id));

            var helpMemoryId = await _helpMemoryRepository.InsertOrUpdateAndGetIdAsync(await ValidateEntity(
                input: ObjectMapper.Map(input, await _helpMemoryRepository.GetAsync(input.Id)),
                socialConflictId: input.SocialConflict == null ? -1 : input.SocialConflict.Id,
                socialConflictSensibleId: input.SocialConflictSensible == null ? -1 : input.SocialConflictSensible.Id,
                directoryGovernmentId: input.DirectoryGovernment == null ? -1 : input.DirectoryGovernment.Id,
                resources: input.Resources,
                uploads: input.UploadFiles));

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto(helpMemoryId);
        }
        private async Task<HelpMemory> ValidateEntity(
            HelpMemory input,
            int socialConflictId,
            int socialConflictSensibleId,
            int directoryGovernmentId,
            List<HelpMemoryResourceRelationDto> resources,
            List<UploadResourceInputDto> uploads)
        {
            input.Request.IsValidOrException(DefaultTitleMessage, "La denominación del caso (mesa de diálogo) es obligatoria");
            input.Request.VerifyTableColumn(HelpMemoryConsts.RequestMinLength,
                HelpMemoryConsts.RequestMaxLength, 
                DefaultTitleMessage, 
                $"El texto de la persona solicitante no debe exceder los {HelpMemoryConsts.RequestMaxLength} caracteres");

            if (input.RequestTime > DateTime.Now)
                throw new UserFriendlyException("Aviso", "La fecha de solicitud no debe ser mayor a la fecha actual");

            if (input.Id <= 0 && uploads.Count == 0)
                throw new UserFriendlyException("Aviso", "Debe subir el documento de ayuda y memoria para guardar el registro.");

            if (uploads.Count > 1)
                throw new UserFriendlyException("Aviso", "Solo puedes subir un archivo de ayuda y memoria, remueve alguno para continuar");

            input.Resources = new List<HelpMemoryResource>();

            if (socialConflictId > 0)
            {
                if (await _socialConflictRepository.CountAsync(p => p.Id == socialConflictId) == 0)
                    throw new UserFriendlyException("Aviso", "El conflicto social seleccionado ya no existe o fue eliminado. Verifique la información antes de continuar");

                var socialConflict = await _socialConflictRepository.GetAsync(socialConflictId);

                input.SocialConflict = socialConflict;
                input.SocialConflictId = socialConflict.Id;
                input.Site = ConflictSite.SocialConflict;
            }
            else
            {
                input.SocialConflict = null;
                input.SocialConflictId = null;
                input.Site = ConflictSite.All;
            }

            if (socialConflictSensibleId > 0)
            {
                if (await _socialConflictRepository.CountAsync(p => p.Id == socialConflictSensibleId) == 0)
                    throw new UserFriendlyException("Aviso", "El coordinador de la UT ya no existe o fue eliminado. Verifique la información antes de continuar");

                var socialConflictSensible = await _socialConflictSensibleRepository.GetAsync(socialConflictSensibleId);

                input.SocialConflictSensible = socialConflictSensible;
                input.SocialConflictSensibleId = socialConflictSensible.Id;
                input.Site = ConflictSite.SocialConflictSensible;
            }
            else
            {
                if(socialConflictId <= 0 && socialConflictSensibleId <= 0)
                {
                    input.SocialConflictSensible = null;
                    input.SocialConflictSensibleId = null;
                    input.Site = ConflictSite.All;
                }
            }

            if (await _directoryGovernmentRepository.CountAsync(p => p.Id == directoryGovernmentId) == 0)
                throw new UserFriendlyException("Aviso", "El gestor del cargo ya no existe o fue eliminado. Verifique la información antes de continuar");

            var directoryGovernment = await _directoryGovernmentRepository.GetAsync(directoryGovernmentId);

            input.DirectoryGovernment = directoryGovernment;
            input.DirectoryGovernmentId = directoryGovernment.Id;

            if (input.Site == ConflictSite.All)
                throw new UserFriendlyException("Aviso", "Debe seleccionar el conflicto antes de continuar");

            if(input.Id > 0)
            {
                var dbResources = _helpMemoryResourceRepository
                    .GetAll()
                    .Where(p => p.HelpMemoryId == input.Id)
                    .ToList();

                foreach (var dbResource in dbResources)
                    await _helpMemoryResourceRepository.DeleteAsync(dbResource.Id);
            }

            foreach (var resource in resources)
            {
                if (resource.Remove)
                {
                    if (resource.Id > 0 && input.Id > 0 && await _helpMemoryResourceRepository.CountAsync(p => p.Id == resource.Id && p.HelpMemoryId == input.Id) > 0)
                    {
                        await _helpMemoryResourceRepository.DeleteAsync(resource.Id);
                    }
                }
            }

            foreach (var upload in uploads)
            {
                if (ResourceManager.TokenIsValid(upload.Token) == false)
                    throw new UserFriendlyException("Aviso", "La validez de los archivos subidos a caducado, por favor intente nuevamente.");
            }

            foreach (var upload in uploads)
            {
                input.Resources.Add(ObjectMapper.Map<HelpMemoryResource>(ResourceManager.Create(upload, ResourceConsts.HelpMemory)));
            }

            return input;
        }
    }
}
