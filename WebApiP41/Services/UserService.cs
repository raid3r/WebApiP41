using WebApiP41.Models;

using Microsoft.EntityFrameworkCore;

namespace WebApiP41.Services;

public class UserService(ApiDbContext dbContext)
{

    // реєстрація нового користувача
    public async Task<User> RegisterUserAsync(User newUser)
    {
        // перевірка, чи користувач з таким email вже існує
        if (await dbContext.Users
                .AnyAsync(x => x.Email == newUser.Email))
        {
            throw new InvalidDataException("User already exists");
        }

        newUser.Token = GenerateToken(newUser);

        // збереження нового користувача в базі даних
        await dbContext.Users.AddAsync(newUser);
        // збереження змін в базі даних
        await dbContext.SaveChangesAsync();
        return newUser;
    }


    // перевірка логіна та пароля користувача
    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await dbContext.Users
            .Include(x => x.Image)
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        // закоментовано для демонстрації, щоб токен не змінювався при кожному логіні,
        // а залишався постійним для користувача

        //if (user != null)
        //{
        //    user.Token = GenerateToken(user); // генеруємо новий токен для користувача
        //    await dbContext.SaveChangesAsync();
        //}
        return user;
    }


    // генерація токена для користувача
    public string GenerateToken(User user)
    {
        // простий приклад генерації токена (не безпечний, для демонстрації)
        // GUID example: 123e4567-e89b-12d3-a456-426614174000
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }


    // перевірка токена користувача (приймає токен. повертає користувача або null)
    public async Task<User?> ValidateTokenAsync(string token)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Token == token);
    }

    public async Task<ICollection<User>> GetAllUsersAsync()
    {
        return await dbContext.Users
            .Include(x => x.Image)
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmailIdAsync(string email)
    {
        return await dbContext.Users
            .Include(x => x.Image)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await dbContext.Users
            .Include(x => x.Image)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        await dbContext.SaveChangesAsync();
    }

}
