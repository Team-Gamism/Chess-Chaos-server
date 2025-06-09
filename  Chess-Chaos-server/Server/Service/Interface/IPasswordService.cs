namespace Server.Service.Interface;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyHashPassword(string password, string hashedPassword);
}