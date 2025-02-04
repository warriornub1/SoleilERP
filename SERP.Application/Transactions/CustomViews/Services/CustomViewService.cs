using AutoMapper;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Transactions.CustomViews.DTOs.Request;
using SERP.Application.Transactions.CustomViews.DTOs.Response;
using SERP.Application.Transactions.CustomViews.Interfaces;
using SERP.Domain.Common.Model;
using SERP.Domain.Transactions.CustomViews;
using SERP.Domain.Transactions.CustomViews.Model;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Transactions.CustomViews.Services
{
    internal class CustomViewService : ICustomViewService
    {
        private readonly ICustomViewRepository _customViewRepository;
        private readonly ICustomViewAttributeRepository _customViewAttributeRepository;
        private readonly ICustomViewFilterRepository _customViewFilterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomViewService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CustomViewService(
            ICustomViewRepository customViewRepository,
            ICustomViewAttributeRepository customViewAttributeRepository,
            ICustomViewFilterRepository customViewFilterRepository,
            IMapper mapper,
            ILogger<CustomViewService> logger,
            IUnitOfWork unitOfWork)
        {
            _customViewRepository = customViewRepository;
            _customViewAttributeRepository = customViewAttributeRepository;
            _customViewFilterRepository = customViewFilterRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CustomViewResponseDto>> GetByCustomViewType(string customViewType, string? userId, bool onlyEnabled)
        {
            return _mapper.Map<List<CustomViewResponseDto>>(await _customViewRepository.GetByCustomViewType(customViewType, userId, onlyEnabled));
        }

        public async Task<CustomViewAttributeResponseDto?> GetAttributesByCustomViewId(int customViewId)
        {
            return _mapper.Map<CustomViewAttributeResponseDto>(await _customViewRepository.GetAttributesByCustomViewId(customViewId));
        }

        public async Task<int> CreateCustomView(string userId, CreateCustomViewRequestDto request)
        {
            // TODO: Add validation for customView attribute
            if (await _customViewRepository.IsCustomViewNameExist(request.custom_view_name, request.custom_view_type, request.user_id))
            {
                throw new BadRequestException(ErrorCodes.CustomViewAlreadyExist, string.Format(ErrorMessages.CustomViewAlreadyExist, request.custom_view_name));
            }

            var customView = _mapper.Map<CustomView>(request);
            customView.created_by = userId;
            customView.created_on = DateTime.Now;
            try
            {
                _unitOfWork.BeginTransaction();
                await _customViewRepository.CreateAsync(customView);
                await _unitOfWork.SaveChangesAsync();

                var customViewAttribute = _mapper.Map<List<CustomViewAttribute>>(request.attributes);
                customViewAttribute.ForEach(x =>
                {
                    x.custom_view_id = customView.id;
                    x.created_by = userId;
                    x.created_on = DateTime.Now;
                });

                if (customViewAttribute.Count > 0)
                {
                    await _customViewAttributeRepository.CreateRangeAsync(customViewAttribute);
                }

                if (request.filters is not null)
                {
                    var customViewFilter = _mapper.Map<List<CustomViewFilter>>(request.filters);
                    customViewFilter.ForEach(x =>
                    {
                        x.custom_view_id = customView.id;
                        x.created_by = userId;
                        x.created_on = DateTime.Now;
                    });

                    if (customViewFilter.Count > 0)
                    {
                        await _customViewFilterRepository.CreateRangeAsync(customViewFilter);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                _logger.LogError(e, e.Message);
                throw;
            }

            return customView.id;
        }

        public async Task UpdateCustomViewAttributes(string userId, UpdateCustomViewAttributeRequestDto request)
        {
            var existedCustomView = await _customViewRepository.GetByIdAsync(x => x.id == request.id);

            if (existedCustomView == null)
            {
                throw new NotFoundException(ErrorCodes.CustomViewNotFound, string.Format(ErrorMessages.CustomViewNotFound, request.id));
            }

            //_mapper.Map(request, existedCustomView);
            //existedCustomView.last_modified_by = userId;
            //existedCustomView.last_modified_on = DateTime.Now;

            var customViewAttributeEntities = await _customViewAttributeRepository.GetByCustomViewId([request.id]);
            var customViewFilterEntities = await _customViewFilterRepository.GetByCustomViewId([request.id]);

            var customViewAttributeToCreate = new List<CustomViewAttribute>();
            var customViewAttributeToUpdate = new List<CustomViewAttribute>();
            var customViewAttributeToDelete = new List<CustomViewAttribute>();

            var customViewFilterToCreate = new List<CustomViewFilter>();
            var customViewFilterToUpdate = new List<CustomViewFilter>();
            var customViewFilterToDelete = new List<CustomViewFilter>();

            foreach (var requestModel in request.attributes)
            {
                var customViewAttribute = customViewAttributeEntities.Find(x => x.id == requestModel.custom_view_attribute_id);
                switch (requestModel.action)
                {
                    // create
                    case "C":
                        {
                            if (customViewAttributeEntities.Any(x => x.attribute == requestModel.attribute))
                            {
                                throw new BadRequestException(ErrorCodes.CustomViewAttributeAlreadyExist, string.Format(ErrorMessages.CustomViewAttributeAlreadyExist, requestModel.attribute));
                            }

                            customViewAttribute = _mapper.Map<CustomViewAttribute>(requestModel);
                            customViewAttribute.custom_view_id = request.id;
                            customViewAttribute.created_by = userId;
                            customViewAttributeToCreate.Add(customViewAttribute);
                            break;
                        }
                    // update
                    case "U":
                        {
                            if (customViewAttribute is null)
                            {
                                throw new BadRequestException(ErrorCodes.CustomViewAttributeNotFound, string.Format(ErrorMessages.CustomViewAttributeNotFound, requestModel.custom_view_attribute_id));
                            }

                            _mapper.Map(requestModel, customViewAttribute);
                            customViewAttribute.last_modified_by = userId;
                            customViewAttribute.last_modified_on = DateTime.Now;
                            customViewAttributeToUpdate.Add(customViewAttribute);
                            break;
                        }
                    // delete
                    case "D":
                        {
                            if (customViewAttribute is null)
                            {
                                throw new BadRequestException(ErrorCodes.CustomViewAttributeNotFound, string.Format(ErrorMessages.CustomViewAttributeNotFound, requestModel.custom_view_attribute_id));
                            }

                            customViewAttributeToDelete.Add(customViewAttribute);
                            break;
                        }
                }
            }

            if (request.filters is not null)
            {
                foreach (var requestModel in request.filters)
                {
                    var customViewFilter = customViewFilterEntities.Find(x => x.id == requestModel.custom_view_filter_id);
                    switch (requestModel.action)
                    {
                        // create
                        case "C":
                            {
                                if (customViewFilterEntities.Any(x => x.filter == requestModel.filter))
                                {
                                    throw new BadRequestException(ErrorCodes.CustomViewFilterAlreadyExist, string.Format(ErrorMessages.CustomViewFilterAlreadyExist, requestModel.filter));
                                }

                                customViewFilter = _mapper.Map<CustomViewFilter>(requestModel);
                                customViewFilter.custom_view_id = request.id;
                                customViewFilter.created_by = userId;
                                customViewFilterToCreate.Add(customViewFilter);
                                break;
                            }
                        // update
                        case "U":
                            {
                                if (customViewFilter is null)
                                {
                                    throw new BadRequestException(ErrorCodes.CustomViewFilterNotFound, string.Format(ErrorMessages.CustomViewFilterNotFound, requestModel.custom_view_filter_id));
                                }

                                _mapper.Map(requestModel, customViewFilter);
                                customViewFilter.last_modified_by = userId;
                                customViewFilter.last_modified_on = DateTime.Now;
                                customViewFilterToUpdate.Add(customViewFilter);
                                break;
                            }

                        // delete
                        case "D":
                            {
                                if (customViewFilter is null)
                                {
                                    throw new BadRequestException(ErrorCodes.CustomViewFilterNotFound, string.Format(ErrorMessages.CustomViewFilterNotFound, requestModel.custom_view_filter_id));
                                }

                                customViewFilterToDelete.Add(customViewFilter);
                                break;
                            }
                    }
                }
            }

            try
            {
                _unitOfWork.BeginTransaction();
                //await _customViewRepository.UpdateAsync(existedCustomView);

                if (customViewAttributeToCreate.Count > 0)
                {
                    await _customViewAttributeRepository.CreateRangeAsync(customViewAttributeToCreate);
                }

                if (customViewAttributeToUpdate.Count > 0)
                {
                    await _customViewAttributeRepository.UpdateRangeAsync(customViewAttributeToUpdate);
                }

                if (customViewAttributeToDelete.Count > 0)
                {
                    await _customViewAttributeRepository.DeleteRangeAsync(customViewAttributeToDelete);
                }

                if (customViewFilterToCreate.Count > 0)
                {
                    await _customViewFilterRepository.CreateRangeAsync(customViewFilterToCreate);
                }

                if (customViewFilterToUpdate.Count > 0)
                {
                    await _customViewFilterRepository.UpdateRangeAsync(customViewFilterToUpdate);
                }

                if (customViewFilterToDelete.Count > 0)
                {
                    await _customViewFilterRepository.DeleteRangeAsync(customViewFilterToDelete);
                }

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                _logger.LogError(e, e.Message);
                throw;
            }

        }

        public async Task DeleteCustomView(List<int> customViewIds)
        {
            var customViews = await GetCustomViewListAsync(customViewIds);
            var invalidCustomViewIds = customViewIds.Except(customViews.Select(y => y.id)).ToArray();
            if (invalidCustomViewIds.Length > 0)
            {
                throw new BadRequestException(ErrorCodes.CustomViewNotFound, string.Format(ErrorMessages.CustomViewNotFound, string.Join(",", invalidCustomViewIds)));
            }

            try
            {
                var customViewAttributes = await _customViewAttributeRepository.GetByCustomViewId(customViewIds);
                var customViewFilters = await _customViewFilterRepository.GetByCustomViewId(customViewIds);

                _unitOfWork.BeginTransaction();
                await _customViewAttributeRepository.DeleteRangeAsync(customViewAttributes);
                await _customViewFilterRepository.DeleteRangeAsync(customViewFilters);
                await _customViewRepository.DeleteRangeAsync(customViews);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task UpdateCustomView(string userId, List<UpdateCustomViewRequestDto> requests)
        {
            var listIds = requests.Select(x => x.id).ToList();
            var customViews = await GetCustomViewListAsync(listIds);
            if (customViews is null)
            {
                throw new BadRequestException(ErrorCodes.CustomViewNotFound, string.Format(ErrorMessages.CustomViewNotFound, string.Join(",", listIds)));
            }
            foreach (var request in requests)
            {
                var customView = customViews.Find(x => x.id == request.id);
                if (customView is null)
                {
                    throw new BadRequestException(ErrorCodes.CustomViewNotFound, string.Format(ErrorMessages.CustomViewNotFound, request.id));
                }

                _mapper.Map(request, customView);
                customView.last_modified_by = userId;
                customView.last_modified_on = DateTime.Now;
            }

            await _customViewRepository.UpdateRangeAsync(customViews);
            await _unitOfWork.SaveChangesAsync();
        }

        public PagedResponse<CustomViewPagedResponseDto> SearchPaged(SearchPagedRequestDto request, FilterCustomViewPagedRequestDto filter)
        {
            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            var query = _customViewRepository.BuildFilterCustomViewQuery(new PagedFilterCustomViewRequestModel
            {
                Keyword = request.Keyword,
                custom_view_type = filter.custom_view_type,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.CustomView,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<PageCustomViewResponseDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return new PagedResponse<CustomViewPagedResponseDto>();
            }

            var totalPage = (int)Math.Ceiling(totalRows / (request.PageSize * 1.0));
            var pagedResponse = orderBy(query).Skip(skipRow).Take(pageable.Size).ToList();

            return new PagedResponse<CustomViewPagedResponseDto>
            {
                Items = _mapper.Map<List<CustomViewPagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }

        #region Private Methods
        private async Task<List<CustomView>> GetCustomViewListAsync(List<int> customViewIds)
        {
            var customViews = await _customViewRepository.Find(x => customViewIds.Contains(x.id));
            return customViews.ToList();
        }
        #endregion
    }
}
