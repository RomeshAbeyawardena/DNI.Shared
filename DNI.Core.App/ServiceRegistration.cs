﻿using DNI.Core.Contracts;
using DNI.Core.Contracts.Options;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using DNI.Core.Services.Extensions;
using DNI.Core.App.Domains;
using Microsoft.EntityFrameworkCore;
using DNI.Core.App.Contracts;
using AutoMapper;
using System.Reflection;
using DNI.Core.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace DNI.Core.App
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            services
                .AddLogging(logginOptions => logginOptions.AddConsole())
                .RegisterCryptographicCredentialsFactory<MCryptographicCredentials>((factory, cryptographyProvider, s) => factory
                .CaseWhen(Constants.PersonalDataEncryption, cryptographyProvider
                .GetCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, 
                    Encoding.UTF8, "3d21cecb-189d-4e6b-bea1-91b68de3a37b", "851a5944-115f-4e79-b468-82b67f00e349", 1000000, 32, "851a5944-115f-4e".GetBytes(Encoding.UTF8)))
                .CaseWhen(Constants.IdentifierDataEncryption, cryptographyProvider
                    .GetCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, 
                    Encoding.UTF8, "42e6f1f0-7cd2-4ce3-a06c-f86c1c82fd24", "eeaf5b47-636c-4997-ae41-d979e3b04094", 1000000, 32, "bceac9fa-70a3-4b".GetBytes(Encoding.UTF8))))
                .AddAutoMapper(Assembly.GetAssembly(typeof(ServiceRegistration)))
                .RegisterDbContextRepositories<TestDbContext>(options => { 
                    options.ServiceLifetime = ServiceLifetime.Transient; 
                    options.DbContextOptions = dbContextOptions => dbContextOptions
                        .UseSqlServer("Server=localhost;Database=KeyExchange;Trusted_Connection=true");
                    options.EntityTypeDescriber = describer => describer.Describe<Customer>(); })
                .RegisterCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, Encoding.ASCII, 
                "drrNR2mQjfRpKbuN9f9dSwBP2MAfVCPS", 
                "vaTfUcv4dK6wYF6Z8HnYGuHQME3PWWYnz5VRaJDXDSPvFWJxqF2Q2ettcbufQbz5", 1000000, 32, null)
                .RegisterDefaultValueGenerator<Customer>(customerGenerator => 
                    customerGenerator.Add(customer => customer.UniqueId, (serviceProvider) => serviceProvider
                    .GetRequiredService<IGuidGeneratorService>()
                    .Generate()));
        }

    }
}
