using AutoMapper;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Common.Helper;
using SERP.Application.Finance.Groups.DTOs;
using SERP.Application.Finance.Groups.DTOs.Request;
using SERP.Application.Finance.Groups.DTOs.Response;
using SERP.Application.Finance.Groups.Interfaces;
using SERP.Application.Masters.Lovs.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;
using SERP.Domain.Finance.Groups;
using static SERP.Application.Finance.Groups.DTOs.Request.CreateGroupRequestModel;
using static SERP.Application.Finance.Groups.DTOs.Request.UpdateGroupRequestModel;


namespace SERP.Application.Finance.Groups.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILovRepository _lovRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GroupService(ILovRepository lovRepository, IUnitOfWork unitOfWork, IGroupRepository groupRepository, IMapper mapper, IConfiguration configuration)
        {
            _lovRepository = lovRepository;
            _unitOfWork = unitOfWork;
            _groupRepository = groupRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<PagedResponse<GroupResponseModel>> GetAllPagedAsync(int page, int pageSize, string groupType)
        {
            try
            {
                var pageable = PagingUtilities.GetPageable(page, pageSize);
                var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
                //var groups = (await _groupRepository.Find(x => groupType == x.group_type)).ToList();
                //var sortedGroups = groups.OrderBy(x => x.group_code)
                //                            .ThenBy(y => y.level)
                //                            .ToList();

                var sortedGroups = await _groupRepository.GetGroupPage(groupType);

                List<GroupResponseModel> groupsResponse = _mapper.Map<List<GroupResponseModel>>(sortedGroups);

                var totalRows = sortedGroups.Count();
                if (totalRows == 0)
                {
                    return new PagedResponse<GroupResponseModel>();
                }
                var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);
                var pagedResponse = groupsResponse.Skip(skipRow).Take(pageable.Size).ToList();

                return new PagedResponse<GroupResponseModel>
                {
                    Items = pagedResponse,
                    TotalItems = totalRows,
                    TotalPage = totalPage,
                    Page = pageable.Page,
                    PageSize = pageable.Size
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<PagedResponse<GroupResponseModel>> SearchPagedAsync(int page, int pageSize, string groupType, string keyword, SearchPagedGroupRequestModel request)
        {
            try
            {

                // extract out all list from payload
                //var typeList = request.typeList == null ? null :  request.typeList.Select(x => x.type).ToList();
                var levelList = request.levelList == null ? null : request.levelList.Select(x => x.level).ToList();
                var parentList = request.parentGroupList == null ? null : request.parentGroupList.Select(x => x.group_id).ToList();
                var statusList = request.statusList == null ? null : request.statusList.Select(x => x.status).ToList();

                var pageable = PagingUtilities.GetPageable(page, pageSize);
                var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

                var sortedGroups = await _groupRepository.GroupFilterPaged(request.create_date_from,
                                                                    request.create_date_to,
                                                                    // typeList,
                                                                    levelList,
                                                                    parentList,
                                                                    statusList,
                                                                    keyword,
                                                                    groupType);

                List<GroupResponseModel> groupsResponse = _mapper.Map<List<GroupResponseModel>>(sortedGroups);

                var totalRows = sortedGroups.Count();
                if (totalRows == 0)
                {
                    return new PagedResponse<GroupResponseModel>();
                }
                var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);
                var pagedResponse = groupsResponse.Skip(skipRow).Take(pageable.Size).ToList();

                return new PagedResponse<GroupResponseModel>
                {
                    Items = pagedResponse,
                    TotalItems = totalRows,
                    TotalPage = totalPage,
                    Page = pageable.Page,
                    PageSize = pageable.Size
                };

            }
            catch
            {
                throw;
            }
        }

        public async Task<GroupResponseModel> GetByIdAsync(int id)
        {
            try
            {
                var group = await _groupRepository.GetByIdAsync(x => x.id == id);
                if (group == null)
                    throw new BadRequestException(ErrorCodes.GroupIDNotFound, string.Format(ErrorMessages.GroupIDNotFound, id));
                return _mapper.Map<GroupResponseModel>(group);

            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task CreateGroupAsync(string userId, CreateGroupRequestModel request)
        {
            try
            {
                // Validation Check
                var validator = new CreateGroupRequestModelValidator();
                var validatoResult = await validator.ValidateAsync(request);
                if (validatoResult.Errors.Any())
                    throw new BadRequestException(validatoResult);

                // group_code must not exists in Group table
                var group = await _groupRepository.GetFirst(x => x.group_code == request.group_code);
                if (group != null)
                    throw new BadRequestException(ErrorCodes.GroupCodeAlreadyExist, string.Format( ErrorMessages.GroupCodeAlreadyExist, request.group_code) );

                // check if group_type exist in LOV table with lov_type='GroupType'
                var lov = await _lovRepository.GetFirst(x => x.lov_value == request.group_type);
                if (lov == null)
                    throw new BadRequestException(ErrorCodes.GroupTypeNotFound, ErrorMessages.GroupTypeNotFound);

                /*
                 Company (CO) - max level = 4, parent group id is required if max level <4
                 Natural Account (NA) - max level = 4, parent group id is required if max level <4
                 Cost Center (CC) - max level = 3, parent group id is required if max level <3
                 Revenue Center (RC) - max level = 2, parent group id is required if max level <2
                 */
                validateGroupType(request.group_type, request.level, request.parent_group_id);



                // parent_group_id exists in Group table with same group_type, status flag of E and one level higher
                if (request.parent_group_id.HasValue || request.parent_group_id != null)
                {

                    var parentGroup = await _groupRepository.GetByIdAsync(x => x.id == request.parent_group_id);

                    if (parentGroup == null)
                        throw new BadRequestException(ErrorCodes.ParentGroupIDDontExist, string.Format(ErrorMessages.ParentGroupIDDontExist, request.parent_group_id) );

                    if (parentGroup.group_type != request.group_type)
                        throw new BadRequestException(ErrorCodes.ParentGroupTypeNotTheSame, string.Format(ErrorMessages.ParentGroupTypeNotTheSame, parentGroup.group_type, request.group_type));

                    if (parentGroup.status_flag != DomainConstant.StatusFlag.Enabled)
                        throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, ErrorMessages.ParentStatusFlagNotEnabled);

                    if (parentGroup.level - 1 != request.level)
                        throw new BadRequestException(ErrorCodes.ParentGroupNotOneLevelHigher, string.Format(ErrorMessages.ParentGroupNotOneLevelHigher, parentGroup.level, request.level));
                }

                Group newGroup = new Group()
                {
                    group_code = request.group_code,
                    group_description = request.group_description,
                    group_type = request.group_type,
                    level = request.level,
                    parent_group_id = request.parent_group_id,
                    status_flag = request.status_flag,
                    created_on = DateTime.Now,
                    created_by = userId
                };
                await _groupRepository.CreateAsync(newGroup);
                await _unitOfWork.SaveChangesAsync();
                //_unitOfWork.Commit();

            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateGroupAsync(string userId, List<UpdateGroupRequestModel> requests)
        {
            try
            {
                // Validation Check
                var validator = new UpdateGroupRequestModelValidator();
                foreach(var request in requests)
                {
                    var validatorResult = await validator.ValidateAsync(request);
                    if (validatorResult.Errors.Any())
                        throw new BadRequestException(validatorResult);
                    
                }

                // check for duplicated id
                var group_By_Id = requests.GroupBy(x => x.id)
                       .Where(x => x.Count() > 1)
                       .Select(x => x.Key)
                       .ToList();

                if (group_By_Id.Count() > 0)
                    throw new BadRequestException(ErrorMessages.DuplicatedGroupId);

                //// check for duplicated group_code
                //var group_By_GroupCode = requests.GroupBy(x => x.group_code)
                //        .Where(x => x.Count() > 1)
                //        .Select(x => x.Key)
                //        .ToList();

                //if (group_By_GroupCode.Count() > 0)
                //    throw new BadRequestException(ErrorMessages.DuplicatedGroupCode);

                List<Group> groupToUpdate = new List<Group>();

                foreach(var request in requests)
                {
                    // group_code must not exists in Group table
                    var group_code = await _groupRepository.GetFirst(x => x.group_code == request.group_code && x.id != request.id);
                    if (group_code != null)
                        throw new BadRequestException(ErrorCodes.GroupCodeAlreadyExist, string.Format(ErrorMessages.GroupCodeAlreadyExist, request.group_code));

                    var group = await _groupRepository.GetByIdAsync(x => x.id == request.id);
                    if (group == null)
                        throw new BadRequestException(ErrorCodes.GroupIDNotFound, string.Format(ErrorMessages.GroupIDNotFound, request.id) );

                    /*
                     Company (CO) - max level = 4, parent group id is required if max level <4
                     Natural Account (NA) - max level = 4, parent group id is required if max level <4
                     Cost Center (CC) - max level = 3, parent group id is required if max level <3
                     Revenue Center (RC) - max level = 2, parent group id is required if max level <2
                    */
                    validateGroupType(group.group_type, request.level, request.parent_group_id);

                    // parent_group_id exists in Group table with same group_type, status flag of E and one level higher
                    if (request.parent_group_id.HasValue || request.parent_group_id != null)
                    {

                        var parentGroup = await _groupRepository.GetByIdAsync(x => x.id == request.parent_group_id);

                        if (parentGroup == null)
                            throw new BadRequestException(ErrorCodes.ParentGroupIDDontExist, string.Format( ErrorMessages.ParentGroupIDDontExist, request.parent_group_id) );

                        if (parentGroup.group_type != group.group_type)
                            throw new BadRequestException(ErrorCodes.ParentGroupTypeNotTheSame, string.Format(ErrorMessages.ParentGroupTypeNotTheSame, parentGroup.group_type, group.group_type));

                        if (parentGroup.status_flag != DomainConstant.StatusFlag.Enabled)
                            throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, ErrorMessages.ParentStatusFlagNotEnabled);

                        if (parentGroup.level - 1 != request.level)
                            throw new BadRequestException(ErrorCodes.ParentGroupNotOneLevelHigher, string.Format(ErrorMessages.ParentGroupNotOneLevelHigher, parentGroup.level, request.level));
                    }

                    group.group_code = request.group_code;
                    group.group_description = request.group_description;
                    group.level = request.level;
                    group.parent_group_id = request.parent_group_id;
                    group.status_flag = request.status_flag;
                    group.last_modified_by = userId;
                    group.last_modified_on = DateTime.Now;

                    groupToUpdate.Add(group);
                }

                await _groupRepository.UpdateRangeAsync(groupToUpdate);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteGroupAsync(DeleteGroupRequestModel request)
        {
            try
            {

                _unitOfWork.BeginTransaction();

                // Validation Check
                var validator = new DeleteGroupRequestModelValidator();
                var validatoResult = await validator.ValidateAsync(request);
                if (validatoResult.Errors.Any())
                    throw new BadRequestException(validatoResult);

                var validatorList = new DeleteGroupIdValidator();
                foreach (var r in request.groupList)
                {
                    var validatorResult = await validatorList.ValidateAsync(r);
                    if (validatorResult.Errors.Any())
                        throw new BadRequestException(validatorResult);

                }

                var requestGroupIds = request.groupList.Select(x => x.id).ToList();

                var groupList = await _groupRepository.Find(x => requestGroupIds.Contains(x.id) );
                groupList = groupList.ToList();

                var notFoundId = requestGroupIds.Except(groupList.Select(x => x.id)).ToList();
                if(notFoundId.Any() || notFoundId.Count() != 0)
                    throw new BadRequestException(ErrorCodes.DeleteGroupIsParent, string.Format(ErrorMessages.GroupIDNotFound, string.Join(", ", notFoundId ) ) );

                var parentGroupList = await _groupRepository.GetParentIdList(requestGroupIds);
                parentGroupList = parentGroupList.ToList();

                if (parentGroupList.Any() || parentGroupList.Count() != 0)
                    throw new BadRequestException(ErrorCodes.DeleteGroupIsParent, string.Format(ErrorMessages.DeleteGroupIsParent, string.Join(", ", parentGroupList) ) );

                await _groupRepository.DeleteRangeAsync(groupList);

            }
            catch(Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();
        }

        public async Task ImportCompanyGroupAsync(string userId, ImportCompanyGroupModel request)
        {
            string localFilePath = "";
            try
            {
                if (!FileHelper.IsExcelFile(request.File.ContentType))
                    throw new BadRequestException(ErrorCodes.NotExcelFile, ErrorMessages.NotExcelFile);


                if(request.File.Length > ApplicationConstant.Settings.MaximumFileSize)
                    throw new BadRequestException(ErrorCodes.ExceedMaximumFileSize, ErrorMessages.ExceedMaximumFileSize);

                var currentTime = DateTime.Now;
                localFilePath = $"App_Data/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";
                await FileHelper.SaveFileToTemporaryFolder(request.File, request.File.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, request.File.FileName);

                List<GroupExcelDto> excelData = await ReadExcelDataGroup(inputFilePath, DomainConstant.Group.GroupType.Company);
                List<Group> updateGroup = await validateExcelData(excelData, "Group", DomainConstant.Group.GroupTypeMaxLevel.Company, userId, DomainConstant.Group.GroupType.Company);

                if(updateGroup.Count > 0)
                {
                    await _groupRepository.UpdateRangeAsync(updateGroup);
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if(!string.IsNullOrEmpty(localFilePath))
                    Directory.Delete(localFilePath, true);
            }
        }

        public async Task ImportCostCenterGroupAsync(string userId, ImportCompanyGroupModel request)
        {
            string localFilePath = "";
            try
            {
                if (!FileHelper.IsExcelFile(request.File.ContentType))
                    throw new BadRequestException(ErrorCodes.NotExcelFile, ErrorMessages.NotExcelFile);


                if (request.File.Length > ApplicationConstant.Settings.MaximumFileSize)
                    throw new BadRequestException(ErrorCodes.ExceedMaximumFileSize, ErrorMessages.ExceedMaximumFileSize);

                var currentTime = DateTime.Now;
                localFilePath = $"App_Data/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";
                await FileHelper.SaveFileToTemporaryFolder(request.File, request.File.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, request.File.FileName);

                List<GroupExcelDto> excelData = await ReadExcelDataGroup(inputFilePath, DomainConstant.Group.GroupType.CostCenter);
                List<Group> updateGroup = await validateExcelData(excelData, "Group", DomainConstant.Group.GroupTypeMaxLevel.CostCenter, userId, DomainConstant.Group.GroupType.CostCenter);

                if (updateGroup.Count > 0)
                    await _groupRepository.UpdateRangeAsync(updateGroup);

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!string.IsNullOrEmpty(localFilePath))
                    Directory.Delete(localFilePath, true);
            }
        }

        public async Task ImportRevenueCenterGroupAsync(string userId, ImportCompanyGroupModel request)
        {
            string localFilePath = "";
            try
            {
                if (!FileHelper.IsExcelFile(request.File.ContentType))
                    throw new BadRequestException(ErrorCodes.NotExcelFile, ErrorMessages.NotExcelFile);


                if (request.File.Length > ApplicationConstant.Settings.MaximumFileSize)
                    throw new BadRequestException(ErrorCodes.ExceedMaximumFileSize, ErrorMessages.ExceedMaximumFileSize);

                var currentTime = DateTime.Now;
                localFilePath = $"App_Data/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";
                await FileHelper.SaveFileToTemporaryFolder(request.File, request.File.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, request.File.FileName);

                List<GroupExcelDto> excelData = await ReadExcelDataGroup(inputFilePath, DomainConstant.Group.GroupType.RevenueCenter);
                List<Group> updateGroup = await validateExcelData(excelData, "Group", DomainConstant.Group.GroupTypeMaxLevel.RevenueCenter, userId, DomainConstant.Group.GroupType.RevenueCenter);

                if (updateGroup.Count > 0)
                    await _groupRepository.UpdateRangeAsync(updateGroup);

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!string.IsNullOrEmpty(localFilePath))
                    Directory.Delete(localFilePath, true);
            }
        }

        public async Task ImportNaturalAccountGroupAsync(string userId, ImportCompanyGroupModel request)
        {
            string localFilePath = "";
            try
            {
                if (!FileHelper.IsExcelFile(request.File.ContentType))
                    throw new BadRequestException(ErrorCodes.NotExcelFile, ErrorMessages.NotExcelFile);


                if (request.File.Length > ApplicationConstant.Settings.MaximumFileSize)
                    throw new BadRequestException(ErrorCodes.ExceedMaximumFileSize, ErrorMessages.ExceedMaximumFileSize);

                var currentTime = DateTime.Now;
                localFilePath = $"App_Data/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";
                await FileHelper.SaveFileToTemporaryFolder(request.File, request.File.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, request.File.FileName);

                List<GroupExcelDto> excelData = await ReadExcelDataGroup(inputFilePath, DomainConstant.Group.GroupType.NaturalAccount);
                List<Group> updateGroup = await validateExcelData(excelData, "Group", DomainConstant.Group.GroupTypeMaxLevel.NaturalAccount, userId, DomainConstant.Group.GroupType.NaturalAccount);

                if (updateGroup.Count > 0)
                    await _groupRepository.UpdateRangeAsync(updateGroup);

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!string.IsNullOrEmpty(localFilePath))
                    Directory.Delete(localFilePath, true);
            }
        }

        public async Task<GroupFilterByGroupType> GetGroupListByGroupTypeAsync(string groupType)
        {
            try
            {
                List<Group> groupList = await _groupRepository.GetGroupByGroupType(groupType);

                GroupFilterByGroupType response = new GroupFilterByGroupType();
                response.items = new List<GroupFilter>();

                foreach (Group group in groupList)
                {
                    response.items.Add(new GroupFilter()
                    {
                        id = group.id,
                        group_code = group.group_code,
                        group_description = group.group_description,
                        level = group.level,
                    });
                }
                return response;

            }
            catch
            {
                throw;
            }
        }

        public async Task<GroupTypeParentGroupModel> GetParentGroupListByGroupTypeAsync(string groupType)
        {
            try
            {
                List<Group> groupList = await _groupRepository.GetParentGroupList(groupType);
                GroupTypeParentGroupModel response = new GroupTypeParentGroupModel();
                response.items = new List<ParentGrouping>();

                foreach(Group group in groupList)
                {
                    response.items.Add(new ParentGrouping
                    {
                        id = group.id,
                        group_code = group.group_code,
                        group_description = group.group_description,
                    });
                }
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(byte[], string)> GetGroupTemplate()
        {
            try
            {
                string filePath = _configuration["GroupReport:FilePath"];
                string fileName = _configuration["GroupReport:FileName"];
                string fileLocation = filePath + fileName;

                if (string.IsNullOrEmpty(fileLocation) || !File.Exists(fileLocation))
                    throw new BadRequestException(ErrorCodes.FileNotFound, ErrorMessages.FileNotFound);

                byte[] fileBytes = await File.ReadAllBytesAsync(fileLocation);
                return(fileBytes, fileName);

            }
            catch
            {
                throw;
            }
        }

        private async Task<List<GroupExcelDto>> ReadExcelDataGroup(string inputFilePath, string groupType)
        {
            var execelData = new List<GroupExcelDto>();
            await using (var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var rowIndex = 1;
                    reader.Read();

                    while (reader.Read())
                    {
                        if (rowIndex != 0)
                        {
                            GroupExcelDto rowData = new GroupExcelDto();

                            rowData.code = reader.IsDBNull(0) ? null : reader.GetValue(0).ToString();
                            rowData.description = reader.IsDBNull(1) ? null : reader.GetValue(1).ToString();
                            rowData.level = reader.IsDBNull(2) ? null : int.Parse(reader.GetValue(2).ToString());
                            rowData.parentGroup = reader.IsDBNull(3) ? null : reader.GetValue(3).ToString();
                            rowData.status = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString();
                            rowData.groupType = groupType;

                            execelData.Add(rowData);
                        }
                        rowIndex++;
                    }
                }
            }
            return execelData;
        }

        private void validateGroupType(string group_type, int level, int? parent_group_id)
        {
            switch (group_type)
            {
                case DomainConstant.Group.GroupType.Company: 

                    if (level > DomainConstant.Group.GroupTypeMaxLevel.Company)
                        throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, string.Format(ErrorMessages.GroupLevelExceedMaxGroupLevel, level, 
                            DomainConstant.Group.GroupTypeMaxLevel.Company, 
                            nameof(DomainConstant.Group.GroupTypeMaxLevel.Company)));

                    if (level < DomainConstant.Group.GroupTypeMaxLevel.Company && (!parent_group_id.HasValue || parent_group_id == null) )
                        throw new BadRequestException(ErrorCodes.ParentGroupIdNull, ErrorMessages.ParentGroupIdNull);
                    


                    break;

                case DomainConstant.Group.GroupType.NaturalAccount: 

                    if (level > DomainConstant.Group.GroupTypeMaxLevel.NaturalAccount)
                        throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, string.Format(ErrorMessages.GroupLevelExceedMaxGroupLevel, level, 
                            DomainConstant.Group.GroupTypeMaxLevel.NaturalAccount, 
                            nameof(DomainConstant.Group.GroupTypeMaxLevel.NaturalAccount)));

                    if (level < DomainConstant.Group.GroupTypeMaxLevel.NaturalAccount && (!parent_group_id.HasValue || parent_group_id == null) )
                        throw new BadRequestException(ErrorCodes.ParentGroupIdNull, ErrorMessages.ParentGroupIdNull);

                    break;

                case DomainConstant.Group.GroupType.CostCenter:

                    if (level > DomainConstant.Group.GroupTypeMaxLevel.CostCenter)
                        throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, string.Format(ErrorMessages.GroupLevelExceedMaxGroupLevel, level, 
                            DomainConstant.Group.GroupTypeMaxLevel.CostCenter, 
                            nameof(DomainConstant.Group.GroupTypeMaxLevel.CostCenter)));

                    if (level < DomainConstant.Group.GroupTypeMaxLevel.CostCenter && ( !parent_group_id.HasValue || parent_group_id == null) )
                        throw new BadRequestException(ErrorCodes.ParentGroupIdNull, ErrorMessages.ParentGroupIdNull);

                    break;

                case DomainConstant.Group.GroupType.RevenueCenter:

                    if (level > DomainConstant.Group.GroupTypeMaxLevel.RevenueCenter)
                        throw new BadRequestException(ErrorCodes.ParentStatusFlagNotEnabled, string.Format(ErrorMessages.GroupLevelExceedMaxGroupLevel, level, 
                            DomainConstant.Group.GroupTypeMaxLevel.RevenueCenter, 
                            nameof(DomainConstant.Group.GroupTypeMaxLevel.RevenueCenter)));

                    if (level < DomainConstant.Group.GroupTypeMaxLevel.RevenueCenter && ( !parent_group_id.HasValue || parent_group_id == null) )
                        throw new BadRequestException(ErrorCodes.ParentGroupIdNull, ErrorMessages.ParentGroupIdNull);

                    break;

                default:
                    throw new BadRequestException(ErrorCodes.GroupTypeNotSupport, ErrorMessages.GroupTypeNotSupported);

            }
        }

        private async Task<List<Group>> validateExcelData(List<GroupExcelDto> excelData, string groupType, int maxLevel, string userId, string type)
        {
            try
            {

                int row = 1;
                var parentGroup = excelData.Select(x => x.parentGroup)
                                           .ToHashSet()
                                           .ToList();

                var parentGroupIdDictionary = await _groupRepository.GetGroupDictionary(parentGroup, type);

                var groups = excelData.Select(x => x.code)
                                      .ToHashSet()
                                      .ToList();

                var groupDictionary = await _groupRepository.GetGroupDictionary(groups, type);

                List<Group> createList = new List<Group>();
                List<Group> updateList = new List<Group>();

                foreach (var data in excelData)
                {
                    // Reject if group_code, group_description, level or status_flag is empty
                    if (data.code is null || data.code == null)
                        throw new BadRequestException(ErrorCodes.ExcelDataEmptyCode, string.Format(ErrorMessages.ExcelDataEmptyCode, groupType, row + 1));

                    if (data.description is null || data.description == null)
                        throw new BadRequestException(ErrorCodes.ExcelDataEmptyDescription, string.Format(ErrorMessages.ExcelDataEmptyDescription, groupType, row + 1));


                    if (data.level is null || data.level == null)
                        throw new BadRequestException(ErrorCodes.ExcelDataEmptyLevel, string.Format(ErrorMessages.ExcelDataEmptyLevel, groupType, row + 1));

                    if (data.status is null || data.status == null)
                    {
                        throw new BadRequestException(ErrorCodes.ExcelDataEmptyStatus, string.Format(ErrorMessages.ExcelDataEmptyStatus, groupType, row + 1));
                    }

                    // Reject if level more than max group level (4)
                    if(data.level > maxLevel)
                    {
                        throw new BadRequestException(ErrorCodes.RequestMaxLevel, string.Format( ErrorMessages.RequestMaxLevel, data.level, maxLevel ) );
                    }

                    // Reject if level below 4 and parent group id is empty (inserting a parent that is not a max level)
                    if ((data.parentGroup is null || data.parentGroup == null) && data.level < maxLevel)
                    {
                        throw new BadRequestException(ErrorCodes.ExcelDataEmptyParentGroup, string.Format(ErrorMessages.ExcelDataEmptyParentGroup, groupType, row + 1));
                    }

                    // Reject if level is 4 and parent_group_id is not empty (inserting data that has parent but is a parent itself)
                    if ((data.parentGroup is not null || data.parentGroup != null) && data.level == maxLevel)
                    {
                        throw new BadRequestException(ErrorCodes.ExcelParentNotEmptyButMaxLevel, string.Format(ErrorMessages.ExcelParentNotEmptyButMaxLevel, row + 1));
                    }

                    // Reject if parent_group_id is not empty and parent_group_id not exists in Group table (trying to insert a parent that does not exists)
                    if ((data.parentGroup is not null || data.parentGroup != null) && !parentGroupIdDictionary.Keys.Contains(data.parentGroup))
                    {
                        throw new BadRequestException(ErrorCodes.ParentGroupIDNotExistInExcel, string.Format(ErrorMessages.ParentGroupIDNotExistInExcel, data.parentGroup, row + 1));
                    }

                    if ((data.parentGroup is not null || data.parentGroup != null) && parentGroupIdDictionary.Keys.Contains(data.parentGroup) && parentGroupIdDictionary[data.parentGroup].level - 1 != data.level)
                        throw new BadRequestException(ErrorCodes.ParentGroupLevelErrorInExcel, string.Format(ErrorMessages.ParentGroupLevelErrorInExcel, row + 1));

                    // Reject if parent_group_id exist in Group table but different group_type, status flag of D or level is not 1 level higher
                    
                    //string parentGroupType = parentGroupIdDictionary[data.parentGroup].group_type;
                    //string parentStatusFlag = parentGroupIdDictionary[data.parentGroup].status_flag;
                    //int parentlevel = parentGroupIdDictionary[data.parentGroup].level;

                    if ((data.parentGroup is not null || data.parentGroup != null) && data.groupType != parentGroupIdDictionary[data.parentGroup].group_type && !groups.Contains(data.parentGroup) )
                    {
                        //throw new BadRequestException(ErrorCodes.ParentGroupIDNotExistInExcel, string.Format(ErrorMessages.ParentGroupTypeErrorInExcel, parentGroupType, data.groupType, row));
                        if (parentGroupIdDictionary[data.parentGroup].status_flag == DomainConstant.StatusFlag.Disabled)
                        {
                            throw new BadRequestException(ErrorCodes.ParentGroupFlagErrorInExcel, string.Format(ErrorMessages.ParentGroupFlagErrorInExcel, row + 1));
                        }

                        if (data.level != parentGroupIdDictionary[data.parentGroup].level - 1)
                        {
                            throw new BadRequestException(ErrorCodes.ParentGroupLevelErrorInExcel, string.Format(ErrorMessages.ParentGroupLevelErrorInExcel, row + 1));
                        }
                    }


                    // Reject if status_flag is not E and D
                    if (data.status != DomainConstant.StatusFlag.Enabled && data.status != DomainConstant.StatusFlag.Disabled)
                    {
                        throw new BadRequestException(ErrorCodes.RequestGroupFlagErrorInExcel, string.Format(ErrorMessages.RequestGroupFlagErrorInExcel, row + 1));
                    }

                    /*
                     if group_code is exist in Group table and is parent group of another group

                     Reject if level is modified
                     Reject if status_flag is change to disabled
                     */
                    if ( (data.parentGroup is not null || data.parentGroup != null) && data.groupType != parentGroupIdDictionary[data.parentGroup].group_type && groupDictionary.ContainsKey(data.code) )
                    {
                        if (groupDictionary[data.code].level != data.level)
                            throw new BadRequestException(ErrorCodes.RequestGroupLevelModification, string.Format(ErrorMessages.RequestGroupLevelModification, row + 1));

                        if (data.status == DomainConstant.StatusFlag.Disabled)
                            throw new BadRequestException(ErrorCodes.RequestGroupStatusFlagModification, string.Format(ErrorMessages.RequestGroupStatusFlagModification, DomainConstant.StatusFlag.Disabled, row + 1));
                    }

                    if (groupDictionary.ContainsKey(data.code))
                    {
                        var group = groupDictionary[data.code];
                        group.group_code = data.code;
                        group.group_description = data.description;
                        group.group_type = data.groupType;
                        group.level = data.level.Value;
                        group.parent_group_id =  (data.parentGroup is null || data.parentGroup == null) ? null : parentGroupIdDictionary[data.parentGroup].id;
                        group.status_flag = data.status;
                        group.last_modified_on = DateTime.Now;
                        group.last_modified_by = userId;
                        updateList.Add(group);
                    }

                    else
                    {
                        await _groupRepository.CreateAsync(
                            new Group()
                            {
                                group_code = data.code,
                                group_description = data.description,
                                group_type = data.groupType,
                                level = data.level.Value,
                                parent_group_id = (data.parentGroup is null || data.parentGroup == null) ? null : parentGroupIdDictionary[data.parentGroup].id,
                                status_flag = data.status,
                                created_on = DateTime.Now,
                                created_by = userId
                            }
                         );
                        await _unitOfWork.SaveChangesAsync();
                        parentGroupIdDictionary = await _groupRepository.GetGroupDictionary(parentGroup, type);

                        //createList.Add(new Group()
                        //{
                        //    group_code = data.code,
                        //    group_description = data.description,
                        //    group_type = data.groupType,
                        //    level = data.level.Value,
                        //    parent_group_id = (data.parentGroup is null || data.parentGroup == null) ? null : parentGroupIdDictionary[data.parentGroup].id,
                        //    status_flag = data.status,
                        //    created_on = DateTime.Now,
                        //    created_by = userId
                        //});
                    }

                    row++;
                }
                return  updateList;

            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    }
}
