using WebApiP41.Models;

namespace WebApiP41.Services;

public class UserService(IDataProvider<User> dataProvider)
{

    // реєстрація нового користувача
    public async Task<User> RegisterUserAsync(User newUser)
    {
        var users = await dataProvider.GetAllAsync();

        if (users.FirstOrDefault(x => x.Email == newUser.Email) != null) {
            throw new InvalidDataException("User already exists");
        }



        newUser.Token = GenerateToken(newUser);
        users.Add(newUser);
        await dataProvider.SaveChanges(users);
        return newUser;
    }


    // перевірка логіна та пароля користувача
    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var users = await dataProvider.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);

        // закоментовано для демонстрації, щоб токен не змінювався при кожному логіні,
        // а залишався постійним для користувача

        //if (user != null)
        //{
        //    user.Token = GenerateToken(user); // генеруємо новий токен для користувача
        //    await dataProvider.SaveChanges(users);
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
        var users = await dataProvider.GetAllAsync();
        return users.FirstOrDefault(u => u.Token == token);
    }



}
