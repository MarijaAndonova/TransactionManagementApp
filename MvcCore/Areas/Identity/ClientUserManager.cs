using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using MvcCore.Data;
using MvcCore.Models;
using System;
using System.Threading.Tasks;

public class ClientUserManager<TUser> : UserManager<TUser> where TUser : IdentityUser
{
    readonly ApplicationDbContext context;

    public ClientUserManager(ApplicationDbContext context, IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        IServiceProvider services, ILogger<UserManager<TUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators,
            passwordValidators, keyNormalizer, errors, services, logger)
    {
        this.context = context;
    }

    public override async Task<IdentityResult> CreateAsync(TUser user, string password)
    {
        // Execute your custom code before registering the user here

        // Call the base method to perform the user creation
        var result = await base.CreateAsync(user, password);

        // Execute any additional operations after user registration here
        if (result.Succeeded)
        {
            context.Add(new Client
            {
                UserName = user.UserName,
                Name = user.UserName
            });
            await context.SaveChangesAsync();
        }

        return result;
    }
}