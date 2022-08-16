// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerWebApi.Data;
using PlayerWebApi.Data.Entities;

namespace PlayerWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly PlayerDbContext m_dbContext;

    public PlayerController(PlayerDbContext dbContext)
    {
        m_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Create Player
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> PostAsync()
    {
        PlayerSkill playerSkill = new PlayerSkill() { Id = 1, PlayerId = 1, Skill = "Making chicken nuggets", Value = 0 };
        await m_dbContext.PlayerSkills.AddAsync(playerSkill);

        Player newPlayer = new Player()
        {
            Id = 1,
            Name = "Juan",
            PlayerSkills = m_dbContext.PlayerSkills.Where(x => x.PlayerId == 1).ToList(),
            Position = "Benchwarmer"
        };

        await m_dbContext.Players.AddAsync(newPlayer);

        await m_dbContext.SaveChangesAsync();

        return Ok(newPlayer);
    }

    /// <summary>
    /// Read Players
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var players = await m_dbContext.Players.Include(p => p.PlayerSkills).ToListAsync();

        return Ok(players);
    }

    /// <summary>
    /// Read Player
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var possiblePlayers = await m_dbContext.Players.Include(p => p.PlayerSkills).Where(x => x.Id == id).ToListAsync();
            var player = possiblePlayers.First();

            return Ok(player);
        }
        catch
        {
            return NoContent();
        }
    }

    /// <summary>
    /// Upate Player
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id)
    {
        try
        {
            var possiblePlayers = await m_dbContext.Players.Include(p => p.PlayerSkills).Where(x => x.Id == id).ToListAsync();
            var player = possiblePlayers.First();

            player.Position = "Da boss";

            await m_dbContext.SaveChangesAsync();
            return Ok(player);
        }
        catch
        {
            return NoContent();
        }
    }

    /// <summary>
    /// Delete Player
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var possiblePlayers = await m_dbContext.Players.Include(p => p.PlayerSkills).Where(x => x.Id == id).ToListAsync();
            var player = possiblePlayers.First();
            var skill = await m_dbContext.PlayerSkills.FirstAsync(x => x.PlayerId == player.Id);

            m_dbContext.PlayerSkills.Remove(skill);
            m_dbContext.Players.Remove(player);
            await m_dbContext.SaveChangesAsync();

            return Ok(m_dbContext.Players);
        }
        catch
        {
            return NoContent();
        }
    }
}
