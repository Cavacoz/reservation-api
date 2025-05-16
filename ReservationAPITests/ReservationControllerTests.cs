using ReservationAPI.Controllers;
using ReservationAPI.Data;
using ReservationAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net;
using ReservationAPI.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using ReservationAPI.Exceptions;


public class ReservationControllerTests
{

    // private AppDbContext GetDbContext()
    // {
    //     var options = new DbContextOptionsBuilder<AppDbContext>()
    //     .UseSqlite("Data Source=reservationTestDb.db")
    //     .Options;

    //     return new AppDbContext(options);
    // }

    private IReservationService GetReservationMockService()
    {
        var moqService = new Mock<IReservationService>();

        moqService.Setup(s => s.GetReservations("1"))
              .ReturnsAsync(new List<SendReservationDto>());

        moqService.Setup(s => s.GetReservation(1, "1"))
            .ThrowsAsync(new ReservationNotFound("Reservation was not found."));

        moqService.Setup(s => s.DeleteReservation(1, "1"))
            .ReturnsAsync(false);

        moqService.Setup(s => s.AddReservation("name", new DateOnly(2025, 1, 1), "1"))
            .ReturnsAsync(new SendReservationDto {ReservationName = "name", ReservationDay = new DateOnly(2025, 1, 1), ReservationID = 1});

        return moqService.Object;
    }

    private ReservationController GetReservationController()
    {

        var moqLogger = new Mock<ReservationAPI.Logging.ILogger>();
        var moqLoggerFactory = new Mock<ReservationAPI.Logging.ILoggerFactory>();

        moqLoggerFactory.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(moqLogger.Object);

        return new ReservationController(GetReservationMockService(), moqLoggerFactory.Object);

    }

    [Fact]
    public async Task GetAll_ReturnReservationsForUser()
    {
        var controller = GetReservationController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, "1")
                }, "mock"))
            }
        };
        var result = await controller.GetAll();
        var noContentResult = Assert.IsType<NoContentResult>(result);

        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }

    [Fact]
    public async Task GetReservation_ReturnReservationForUser()
    {
        var controller = GetReservationController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, "1")
                }, "mock"))
            }
        };

        var result = await controller.GetReservation(1);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }


    [Fact]
    public async Task AddReservation_ReturnAddedReservation()
    {
        var controller = GetReservationController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, "1")
                }, "mock"))
            }
        };

        AddReservationDto reservationToAdd = new() { ReservationName = "name", Year = 2025, Month = 1, Day = 1 };

        var result = await controller.AddReservation(reservationToAdd);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
    }

}