using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Api.Controllers;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardController(ApplicationDbContext context)
        {
            _context = context;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LeaderboardController initialized");
            Console.ResetColor();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetLeaderboard()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("GetLeaderboard endpoint called");
            Console.ResetColor();

            var leaderboard = await _context.Users
                .OrderByDescending(u => u.Score)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Score
                })
                .ToListAsync();

            return Ok(leaderboard);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetLeaderboardPosition(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var position = await _context.Users
                .CountAsync(u => u.Score > user.Score) + 1;

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Score,
                Position = position
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateScore(string id, [FromBody] LeaderboardUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(int.Parse(id));
            if (user == null) return NotFound();

            user.Score = updateDto.Score;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Score
            });
        }

        [HttpPut]
        public async Task<ActionResult<object>> UpdateUserScore([FromBody] UpdateScoreRequest request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null) return NotFound();

            if (request.Increment.HasValue)
            {
                user.Score += request.Increment.Value;
            }
            else if (request.Decrement.HasValue)
            {
                user.Score = Math.Max(0, user.Score - request.Decrement.Value);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Score
            });
        }
    }

    public class UpdateScoreRequest
    {
        public int Id { get; set; }
        public int? Increment { get; set; }
        public int? Decrement { get; set; }
    }

    public class LeaderboardUpdateDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int Position { get; set; }
    }
}

public class LeaderboardControllerTests
{
    [Fact]
    public async Task GetLeaderboard_ReturnsOrderedLeaderboard()
    {
        var mockContext = new Mock<ApplicationDbContext>();
        var users = new List<User>
        {
            new User { Id = 1, Username = "User1", Score = 100, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PasswordHash = "password1" },
            new User { Id = 2, Username = "User2", Score = 200, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PasswordHash = "password2" },
            new User { Id = 3, Username = "User3", Score = 150, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", PasswordHash = "password3" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        var controller = new LeaderboardController(mockContext.Object);

        var result = await controller.GetLeaderboard();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
        Assert.Equal(3, returnedUsers.Count());
        Assert.Equal(200, ((dynamic)returnedUsers.First()).Score);
    }
}

