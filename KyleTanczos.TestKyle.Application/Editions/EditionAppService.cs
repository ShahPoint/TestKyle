﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Editions;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Editions.Dto;

namespace KyleTanczos.TestKyle.Editions
{
    [AbpAuthorize(AppPermissions.Pages_Editions)]
    public class EditionAppService: TestKyleAppServiceBase, IEditionAppService
    {
        private readonly EditionManager _editionManager;

        public EditionAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;
        }

        public async Task<ListResultOutput<EditionListDto>> GetEditions()
        {
            var editions = await _editionManager.Editions.ToListAsync();
            return new ListResultOutput<EditionListDto>(
                editions.MapTo<List<EditionListDto>>()
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Create, AppPermissions.Pages_Editions_Edit)]
        public async Task<GetEditionForEditOutput> GetEditionForEdit(NullableIdInput input)
        {
            var features = FeatureManager.GetAll();

            EditionEditDto editionEditDto;
            List<NameValue> featureValues;

            if (input.Id.HasValue) //Editing existing edition?
            {
                var edition = await _editionManager.FindByIdAsync(input.Id.Value);
                featureValues = (await _editionManager.GetFeatureValuesAsync(input.Id.Value)).ToList();
                editionEditDto = edition.MapTo<EditionEditDto>();
            }
            else
            {
                editionEditDto = new EditionEditDto();
                featureValues = features.Select(f => new NameValue(f.Name, f.DefaultValue)).ToList();
            }

            return new GetEditionForEditOutput
            {
                Edition = editionEditDto,
                Features = features.MapTo<List<FlatFeatureDto>>().OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Create, AppPermissions.Pages_Editions_Edit)]
        public async Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input)
        {
            if (!input.Edition.Id.HasValue)
            {
                await CreateEditionAsync(input);
            }
            else
            {
                await UpdateEditionAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Delete)]
        public async Task DeleteEdition(EntityRequestInput input)
        {
            var edition = await _editionManager.GetByIdAsync(input.Id);
            await _editionManager.DeleteAsync(edition);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Create)]
        protected virtual async Task CreateEditionAsync(CreateOrUpdateEditionDto input)
        {
            var edition = new Edition(input.Edition.DisplayName);

            await _editionManager.CreateAsync(edition);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.

            await SetFeatureValues(edition, input.FeatureValues);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Edit)]
        protected virtual async Task UpdateEditionAsync(CreateOrUpdateEditionDto input)
        {
            Debug.Assert(input.Edition.Id != null, "input.Edition.Id should be set.");

            var edition = await _editionManager.GetByIdAsync(input.Edition.Id.Value);
            edition.DisplayName = input.Edition.DisplayName;

            await SetFeatureValues(edition, input.FeatureValues);
        }

        private Task SetFeatureValues(Edition edition, List<NameValueDto> featureValues)
        {
            return _editionManager.SetFeatureValuesAsync(edition.Id, featureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }
    }
}
