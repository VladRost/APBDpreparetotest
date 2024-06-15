using APBDPrepare.Context;
using APBDPrepare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDPrepare.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientController: ControllerBase
{
    private readonly BoatDbContext _dbContext;

    public ClientController(BoatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        var client = _dbContext.Clients.Include(c => c.ClientCategory).SingleOrDefault(c => c.IdClient == id);
        if (client == null)
        {
            throw new ArgumentException("Client doesnt exists");
            
        }

        var reservations =
            await _dbContext.Reservations.Where(e => e.IdClient == id).OrderBy(e => e.DateTo).ToListAsync();
        var clientDto = new ClientsReservationsResponseDto
        {
            IdClient = client.IdClient,
            Name = client.Name,
            LastName = client.LastName,
            Birthday = client.Birthday,
            Pesel = client.Pesel,
            Email = client.Email,
            Reservations = reservations.Select(r => new ReservationDto
            {
                IdReservation = r.IdReservation,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                Capacity = r.Capacity,
                NumOfBoats = r.NumOfBoats,
                Fulfilled = r.Fulfilled,
                Price = r.Price,
                CancelReason = r.CancelReason
            })
        };
        return Ok(clientDto);
    }
}