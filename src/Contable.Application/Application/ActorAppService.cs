﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.Actors;
using Contable.Application.Actors.Dto;
using Contable.Application.Extensions;
using Contable.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.UI;
using System.ComponentModel.DataAnnotations;
using Contable.Application.Managers.Dto;
using Contable.Application.Orders.Dto;
using Contable.Authorization.Users;
using Contable.Application.Compromises.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.Application.SocialConflictAlerts.Dto;
using Contable.Application.SocialConflictSensibles.Dto;
namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor)]
    public class ActorAppService: ContableAppServiceBase, IActorAppService
    {
        private readonly IRepository<Actor> _actorRepository;
        private readonly IRepository<ActorType> _actorTypeRepository;
        private readonly IRepository<ActorMovement> _actorMovementRepository;
        private readonly EmailAddressAttribute _emailAddressAttribute;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<SocialConflictActor> _socialConflictActorRepository;
        private readonly IRepository<SocialConflict> _socialConflictRepository;
        private readonly IRepository<SocialConflictAlert> _socialConflictAlertRepository;
        private readonly IRepository<SocialConflictSensible> _socialConflictSensibleRepository;

        public ActorAppService(
            IRepository<Actor> actorRepository, 
            IRepository<ActorType> actorTypeRepository, 
            IRepository<ActorMovement> actorMovementRepository,
            IRepository<User, long> userRepository,
            IRepository<SocialConflictActor> socialConflictActorRepository,
            IRepository<SocialConflict> socialConflictRepository,
            IRepository<SocialConflictAlert> socialConflictAlertRepository,
            IRepository<SocialConflictSensible> socialConflictSensibleRepository)
        {
            _actorRepository = actorRepository;
            _actorTypeRepository = actorTypeRepository;
            _actorMovementRepository = actorMovementRepository;
            _emailAddressAttribute = new EmailAddressAttribute();
            _userRepository = userRepository;
            _socialConflictActorRepository = socialConflictActorRepository;
            _socialConflictRepository = socialConflictRepository;
            _socialConflictAlertRepository = socialConflictAlertRepository;
            _socialConflictSensibleRepository = socialConflictSensibleRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor_Create)]
        public async Task<EntityDto<long>> Create(ActorCreateDto input)
        {
            var actorId = await _actorRepository.InsertAndGetIdAsync(await ValidateEntity(
            actor: ObjectMapper.Map<Actor>(input),
            actorTypeId: input.ActorType == null ? -1 : input.ActorType.Id,
            actorMovementId: input.ActorMovement == null ? -1 : input.ActorMovement.Id
            ));
            await CurrentUnitOfWork.SaveChangesAsync();
            return new EntityDto<long>(actorId);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _actorRepository.CountAsync(p => p.Id == input.Id));


            var socialConflictActors = ObjectMapper.Map<List<SocialConflictActorLocationDto>>(_socialConflictActorRepository
                .GetAll()
                .Where(p => p.ActorId == input.Id && p.SocialConflictId >0 && p.Site == ActorSite.SocialConflict)
                .ToList());

            var socialConflictAlertActors = ObjectMapper.Map<List<SocialConflictActorLocationDto>>(_socialConflictActorRepository
                .GetAll()
                .Where(p => p.ActorId == input.Id && p.SocialConflictAlertId > 0 && p.Site == ActorSite.SocialConflictAlert)
                .ToList());

            var socialConflictSensibleActors = ObjectMapper.Map<List<SocialConflictActorLocationDto>>(_socialConflictActorRepository
                .GetAll()
                .Where(p => p.ActorId == input.Id && p.SocialConflictSensibleId > 0 && p.Site == ActorSite.SocialConflictSensible)
                .ToList());

            string mensaje = $"El actor no se puede eliminar porque se encuentra en :{Environment.NewLine}";
            if (socialConflictActors.Count > 0)
                mensaje += $"{socialConflictActors.Count} casos de conflicto.{Environment.NewLine}";
            if (socialConflictAlertActors.Count > 0)
                mensaje += $"{socialConflictAlertActors.Count} alertas de situaciones conflictivas.{Environment.NewLine}";
            if (socialConflictSensibleActors.Count > 0)
                mensaje += $"{socialConflictSensibleActors.Count} situaciones sensibles al conflicto.{Environment.NewLine}";

            if (socialConflictActors.Count > 0 || socialConflictAlertActors.Count > 0 || socialConflictSensibleActors.Count > 0)
                throw new UserFriendlyException(DefaultTitleMessage, mensaje +"Verifique la información antes de continuar");

            await _actorRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor)]
        public async Task<ActorGetDataDto> Get(NullableIdDto input)
        {
            var output = new ActorGetDataDto
            {
                Actor = new ActorGetDto()
            };

            //output.Actor.ActorType = new ActorTypeDto();
            //output.Actor.ActorMovement = new ActorMovementDto();
            //output.Actor.ActorType.Id = -1;
            //output.Actor.ActorMovement.Id = -1;

            if (input.Id.HasValue)
            {
                VerifyCount(await _actorRepository.CountAsync(p => p.Id == input.Id));

                var actor = _actorRepository
                    .GetAll()
                    .Include(p => p.ActorType)
                    .Include(p => p.ActorMovement)
                    .Where(p => p.Id == input.Id.Value)
                    .First();             

                output.SocialConflicts = ObjectMapper.Map<List<SocialConflictGetAllDto>>(_socialConflictRepository
                .GetAll()
                .Include(p => p.Analyst)
                .Include(p => p.Coordinator)
                .Include(p => p.Manager)
                .Include(p => p.Typology)
                .Include(p => p.SubTypology)
                .Include(p => p.Sector)
                .Include(p => p.Actors)
                .Where(p => p.Actors.Any(a => a.ActorId == input.Id)));

                output.SocialConflictAlerts = ObjectMapper.Map<List<SocialConflictAlertGetAllDto>>(_socialConflictAlertRepository
                .GetAll()
                .Include(p => p.Analyst)
                .Include(p => p.Coordinator)
                .Include(p => p.Manager)
                .Include(p => p.Typology)
                .Include(p => p.SubTypology)
                .Include(p => p.Actors)
                .Where(p => p.Actors.Any(a => a.ActorId == input.Id)));

                output.SocialConflictSensibles = ObjectMapper.Map<List<SocialConflictSensibleGetAllDto>>(_socialConflictSensibleRepository
                .GetAll()
                .Include(p => p.Analyst)
                .Include(p => p.Coordinator)
                .Include(p => p.Manager)
                .Include(p => p.Typology)
                .Include(p => p.Actors)
                .Where(p => p.Actors.Any(a => a.ActorId == input.Id)));

                output.Actor = ObjectMapper.Map<ActorGetDto>(actor);
                var creatorUser = actor.CreatorUserId.HasValue ? _userRepository
                .GetAll()
                .Where(p => p.Id == actor.CreatorUserId.Value)
                .FirstOrDefault() : null;

                var editUser = actor.LastModifierUserId.HasValue ? _userRepository
                    .GetAll()
                    .Where(p => p.Id == actor.LastModifierUserId.Value)
                    .FirstOrDefault() : null;

                output.Actor.CreatorUser = creatorUser == null ? null : ObjectMapper.Map<ActorUserDto>(creatorUser);
                output.Actor.EditUser = editUser == null ? null : ObjectMapper.Map<ActorUserDto>(editUser);

            }
            output.ActorTypes = ObjectMapper.Map<List<ActorTypeDto>>(_actorTypeRepository
            .GetAll()
            .OrderBy(p => p.Name)
            .ToList());

            output.ActorMovements = ObjectMapper.Map<List<ActorMovementDto>>(_actorMovementRepository
            .GetAll()
            .OrderBy(p => p.Name)
            .ToList());

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor)]
        public async Task<PagedResultDto<ActorGetAllDto>> GetAll(ActorGetAllInputDto input)
        {
            var query = _actorRepository
               .GetAll()
               .Include(p => p.ActorType)
               .Include(p => p.ActorMovement)
               .LikeAllBidirectional(input.Filter.SplitByLike().Select(word => (Expression<Func<Actor, bool>>)(expression => EF.Functions.Like(expression.FullName, $"%{word}%"))).ToArray());

            var count = await query.CountAsync();
            var output = query.OrderBy(input.Sorting).PageBy(input);
            return new PagedResultDto<ActorGetAllDto>(count, ObjectMapper.Map<List<ActorGetAllDto>>(output));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_Actor_Edit)]
        public async Task<EntityDto> Update(ActorUpdateDto input)
        {
            VerifyCount(await _actorRepository.CountAsync(p => p.Id == input.Id));
            var actorId = await _actorRepository.InsertOrUpdateAndGetIdAsync(await ValidateEntity(
                actor: ObjectMapper.Map(input, await _actorRepository.GetAsync(input.Id)),
                actorTypeId: input.ActorType == null ? -1 : input.ActorType.Id,
                actorMovementId: input.ActorMovement == null ? -1 : input.ActorMovement.Id));

            await CurrentUnitOfWork.SaveChangesAsync();
            return new EntityDto(actorId);
        }
       private async Task<Actor> ValidateEntity(Actor actor, int actorTypeId, int actorMovementId)
        {
            actor.FullName = (actor.FullName ?? "").Trim().ToUpper();
            actor.DocumentNumber = (actor.DocumentNumber ?? "").Trim().ToUpper();
            actor.Institution = (actor.Institution ?? "").Trim().ToUpper();
            actor.InstitutionAddress = (actor.InstitutionAddress ?? "").Trim().ToUpper();

            actor.FullName.IsValidOrException(DefaultTitleMessage, "El nombre del actor es obligatorio");

            actor.FullName.VerifyTableColumn(ActorConsts.FullNameMinLength,
                ActorConsts.FullNameMaxLength,
                DefaultTitleMessage,
                $"El nombre del actor {actor.FullName} no debe exceder los {ActorConsts.FullNameMaxLength} caracteres");

            if (string.IsNullOrWhiteSpace(actor.DocumentNumber) == false)
                actor.DocumentNumber.VerifyTableColumn(ActorConsts.DocumentNumberMinLength, ActorConsts.DocumentNumberMaxLength, DefaultTitleMessage, $"El DNI o RUC debe tener {ActorConsts.DocumentNumberMaxLength} caracteres");

            actor.DocumentNumber.VerifyTableColumn(ActorConsts.DocumentNumberMinLength,
                ActorConsts.DocumentNumberMaxLength,
                DefaultTitleMessage,
                $"El N° de documento del actor {actor.FullName} no debe exceder los {ActorConsts.DocumentNumberMaxLength} caracteres");

            if (actor.Id == 0)
                if (await _actorRepository.CountAsync(p => p.DocumentNumber == actor.DocumentNumber) > 0)
                    throw new UserFriendlyException(DefaultTitleMessage, $"El actor {actor.FullName} ya existe. Verifique la información antes de continuar");

            actor.JobPosition.VerifyTableColumn(ActorConsts.JobPositionMinLength,
                ActorConsts.JobPositionMaxLength,
                DefaultTitleMessage,
                $"El cargo del actor {actor.FullName} no debe exceder los {ActorConsts.JobPositionMaxLength} caracteres");

            actor.Institution.IsValidOrException(DefaultTitleMessage, $"La institución del {actor.FullName} es obligatoria");
            actor.Institution.VerifyTableColumn(ActorConsts.InstitutionMinLength,
                ActorConsts.InstitutionMaxLength,
                DefaultTitleMessage,
                $"La institución a la que pertenece el actor {actor.FullName} no debe exceder los {ActorConsts.InstitutionMaxLength} caracteres");

            actor.PhoneNumber.VerifyTableColumn(ActorConsts.PhoneNumberMinLength,
                ActorConsts.PhoneNumberMaxLength,
                DefaultTitleMessage,
                $"El número de teléfono del actor {actor.FullName} no debe exceder los {ActorConsts.PhoneNumberMaxLength} caracteres");


            if (string.IsNullOrWhiteSpace(actor.EmailAddress) == false && _emailAddressAttribute.IsValid(actor.EmailAddress) == false)
                throw new UserFriendlyException(DefaultTitleMessage, $"El correo electrónico {actor.EmailAddress} del actor es inválido");

            actor.EmailAddress.VerifyTableColumn(ActorConsts.EmailAddressMinLength,
               ActorConsts.EmailAddressMaxLength,
               DefaultTitleMessage,
               $"El correo electrónico del actor {actor.FullName} no debe exceder los {ActorConsts.EmailAddressMaxLength} caracteres");


            if (await _actorTypeRepository.CountAsync(p => p.Id == actorTypeId) == 0)
                throw new UserFriendlyException(DefaultTitleMessage, $"El tipo de actor {actor.ActorType.Name} ya no existe o fue eliminado. Verifique la información antes de continuar");

            var dbActorType = await _actorTypeRepository.GetAsync(actorTypeId);
            actor.ActorType = dbActorType;
            actor.ActorTypeId = dbActorType.Id;

            ActorMovement dbActorMovement = null;
            if (actor.ActorType.ShowMovement)
            {
                if (await _actorMovementRepository.CountAsync(p => p.Id == actorMovementId) == 0)
                    throw new UserFriendlyException(DefaultTitleMessage, $"La capacidad de movilización {actor.ActorMovement.Name} ya no existe o fue eliminado. Verifique la información antes de continuar");

                dbActorMovement = await _actorMovementRepository.GetAsync(actorMovementId);
                actor.ActorMovement = dbActorMovement;
                actor.ActorMovementId = dbActorMovement.Id;
            }
            else
            {
                actor.ActorMovement = null;
                actor.ActorMovementId = null;
            }

            if (actor.ActorType.ShowDetail)
            {
                actor.Position.VerifyTableColumn(ActorConsts.PositionMinLength,
                    ActorConsts.PositionMaxLength,
                    DefaultTitleMessage,
                    $"La posición del actor {actor.FullName} no debe exceder los {ActorConsts.PositionMaxLength} caracteres");
                actor.Interest.VerifyTableColumn(ActorConsts.InterestMinLength,
                    ActorConsts.InterestMaxLength,
                    DefaultTitleMessage,
                    $"El interés del actor {actor.FullName} no debe exceder los {ActorConsts.InterestMaxLength} caracteres");
            }

            if (actor.IsPoliticalAssociation)
            {
                actor.PoliticalAssociation.VerifyTableColumn(ActorConsts.PoliticalAssociationMinLength,
                    ActorConsts.PoliticalAssociationMaxLength,
                    DefaultTitleMessage,
                    $"El nombre del partido político al que pertenece el actor {actor.FullName} no debe exceder los {ActorConsts.PoliticalAssociationMaxLength} caracteres");
            }

            return actor;
        }
    }
}
