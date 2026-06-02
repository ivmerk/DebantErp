using DebantErp.BL.Speciality;
using DebantErp.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/specialities")]
public class SpecialityController : ControllerBase
{
    private readonly ISpeciality _speciality;

    public SpecialityController(ISpeciality speciality)
    {
        _speciality = speciality;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpeciality(int id)
    {
        var speciality = await _speciality.GetSpeciality(id);
        return Ok(speciality);
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecialities()
    {
        var specialities = await _speciality.GetSpecialities();
        return Ok(specialities);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUpdateSpecialityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var specialityId = await _speciality.Create(dto);
        return Ok(specialityId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CreateUpdateSpecialityDto dto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var specialityId = await _speciality.Update(id, dto);
        return Ok(specialityId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var specialityId = await _speciality.Delete(id);
        return Ok(specialityId);
    }
}
