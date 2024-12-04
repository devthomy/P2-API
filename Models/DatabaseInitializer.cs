using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public static class DatabaseInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Check if users already exist
            if (context.Users.Any())
            {
                Console.WriteLine("Database already seeded with users.");
                return;
            }

            try
            {
                // Create users
                var user1 = new User
                {
                    Username = "alicemartin",
                    FirstName = "Alice", 
                    LastName = "Martin",
                    Email = "alice.martin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password789"),
                    Score = 15,
                    CreatedAt = DateTime.Now
                };

                var user2 = new User
                {
                    Username = "bobwilson", 
                    FirstName = "Bob",
                    LastName = "Wilson",
                    Email = "bob.wilson@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password101"),
                    Score = 25,
                    CreatedAt = DateTime.Now
                };

                // Add users first and save to get IDs
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.SaveChanges();
                Console.WriteLine($"Added users: {user1.Username}, {user2.Username}");

                // Create and add teams
                var team1 = new Team { TeamName = "Team Alpha", TotalScore = 100, CreatedAt = DateTime.Now, User = new List<User> { user1 } };
                var team2 = new Team { TeamName = "Team Beta", TotalScore = 200, CreatedAt = DateTime.Now, User = new List<User> { user2 } };
                context.Teams.AddRange(team1, team2);
                context.SaveChanges();
                Console.WriteLine("Added teams");

                // Create and add defis
                var defi1 = new Defi { Title = "Algorithm Implementation", Description = "Implement a sorting algorithm", Points = 15, CreatedAt = DateTime.Now };
                var defi2 = new Defi { Title = "Debug Code", Description = "Find and fix bugs in the given code", Points = 10, CreatedAt = DateTime.Now };
                var defi3 = new Defi { Title = "Calculus Problem", Description = "Solve differential equations", Points = 20, CreatedAt = DateTime.Now };
                var defi4 = new Defi { Title = "Statistics Exercise", Description = "Calculate probability distributions", Points = 15, CreatedAt = DateTime.Now };
                context.Defis.AddRange(defi1, defi2, defi3, defi4);
                context.SaveChanges();
                Console.WriteLine("Added defis");

                // Create and add challenges
                var challenge1 = new Challenge { Title = "Programming Basics", Description = "Master fundamental programming concepts", TotalPoints = 50, Status = true, Defis = new List<Defi> { defi1, defi2 } };
                var challenge2 = new Challenge { Title = "Mathematics Challenge", Description = "Test your mathematical skills", TotalPoints = 75, Status = true, Defis = new List<Defi> { defi3, defi4 } };
                var challenge3 = new Challenge { Title = "Web Development", Description = "Create responsive web applications", TotalPoints = 100, Status = true, Defis = new List<Defi>() };
                context.Challenges.AddRange(challenge1, challenge2, challenge3);
                context.SaveChanges();
                Console.WriteLine("Added challenges");

                // Create and add challenge participations
                var challengeParticipation1 = new ChallengeParticipation
                {
                    ParticipationDate = DateTime.Now,
                    Score = 10,
                    Challenge = challenge1,
                    User = user1,
                };
                var challengeParticipation2 = new ChallengeParticipation
                {
                    ParticipationDate = DateTime.Now,
                    Score = 20,
                    Challenge = challenge2,
                    User = user2
                };
                context.Participations.AddRange(challengeParticipation1, challengeParticipation2);
                context.SaveChanges();
                Console.WriteLine("Added participations");

                // Create and add team members
                var teamMember1 = new TeamMember { Team = team1, User = user1 };
                var teamMember2 = new TeamMember { Team = team2, User = user2 };
                context.TeamMembers.AddRange(teamMember1, teamMember2);
                context.SaveChanges();
                Console.WriteLine("Added team members");

                Console.WriteLine("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}
