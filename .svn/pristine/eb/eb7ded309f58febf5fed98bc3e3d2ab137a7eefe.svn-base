using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SERP.Application.Common;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.SystemKVSs.Interfaces;

namespace SERP.Application.Masters.SystemKVSs.Services
{
    internal class SystemKvsService : ISystemKvsService
    {
        private readonly IMapper _mapper;
        private readonly ISystemKvsRepository _systemKvsRepository;

        public SystemKvsService(
            IMapper mapper,
            ISystemKvsRepository systemKvsRepository)
        {
            _mapper = mapper;
            _systemKvsRepository = systemKvsRepository;
        }

        public async Task ValidateFileUploadAsync(List<IFormFile> files)
        {
            var fileLimited = await _systemKvsRepository.GetFileUploadLimited();

            if (fileLimited == null)
            {
                return;
            }

            var validationErrors = Utilities.ValidateFiles(files, fileLimited.FileSizeLimit, fileLimited.AllowedFileExtension);

            if (validationErrors.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in validationErrors)
                {
                    stringBuilder.AppendLine(error);
                }

                throw new BadRequestException(stringBuilder.ToString());
            }

        }
    }
}
