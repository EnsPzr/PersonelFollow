using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Concrete.EntityFramework
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<DataContext>();
            context.Database.Migrate();
            if (!context.UserInformations.Any())
            {
                IUserRepository userRepository = new EfUserRepository(context);
                userRepository.Add(new UserInformation()
                {
                    EMail = "a@a.com",
                    Password = "12345678",
                    UserName = "Enes",
                    UserSurname = "Pazar",
                    isAdministrator = true
                });
            }
        }
    }
}
