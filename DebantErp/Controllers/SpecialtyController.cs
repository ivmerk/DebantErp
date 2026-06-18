using DebantErp.BL.Specialty;
using DebantErp.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/specialties")]
public class SpecialtyController : ControllerBase
{
    private readonly ISpecialty _specialty;

    public SpecialtyController(ISpecialty specialty)
    {
        _specialty = specialty;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecialty(int id)
    {
        var specialty = await _specialty.GetSpecialty(id);
        return Ok(specialty);
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecialties()
    {
        var specialties = await _specialty.GetSpecialties();
        return Ok(specialties);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUpdateSpecialtyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var specialtyId = await _specialty.Create(dto);
        return Ok(specialtyId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CreateUpdateSpecialtyDto dto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var specialtyId = await _specialty.Update(id, dto);
        return Ok(specialtyId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var specialtyId = await _specialty.Delete(id);
        return Ok(specialtyId);
    }
}
