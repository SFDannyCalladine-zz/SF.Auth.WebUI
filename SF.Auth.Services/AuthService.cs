using System;
using AutoMapper;
using SF.Auth.DataTransferObjects.Account;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories.Interfaces;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;
using SF.Common.Services.Exceptions;
using SF.Common.Settings.Repositories.Interfaces;

namespace SF.Auth.Services
{
    public class AuthService : RootService, IAuthService
    {
        #region Private Fields

        private readonly IDbCustomerDatabaseFactory _customerContextFactory;
        private readonly IMapper _mapper;

        private IUserRepository _userRepository;

        #endregion Private Fields

        #region Public Constructors

        public AuthService(
            IRootRepository rootRepository,
            ISettingRepository settingRepository,
            IDbCustomerDatabaseFactory customerContextFactory,
            IMapper mapper)
            : base(
                  rootRepository,
                  settingRepository)
        {
            _customerContextFactory = customerContextFactory;
            _mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

        public Response<UserDto> ValidateUser(ValidateUserRequest request)
        {
            try
            {
                var connectionString = GetConnectionStringByEmail(request.Email);

                _userRepository = new UserRepository(_customerContextFactory.CreateDbContext(connectionString));

                var user = _userRepository.FindUserByEmail(request.Email);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "User can not be found with provided Email.");
                }

                var valid = user.ValidatePassword(request.Password);

                if (valid)
                {
                    var dto = _mapper.Map<UserDto>(user);

                    return new Response<UserDto>(dto);
                }

                return new Response<UserDto>(ResponseCode.ValidationError, "Failed Login");
            }
            catch (Exception e)
            {
                return HandleException<UserDto>(e);
            }
        }

        #endregion Public Methods
    }
}