using ReservationAPI.Controllers;
using ReservationAPI.Data;
using ReservationAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ReservationAPITests;

[TestClass]
public class ReservationControllerTests
{

    private AppDbContext _dbContext;

    private ReservationController _reservationController;

    private ReservationService _reservationService;

    [TestInitialize]
    public void Setup()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ReservationDb")
            .Options;


        _dbContext = new AppDbContext(options);

        var serviceLogger = new LoggerFactory().CreateLogger<ReservationService>();
        var controllerLogger = new LoggerFactory().CreateLogger<ReservationController>();
        var customLoggerFactory = new ReservationAPI.Logging.LoggerFactory();

        _reservationService = new ReservationService(_dbContext, serviceLogger, customLoggerFactory);
        _reservationController = new ReservationController(_reservationService, controllerLogger, customLoggerFactory);
    }

}