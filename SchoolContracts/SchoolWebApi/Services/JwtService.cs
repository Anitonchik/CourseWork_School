using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Models;
using SchoolWebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SchoolWebApi.Services;

public class JwtService
{
    public readonly IStorekeeperAdapter _adapter;
    public readonly IStorekeeperBuisnessLogicContract _storekeeperBuisnessLogicContract;
    public readonly IWorkerBuisnessLogicContract _workerBuisnessLogicContract;

    private readonly IConfiguration _configuration;
    private readonly Mapper _mapperStorekeeper;
    private readonly Mapper _mapperWorker;


    public JwtService(IStorekeeperAdapter adapter, IStorekeeperBuisnessLogicContract storekeeperBuisnessLogicContract,
        IWorkerBuisnessLogicContract workerBuisnessLogicContract, IConfiguration configuration)
    {
        _adapter = adapter;
        _storekeeperBuisnessLogicContract = storekeeperBuisnessLogicContract;
        _workerBuisnessLogicContract = workerBuisnessLogicContract;
        _configuration = configuration;

        var configStorekeeper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Storekeeper, StorekeeperDataModel>();
            cfg.CreateMap<StorekeeperDataModel, Storekeeper>();
        });

        _mapperStorekeeper = new Mapper(configStorekeeper);
        
        var configWorker = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Worker, WorkerDataModel>();
            cfg.CreateMap<WorkerDataModel, Worker>();
        });
        _mapperWorker = new Mapper(configWorker);
    }

    public async Task<LoginResponseModel> Authenticate(LoginRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.UserLogin) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        StorekeeperDataModel userStorekeeperAccaunt;
        WorkerDataModel userWorkerAccaunt;

        string userId = string.Empty;
        string userFIO = string.Empty;
        string userMail = string.Empty;
        UserRole userRole = UserRole.Storekeeper;

        try
        {
            switch (request.Role)
            {
                case UserRole.Storekeeper:
                    userStorekeeperAccaunt = _storekeeperBuisnessLogicContract.GetStorekeeperByLogin(request.UserLogin);
                    if (userStorekeeperAccaunt is null || request.Password != userStorekeeperAccaunt.Password)
                        return null;

                    userId = userStorekeeperAccaunt.Id;
                    userFIO = userStorekeeperAccaunt.FIO;
                    userMail = userStorekeeperAccaunt.Mail;
                    userRole = UserRole.Storekeeper;

                    break;

                case UserRole.Worker:
                    userWorkerAccaunt = _workerBuisnessLogicContract.GetWorkerByLogin(request.UserLogin);
                    if (userWorkerAccaunt is null || request.Password != userWorkerAccaunt.Password)
                        return null;

                    userId = userWorkerAccaunt.Id;
                    userFIO = userWorkerAccaunt.FIO;
                    userMail = userWorkerAccaunt.Mail;
                    userRole = UserRole.Worker;

                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            //_logger.LogError(ex, "ArgumentNullException");
            throw;
        }
        catch (ValidationException ex)
        {
            //_logger.LogError(ex, "ValidationException");
            throw;
        }
        catch (ElementExistsException ex)
        {
            //_logger.LogError(ex, "ElementExistsException");
            throw;
        }
        catch (StorageException ex)
        {
            //_logger.LogError(ex, "StorageException");
            throw;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Exception");
            throw;
        }      

        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // данные внутри токена, вписываем login и роль пользователя
                new Claim(JwtRegisteredClaimNames.Actort, request.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, request.UserLogin)
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            SecurityAlgorithms.HmacSha256Signature),
        };

        // генерация токена
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponseModel
        {
            Id = userId,
            UserFIO = userFIO,
            Role = userRole,
            UserLogin = request.UserLogin,
            Mail = userMail,
            AccessToken = accessToken,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds        
        };
    }

    public async Task<LoginResponseModel> Register(UserBindingModel userModel)
    {
        Storekeeper storekeeper;
        Worker worker;
        UserRole userRole;
        LoginRequestModel response;

        switch (userModel.Role)
        {
            case 0:
                worker = new Worker
                {
                    Id = userModel.Id,
                    FIO = userModel.FIO,
                    Login = userModel.Login,
                    Mail = userModel.Mail,
                    Password = userModel.Password
                };
                userRole = UserRole.Worker;

                _workerBuisnessLogicContract.InsertWorker(_mapperWorker.Map<WorkerDataModel>(worker));

                var userWorker = _workerBuisnessLogicContract.GetWorkerByLogin(worker.Login);

                response = new LoginRequestModel
                {
                    UserLogin = userWorker.Login,
                    Password = userWorker.Password,
                    Role = UserRole.Worker
                };
                return await Authenticate(response);
            case 1:
                storekeeper = new Storekeeper
                {
                    Id = userModel.Id,
                    FIO = userModel.FIO,
                    Login = userModel.Login,
                    Mail = userModel.Mail,
                    Password = userModel.Password
                };
                userRole = UserRole.Storekeeper;

                _storekeeperBuisnessLogicContract.InsertStorekeeper(_mapperStorekeeper.Map<StorekeeperDataModel>(storekeeper));

                var userStorekeeper = _storekeeperBuisnessLogicContract.GetStorekeeperByLogin(storekeeper.Login);

                response = new LoginRequestModel
                {
                    UserLogin = userStorekeeper.Login,
                    Password = userStorekeeper.Password,
                    Role = UserRole.Storekeeper
                };

                return await Authenticate(response);
        }
        return null;
    }

    // request - запрос
    // response - ответ
}
