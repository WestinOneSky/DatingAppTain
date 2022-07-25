using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext2 context2)
        {
            if (await context2.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/USerSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();
                
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("passw0rd"));
                user.PasswordSalt = hmac.Key;

                context2.Users.Add(user);
            }

            await context2.SaveChangesAsync();
        }
    }
}