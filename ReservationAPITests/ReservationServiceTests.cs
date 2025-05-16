using ReservationAPI.Services;
using ReservationAPI.Data;
using ReservationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ReservationAPI.DTO;

public class ReservationServiceTests
{

    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=reservationsTestDb.db")
            .Options;

        return new AppDbContext(options);
    }

    private ReservationService GetReservationService()
    {
        var mockLogger = new Mock<ReservationAPI.Logging.ILogger>();
        var mockLoggerFactory = new Mock<ReservationAPI.Logging.ILoggerFactory>();
        mockLoggerFactory
        .Setup(f => f.CreateLogger(It.IsAny<string>()))
        .Returns(mockLogger.Object);

        return new ReservationService(GetDbContext(), mockLoggerFactory.Object);
    }

    [Fact]
    public async Task GetReservations_ReturnAllReservationsForSpecificUser()
    {
        var dbContext = GetDbContext();
        dbContext.Database.Migrate();

        dbContext.Users.AddRange(
            new User { Name = "Rui", Email = "ruicavaco@hotmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" },
            new User { Name = "Alexia", Email = "something@gmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" }
            );

        await dbContext.SaveChangesAsync();

        dbContext.AddRange(
            new Reservation { ReservationName = "Test Res 1", ReservationDay = new DateOnly(2025, 1, 1), UserId = 1 },
            new Reservation { ReservationName = "Test Res 2", ReservationDay = new DateOnly(2025, 1, 2), UserId = 2 }
        );

        await dbContext.SaveChangesAsync();

        var service = GetReservationService();
        string userIdToTest = "1";
        var result = await service.GetReservations(userIdToTest);

        dbContext.Database.EnsureDeleted();

        Assert.Single(result);
        Assert.Equal("Test Res 1", result[0].ReservationName);

    }

    [Fact]
    public async Task GetReservation_ReturnReservation()
    {

        var dbContext = GetDbContext();
        dbContext.Database.Migrate();

        dbContext.Users.AddRange(
            new User { Name = "Rui", Email = "ruicavaco@hotmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" },
            new User { Name = "Alexia", Email = "something@gmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" }
            );

        await dbContext.SaveChangesAsync();

        dbContext.Add(new Reservation
        {
            ReservationName = "Test Res 1",
            ReservationDay = new DateOnly(2025, 1, 1),
            UserId = 1
        });

        await dbContext.SaveChangesAsync();

        SendReservationDto expectedReservation = new()
        {
            ReservationID = 1,
            ReservationName = "Test Res 1",
            ReservationDay = new DateOnly(2025, 1, 1)
        };

        var service = GetReservationService();
        var result = await service.GetReservation(1, "1");

        dbContext.Database.EnsureDeleted();

        Assert.Equal(expectedReservation.ReservationID, result.ReservationID);
        Assert.Equal(expectedReservation.ReservationName, result.ReservationName);
        Assert.Equal(expectedReservation.ReservationDay, result.ReservationDay);

    }

    [Fact]
    public async Task AddReservation_ReturnReservationAdded()
    {

        var dbContext = GetDbContext();
        dbContext.Database.Migrate();
        var service = GetReservationService();

        string reservationName = "Test Res 1";
        DateOnly reservationDay = new(2025, 1, 1);
        string userId = "1";

        dbContext.Users.AddRange(
            new User { Name = "Rui", Email = "ruicavaco@hotmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" },
            new User { Name = "Alexia", Email = "something@gmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" }
            );

        await dbContext.SaveChangesAsync();

        var result = await service.AddReservation(reservationName, reservationDay, userId);

        dbContext.Database.EnsureDeleted();

        Assert.NotNull(result);
        Assert.Equal(result.ReservationName, reservationName);
        Assert.Equal(result.ReservationDay, reservationDay);
        Assert.True(result.ReservationID > 0);

    }

    [Fact]
    public async Task DeleteReservation_ReturnTrue()
    {

        var dbContext = GetDbContext();
        dbContext.Database.Migrate();
        var service = GetReservationService();

        dbContext.Users.AddRange(
            new User { Name = "Rui", Email = "ruicavaco@hotmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" },
            new User { Name = "Alexia", Email = "something@gmail.com", PasswordHash = "afdsf2378rbefw78332%/&%&/$$G$%#" }
            );

        await dbContext.SaveChangesAsync();

        dbContext.AddRange(
            new Reservation { ReservationName = "Test Res 1", ReservationDay = new DateOnly(2025, 1, 1), UserId = 1 },
            new Reservation { ReservationName = "Test Res 2", ReservationDay = new DateOnly(2025, 1, 2), UserId = 2 }
        );

        await dbContext.SaveChangesAsync();
        var result = await service.DeleteReservation(1, "1");

        dbContext.Database.EnsureDeleted();

        Assert.True(result);
    }
}