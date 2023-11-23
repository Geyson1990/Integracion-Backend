using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Contable.Application.Exporting;
using Contable.Application.Extensions;
using Contable.Application.SectorMeets;
using Contable.Application.SectorMeets.Dto;
using Contable.Application.SectorMeetSessions.Dto;
using Contable.Application.Uploaders.Dto;
using Contable.Authorization;
using Contable.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet)]
    public class SectorMeetAppService : ContableAppServiceBase, ISectorMeetAppService
    {
        private readonly IRepository<SectorMeet> _sectorMeetRepository;
        private readonly IRepository<SectorMeetResource> _sectorMeetResourceRepository;

        private readonly IRepository<SocialConflict> _socialConflictRepository;
        private readonly IRepository<TerritorialUnit> _territorialUnitRepository;
        private readonly IRepository<SocialConflictLocation> _socialConflictLocationRepository;
        private readonly IRepository<Typology> _typologyRepository;
        private readonly IRepository<SubTypology> _subtypologyRepository;
        private readonly IRepository<DialogSpace> _spaceDialogRepository;
        private readonly ISectorMeetExcelExporter _sectorMeetExcelExporter;

        public SectorMeetAppService(
            IRepository<SectorMeet> sectorMeetRepository,
            IRepository<SectorMeetResource> sectorMeetResourceRepository,
            IRepository<Typology> typologyRepository,
            IRepository<SubTypology> subtypologyRepository,
            IRepository<DialogSpace> spaceDialogRepository,
            IRepository<SocialConflict> socialConflictRepository,
            IRepository<TerritorialUnit> territorialUnitRepository,
            IRepository<SocialConflictLocation> socialConflictLocationRepository,
            ISectorMeetExcelExporter sectorMeetExcelExporter)
        {
            _sectorMeetRepository = sectorMeetRepository;
            _socialConflictRepository = socialConflictRepository;
            _territorialUnitRepository = territorialUnitRepository;
            _sectorMeetResourceRepository = sectorMeetResourceRepository;
            _sectorMeetExcelExporter = sectorMeetExcelExporter;
            _typologyRepository = typologyRepository;
            _socialConflictLocationRepository = socialConflictLocationRepository;
            _subtypologyRepository = subtypologyRepository;
            _spaceDialogRepository = spaceDialogRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet_Create)]
        public async Task<EntityDto> Create(SectorMeetCreateDto input)
        {
            try
            {
                if (input.ReplaceCode)
                {
                    if (input.ReplaceYear <= 0)
                        throw new UserFriendlyException("Aviso", "El Código (Año) de reemplazo es obligatorio");
                    if (input.ReplaceCount <= 0)
                        throw new UserFriendlyException("Aviso", "El Código (Número) de reemplazo es obligatorio");
                    if (await _sectorMeetRepository.CountAsync(p => p.Year == input.ReplaceYear && p.Count == input.ReplaceCount) > 0)
                        throw new UserFriendlyException("Aviso", "El código de reemplazo ya esta en uso. Verifique la información antes de continuar");
                    if (await _sectorMeetRepository.CountAsync(p => p.Code == $"{input.ReplaceYear} - {input.ReplaceCount}") > 0)
                        throw new UserFriendlyException(DefaultTitleMessage, "El código de reemplazo ya esta en uso. Verifique la información antes de continuar");
                }

                var sectorMeetId = await _sectorMeetRepository.InsertAndGetIdAsync(await ValidateEntity(
                    input: ObjectMapper.Map<SectorMeet>(input),
                    socialConflictId: input.SocialConflict == null ? -1 : input.SocialConflict.Id,
                    territorialUnitId: input.TerritorialUnit == null ? -1 : input.TerritorialUnit.Id,
                    uploadFiles: input.UploadFiles ?? new List<SectorMeetSessionAttachmentDto>()
                ));
                try
                {

                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                if (input.ReplaceCode)
                    await FunctionManager.CallCreateSectorMeetCodeReplaceProcess(sectorMeetId, input.ReplaceYear, input.ReplaceCount);
                else
                    await FunctionManager.CallCreateSectorMeetCodeProcess(sectorMeetId);

                return new EntityDto(sectorMeetId);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _sectorMeetRepository.CountAsync(p => p.Id == input.Id));

            await _sectorMeetRepository.DeleteAsync(p => p.Id == input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet)]
        public async Task<SectorMeetGetDataDto> Get(NullableIdDto input)
        {
            var output = new SectorMeetGetDataDto();

            if (input.Id.HasValue)
            {
                VerifyCount(await _sectorMeetRepository.CountAsync(p => p.Id == input.Id.Value));

                var dbSectorMeet = _sectorMeetRepository
                    .GetAll()
                    .Include(p => p.TerritorialUnit)
                    .Include(p => p.SocialConflict)
                    .Where(p => p.Id == input.Id.Value)
                    .First();

                output.SectorMeet = ObjectMapper.Map<SectorMeetGetDto>(dbSectorMeet);

                var datos = _sectorMeetResourceRepository.GetAll()
                  .Where(p => p.SectorMeetId == dbSectorMeet.Id)
                  .ToList();

                //var mapperDatos = ObjectMapper.Map<List<SectorMeetResourceRelationDto>>(datos);
                var mapperDatos = new List<SectorMeetResourceRelationDto>();
                foreach (var item in datos)
                {
                    mapperDatos.Add(new SectorMeetResourceRelationDto
                    {
                        ClassName = item.ClassName,
                        CreationTime = item.CreationTime,
                        Description = item.Description,
                        Name = item.Name,
                        Extension = item.Extension,
                        FileName = item.FileName,
                        Id = item.Id,
                        Size = item.Size,
                        SectionFolder = item.SectionFolder,

                    });
                }

                output.SectorMeet.Resources = mapperDatos;
            }

            output.TerritorialUnits = ObjectMapper.Map<List<SectorMeetTerritorialUnitRelationDto>>(_territorialUnitRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToList());

            return output;
        }


        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet)]
        public async Task<SectorMeetGetDataDto> GetIdCaso(NullableIdDto input)
        {
            var output = new SectorMeetGetDataDto();

            if (input.Id.HasValue)
            {
                VerifyCount(await _sectorMeetRepository.CountAsync(p => p.SocialConflictId == input.Id.Value));

                var dbSectorMeet = _sectorMeetRepository
                    .GetAll()
                    .Include(p => p.TerritorialUnit)
                    .Include(p => p.SocialConflict)
                    .Where(p => p.SocialConflictId == input.Id.Value)
                    .First();

                output.SectorMeet = ObjectMapper.Map<SectorMeetGetDto>(dbSectorMeet);
            }

            output.TerritorialUnits = ObjectMapper.Map<List<SectorMeetTerritorialUnitRelationDto>>(_territorialUnitRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToList());

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet)]
        public async Task<PagedResultDto<SectorMeetGetAllDto>> GetAll(SectorMeetGetAllInputDto input)
        {
            try
            {
                var query = _sectorMeetRepository
                .GetAll()
                .Include(p => p.SocialConflict)
                .Include(p => p.TerritorialUnit)
                .WhereIf(input.DepartmentId.HasValue, p => p.Sessions.Any(p => p.DepartmentId == input.DepartmentId.Value))
                .WhereIf(input.ProvinceId.HasValue, p => p.Sessions.Any(p => p.ProvinceId == input.ProvinceId.Value))
                .WhereIf(input.DistrictId.HasValue, p => p.Sessions.Any(p => p.DistrictId == input.DistrictId.Value))
                .WhereIf(input.PersonId.HasValue, p => p.Sessions.Any(p => p.PersonId == input.PersonId.Value))
                .WhereIf(input.SectorMeetSessionType.HasValue && input.SectorMeetSessionType.Value != SectorMeetSessionType.NONE, p => p.Sessions.Any(d => d.Type == input.SectorMeetSessionType.Value))
                .WhereIf(input.FilterByDate && input.StartTime.HasValue && input.EndTime.HasValue, p => p.CreationTime >= input.StartTime.Value && p.CreationTime <= input.EndTime.Value)
                .LikeAllBidirectional(input.SectorMeetCode.SplitByLike(), nameof(SectorMeet.Code))
                .LikeAllBidirectional(input.SectorMeetName.SplitByLike(), nameof(SectorMeet.MeetName));

                List<SectorMeet> listado = new List<SectorMeet>();

                foreach (var item in query)
                {
                    if (item.State == input.State || item.State == 2)
                    {
                        listado.Add(item);
                    }
                }
                query = listado.AsQueryable();

                //var count = await query.CountAsync();
                var result = query.OrderBy(input.Sorting).PageBy(input);

                return new PagedResultDto<SectorMeetGetAllDto>(listado.Count(), ObjectMapper.Map<List<SectorMeetGetAllDto>>(result));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet_Edit)]
        public async Task<EntityDto> Update(SectorMeetUpdateDto input)
        {
            if (input.ReplaceCode)
            {
                if (input.ReplaceYear <= 0)
                    throw new UserFriendlyException("Aviso", "El Código (Año) de reemplazo es obligatorio");
                if (input.ReplaceCount <= 0)
                    throw new UserFriendlyException("Aviso", "El Código (Número) de reemplazo es obligatorio");
                if (await _sectorMeetRepository.CountAsync(p => p.Year == input.ReplaceYear && p.Count == input.ReplaceCount) > 0)
                    throw new UserFriendlyException("Aviso", "El código de reemplazo ya esta en uso. Verifique la información antes de continuar");
            }

            var sectorMeetId = await _sectorMeetRepository.InsertOrUpdateAndGetIdAsync(await ValidateEntity(
                input: ObjectMapper.Map(input, await _sectorMeetRepository.GetAsync(input.Id)),
                socialConflictId: input.SocialConflict == null ? -1 : input.SocialConflict.Id,
                territorialUnitId: input.TerritorialUnit == null ? -1 : input.TerritorialUnit.Id,
                 uploadFiles: input.UploadFiles ?? new List<SectorMeetSessionAttachmentDto>()
                ));
            try
            {
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            if (input.ReplaceCode)
                await FunctionManager.CallCreateSectorMeetCodeReplaceProcess(sectorMeetId, input.ReplaceYear, input.ReplaceCount);

            return new EntityDto(sectorMeetId);
        }

        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet_Edit)]
        public async Task<EntityDto> GenerateMeetProcess(EntityDto input)
        {
            await FunctionManager.CallGenerateMeetProcess(input.Id);

            return new EntityDto(input.Id);
        }

        private async Task<SectorMeet> ValidateEntity(SectorMeet input, int socialConflictId, int territorialUnitId, List<SectorMeetSessionAttachmentDto> uploadFiles)
        {
            try
            {
                input.MeetName.IsValidOrException("Aviso", "El nombre de la reunión es obligatorio");
                input.MeetName.VerifyTableColumn(SectorMeetConsts.MeetNameMinLength,
                    SectorMeetConsts.MeetNameMaxLength,
                    "Aviso",
                    $"El nombre de la reunión no debe exceder los {SectorMeetConsts.MeetNameMaxLength} caracteres");

                if (await _territorialUnitRepository.CountAsync(p => p.Id == territorialUnitId) == 0)
                    throw new UserFriendlyException("Aviso", "La unidad territorial seleccionada es inválida o ya no existe. Por favor verifique la información antes de continuar.");

                if (socialConflictId > 0)
                {
                    var dbSocialConflict = _socialConflictRepository
                        .GetAll()
                        .Where(p => p.Id == socialConflictId)
                        .FirstOrDefault();

                    if (dbSocialConflict == null)
                        throw new UserFriendlyException("Aviso", "El caso seleccionado es inválido o ya no existe. Por favor verifique la información antes de continuar.");

                    input.SocialConflict = dbSocialConflict;
                    input.SocialConflictId = dbSocialConflict.Id;
                }
                else
                {
                    input.SocialConflict = null;
                    input.SocialConflictId = null;
                }

                var territorialUnit = await _territorialUnitRepository.GetAsync(territorialUnitId);

                input.TerritorialUnit = territorialUnit;
                input.TerritorialUnitId = territorialUnit.Id;
                input.Resources = new List<SectorMeetResource>();

                foreach (var uploadFile in uploadFiles)
                {
                    var recurso = ResourceManager.Create(
                        resource: ObjectMapper.Map<UploadResourceInputDto>(uploadFile),
                        section: ResourceConsts.SectorMeet
                    );
                    //var dbResource = ObjectMapper.Map<SectorMeetResource>(recurso);
                    var dbResource = new SectorMeetResource
                    {
                        Name = recurso.Name,
                        LastModificationTime = DateTime.Now,
                        ResourceFolder = recurso.ResourceFolder,
                        Resource = recurso.Resource,
                        FileName = recurso.FileName,
                        ClassName = recurso.ClassName,
                        SectionFolder = recurso.SectionFolder,
                        CommonFolder = recurso.CommonFolder,
                        Description = recurso.Description,
                        Extension = recurso.Extension,
                        Size = recurso.Size,
                    };

                    input.Resources.Add(dbResource);
                }

                return input;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [AbpAuthorize(AppPermissions.Pages_ConflictTools_SectorMeet)]
        public async Task<FileDto> GetExportMeet(SectorMeetGetAllInputDto input)
        {
            var meet = _sectorMeetRepository
            .GetAll()
            .Include(p => p.SocialConflict)
            .Include(p => p.TerritorialUnit)
            .WhereIf(input.DepartmentId.HasValue, p => p.Sessions.Any(p => p.DepartmentId == input.DepartmentId.Value))
            .WhereIf(input.ProvinceId.HasValue, p => p.Sessions.Any(p => p.ProvinceId == input.ProvinceId.Value))
            .WhereIf(input.DistrictId.HasValue, p => p.Sessions.Any(p => p.DistrictId == input.DistrictId.Value))
            .WhereIf(input.PersonId.HasValue, p => p.Sessions.Any(p => p.PersonId == input.PersonId.Value))
            .WhereIf(input.SectorMeetSessionType.HasValue && input.SectorMeetSessionType.Value != SectorMeetSessionType.NONE, p => p.Sessions.Any(d => d.Type == input.SectorMeetSessionType.Value))
            .WhereIf(input.FilterByDate && input.StartTime.HasValue && input.EndTime.HasValue, p => p.CreationTime >= input.StartTime.Value && p.CreationTime <= input.EndTime.Value)
            .LikeAllBidirectional(input.SectorMeetCode.SplitByLike(), nameof(SectorMeet.Code))
            .LikeAllBidirectional(input.SectorMeetName.SplitByLike(), nameof(SectorMeet.MeetName));


            var data = meet.GroupBy(b => new
            {
                b.CreationTime.Year,
                b.CreationTime.Month
            }).Select(obj => new
            {
                group = obj.Key,
                datos = obj.Count()
            }).ToList();


            var query = meet.GroupBy(b => new
            {
                b.CreationTime.Year,
                b.CreationTime.Month,
                b.SocialConflictId
            })
            .Select(groupedUsers => new
            {
                groupMeet = groupedUsers.Key,
                count = groupedUsers.Count()
            }.groupMeet).ToList();

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


            var resultResum = new List<SectorMeetResumenReport>();
            foreach (var datos in data)
            {
                var item = new SectorMeetResumenReport();
                item.AnioEmision = datos.group.Year.ToString();
                item.MesEmision = ObtenerNombreMes(datos.group.Month);
                item.CantidadSectorMeet = datos.datos;
                item.CantidadCasosConflictivos = resultConflict.Where(x => x.llave.Year == datos.group.Year).Where(x => x.llave.Month == datos.group.Month).Sum(c => c.cantidad);
                item.CantidadPreCaso = 0;


                resultResum.Add(item);
            }

            // Detail

            var resultDetail = new List<SectorMeetDetalleReport>();

            foreach (var dataDetalle in meet)
            {
                var detail = new SectorMeetDetalleReport();
                detail.FechaEmision = dataDetalle.CreationTime.ToString();
                detail.AnioEmision = dataDetalle.CreationTime.Year.ToString();
                detail.MesEmision = ObtenerNombreMes(dataDetalle.CreationTime.Month);
                detail.codigo = dataDetalle.Code;
                detail.nombreReunion = dataDetalle.MeetName;
                detail.unidadTerritorial = dataDetalle.TerritorialUnit.Name;
                detail.codigoConflicto = dataDetalle.SocialConflict.Code;
                detail.nombreConflicto = dataDetalle.SocialConflict.CaseName;

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

                var spaceDialog = _spaceDialogRepository
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


                if (dataDetalle.SocialConflict.TypologyId == null)
                {
                    detail.tipologia = " _ ";
                }
                else
                {
                    var tipoSens = _typologyRepository
                            .GetAll()
                            .Where(a => a.Id == dataDetalle.SocialConflict.TypologyId)
                            .FirstOrDefault();
                    detail.tipologia = tipoSens.Name;

                    var tipoSub = _subtypologyRepository
                        .GetAll()
                    .Where(a => a.TypologyId == tipoSens.Id).ToList();

                    detail.tipologiaDetallada = "";
                    foreach (var into in tipoSub)
                    {
                        detail.tipologiaDetallada = detail.tipologiaDetallada + into.Name + "\n";
                    }
                }
                resultDetail.Add(detail);
            }



            // Estadística


            var resultEstadistica = new List<SectorMeetEstadisticaReport>();
            foreach (var dataEstadistica in meet)
            {
                var entity = new SectorMeetEstadisticaReport();
                entity.FechaEmision = dataEstadistica.CreationTime.ToString();
                entity.AnioEmision = dataEstadistica.CreationTime.Year.ToString();
                entity.MesEmision = ObtenerNombreMes(dataEstadistica.CreationTime.Month);
                entity.codigo = dataEstadistica.Code;
                entity.nombreReunion = dataEstadistica.MeetName;
                entity.unidadTerritorial = dataEstadistica.TerritorialUnit.Name;

                entity.codigoConflicto = dataEstadistica.SocialConflict.Code;
                entity.nombreConflicto = dataEstadistica.SocialConflict.CaseName;

                if (dataEstadistica.SocialConflict.Verification == ConflictVerification.DENIED)
                {
                    entity.estadoConflicto = "No aprobado";
                }
                else if (dataEstadistica.SocialConflict.Verification == ConflictVerification.PROCESS)
                {
                    entity.estadoConflicto = "En Proceso";
                }
                else if (dataEstadistica.SocialConflict.Verification == ConflictVerification.ACCEPTED)
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
               .Where(x => x.SocialConflict.Id == dataEstadistica.SocialConflictId)
               .Where(x => x.IsDeleted == false);

                entity.unidadTerritorialConflicto = "";
                foreach (var into in locationEstadistica)
                {
                    entity.unidadTerritorialConflicto = entity.unidadTerritorialConflicto + into.TerritorialUnit.Name + "\n";
                }

                if (dataEstadistica.SocialConflict.TypologyId == null)
                {
                    entity.tipologia = " _ ";
                    entity.tipologiaDetallada = " _ ";
                }
                else
                {
                    var tipoSens = _typologyRepository
                            .GetAll()
                            .Where(a => a.Id == dataEstadistica.SocialConflict.TypologyId)
                            .FirstOrDefault();
                    entity.tipologia = tipoSens.Name;

                    var tipoSub = _subtypologyRepository
                        .GetAll()
                    .Where(a => a.TypologyId == tipoSens.Id).ToList();

                    entity.tipologiaDetallada = "";
                    foreach (var into in tipoSub)
                    {
                        entity.tipologiaDetallada = entity.tipologiaDetallada + into.Name + "\n";
                    }
                }


                var spaceDialogEstadistica = _spaceDialogRepository
               .GetAll()
               .Include(x => x.DialogSpaceType)
               .Where(x => x.SocialConflictId == dataEstadistica.SocialConflictId)
               .Where(x => x.IsDeleted == false)
               .ToList();

                if (spaceDialogEstadistica.Count == 0)
                {
                    entity.codigoEspaciosDialogo = " _ ";
                    entity.denominacionEspaciosDialogo = " _ ";
                    entity.tipoEspaciosDialogo = " _ ";
                }
                else
                {
                    entity.codigoEspaciosDialogo = "";
                    foreach (var item in spaceDialogEstadistica)
                    {
                        entity.codigoEspaciosDialogo = entity.codigoEspaciosDialogo + item.Code + "\n";
                        entity.tipoEspaciosDialogo = entity.tipoEspaciosDialogo + item.DialogSpaceType.Name + "\n";
                        entity.denominacionEspaciosDialogo = entity.denominacionEspaciosDialogo + item.CaseName + "\n" + "\n";
                    }
                }
                resultEstadistica.Add(entity);
            }
            return _sectorMeetExcelExporter.ExportSectorMeet(resultResum, resultDetail, resultEstadistica);
        }
        private string ObtenerNombreMes(int numero)
        {
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;

            return formatoFecha.GetMonthName(numero).ToUpperInvariant();
        }

    }
}
