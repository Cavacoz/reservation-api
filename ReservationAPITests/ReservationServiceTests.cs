using ReservationAPI.Services;
using ReservationAPI.Data;
using ReservationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

    [TestMethod]
    public async Task GetReservation_ReturnReservation()
    {
        await _dbContext.Reservations.AddAsync(new Reservation
        {
            ReservationName = "Rui's Reservation",
            ReservationDay = DateOnly.FromDayNumber(0),
            User = "Rui"
        });

        await _dbContext.SaveChangesAsync();

        Reservation? reservation = await _reservationService.GetReservation(1);

        Reservation expectedReservation = new()
        {
            ReservationName = "Rui's Reservation",
            ReservationDay = DateOnly.FromDayNumber(0),
            User = "Rui"
        };

        Assert.IsNotNull(reservation);
        Assert.AreEqual(expectedReservation.ReservationName, reservation.ReservationName);
        Assert.AreEqual(expectedReservation.ReservationDay, reservation.ReservationDay);
        Assert.AreEqual(expectedReservation.User, reservation.User);
    }

    [TestMethod]
    public async Task AddReservation_ReturnReservationAdded()
    {
        string reservationName = "Alexia's Reservation";
        DateOnly reservationDay = DateOnly.FromDayNumber(0);
        string user = "Alexia";
        Reservation reservation = await _reservationService.AddReservation(reservationName, reservationDay, user);
        Assert.IsNotNull(reservation);
        Assert.AreEqual(reservation.ReservationID, 1);
        Assert.AreEqual(reservation.ReservationName, reservationName);
        Assert.AreEqual(reservation.ReservationDay, reservationDay);
        Assert.AreEqual(reservation.User, user);
    }

    [TestMethod]
    public async Task DeleteReservation_ReturnTrue()
    {
        await _dbContext.AddAsync(new Reservation
        {
            ReservationName = "Alexia's Reservation",
            ReservationDay = DateOnly.FromDayNumber(0),
            User = "Alexia"
        });

        await _dbContext.SaveChangesAsync();

        bool wasDeleted = await _reservationService.DeleteReservation(1);

        Assert.IsTrue(wasDeleted);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}