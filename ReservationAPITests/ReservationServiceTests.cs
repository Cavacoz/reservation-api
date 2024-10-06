using ReservationAPI.Services;
using ReservationAPI.Data;
using ReservationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ReservationAPITests;

[TestClass]
public class ReservationServiceTests
{

    private AppDbContext _dbContext;
    private ReservationService _reservationService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ReservationDb")
            .Options;


        _dbContext = new AppDbContext(options);

        var logger = new LoggerFactory().CreateLogger<ReservationService>();
        var customLoggerFactory = new ReservationAPI.Logging.LoggerFactory();


        _reservationService = new ReservationService(_dbContext, logger, customLoggerFactory);
    }

    [TestMethod]
    public async Task GetReservations_ReturnAllReservations()
    {
        await _dbContext.Reservations.AddRangeAsync(new List<Reservation>
        {
            new Reservation {ReservationName = "Alexia's Reservation", ReservationDay = DateOnly.FromDayNumber(0), User = "Alexia"},
            new Reservation {ReservationName = "Rui's Reservation", ReservationDay = DateOnly.FromDayNumber(0), User = "Rui" }
        });

        await _dbContext.SaveChangesAsync();

        var result = await _reservationService.GetReservations();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}