using System;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Score { get; set; }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, this.PasswordHash);
    }
}