using ApiIntegration;
using Infrastructure.ApiClients.Interfaces;
using Infrastructure.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<BaseApiClient> logger) : base(httpClientFactory, configuration, logger)
        {
        }

        public async Task<UserDto> GetMemberById(int id)
        {
            var response = await GetAsync<BaseResponseModel>(SystemConstants.UserService, $"/api/member/{id}");
            var result = JsonConvert.DeserializeObject<UserDto>(response.XData.ToString());
            return result;
        }

        public async Task<IEnumerable<UserDto>> GetMembers()
        {
            var response = await GetAsync<BaseResponseModel>(SystemConstants.UserService, $"/api/member");
            var result = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(response.XData.ToString());
            return result;
        }
        public IEnumerable<UserDto> GetMembersSync()
        {
            var response = GetSync<BaseResponseModel>(SystemConstants.UserService, $"/api/member");
            var result = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(response.XData.ToString());
            return result;
        }
        public async Task<UserDto> UpdateMember(int id, UpdateUserDto userDto)
        {
            var response = await PutAsync<BaseResponseModel, UpdateUserDto>(SystemConstants.UserService, $"/api/member/{id}", userDto);
            var result = JsonConvert.DeserializeObject<UserDto>(response.XData.ToString());
            return result;
        }
        public UserDto UpdateMemberSync(int id, UpdateUserDto userDto)
        {
            var response = PutSync<BaseResponseModel, UpdateUserDto>(SystemConstants.UserService, $"/api/member/{id}", userDto);
            var result = JsonConvert.DeserializeObject<UserDto>(response.XData.ToString());
            return result;
        }
    }
}
