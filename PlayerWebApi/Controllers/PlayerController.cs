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
    public async Task<IActionResult> PostAsync(string name, string position, IEnumerable<PlayerSkill> playerSkills)
    {
        try
        {
            PlayerValidationCommon.ValidatePlayerName(name);
            PlayerValidationCommon.ValidatePlayerPosition(position);
            PlayerValidationCommon.ValidatePlayerSkills((List<PlayerSkill>)playerSkills);

            Player newPlayer = new Player()
            {
                Name = name,
                PlayerSkills = playerSkills,
                Position = position
            };

            await m_dbContext.Players.AddAsync(newPlayer);

            await m_dbContext.SaveChangesAsync();

            return Ok(m_dbContext.Players.FindAsync(newPlayer.Id).Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Read Players
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var players = await m_dbContext.Players.Include(p => p.PlayerSkills).ToListAsync();

            return Ok(players);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Read Player
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            List<Player> possiblePlayers = await m_dbContext.Players.Include(p => p.PlayerSkills).Where(x => x.Id == id).ToListAsync();
            if(possiblePlayers.Count == 0)
            {
                throw new Exception("Could not find player with ID: " + id);
            }
            var player = possiblePlayers.First();

            return Ok(player);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Upate Player
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, string name = null, string position = null, IEnumerable<PlayerSkill> playerSkills = null)
    {
        try
        {
            var possiblePlayers = await m_dbContext.Players.Include(p => p.PlayerSkills).Where(x => x.Id == id).ToListAsync();
            if (possiblePlayers.Count == 0)
            {
                throw new Exception("Could not find player with ID: " + id);
            }
            var player = possiblePlayers.First();

            if(name != null)
            {
                player.Name = name;
            }
            if(position != null)
            {
                PlayerValidationCommon.ValidatePlayerPosition(position);
                player.Position = position;
            }
            if(playerSkills != null)
            {
                PlayerValidationCommon.ValidatePlayerSkills((List<PlayerSkill>)playerSkills);

                player.PlayerSkills = playerSkills;
            }

            await m_dbContext.SaveChangesAsync();
            return Ok(m_dbContext.Players.FindAsync(id).Result);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
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
            if (possiblePlayers.Count == 0)
            {
                throw new Exception("Could not find player with ID: " + id);
            }
            var player = possiblePlayers.First();
            var skill = await m_dbContext.PlayerSkills.FirstAsync(x => x.PlayerId == player.Id);

            m_dbContext.PlayerSkills.Remove(skill);
            m_dbContext.Players.Remove(player);
            await m_dbContext.SaveChangesAsync();

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
