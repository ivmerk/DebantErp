using Microsoft.AspNetCore.Mvc;
using DebantErp.Dtos;
using DebantErp.Rdos;
using DebantErp.BL.OrderLaborCost;

namespace DebantErp.Controllers
{
  [Route("api/labor-cost")]
  [ApiController]
  public class LaborCostController: ControllerBase
  {
    private readonly IOrderLaborCost _orderLaborCost;

    public LaborCostController(IOrderLaborCost laborCost)
    {
      _orderLaborCost = laborCost;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderLaborCostRdo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetList()
    {
      var laborCosts = await _orderLaborCost.Get();
      if (laborCosts == null) return NotFound();
      return Ok(laborCosts);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderLaborCostRdo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
      var laborCost = await _orderLaborCost.Get(id);
      if (laborCost == null) return NotFound();
      return Ok(laborCost);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateOrderLaborCostDto dto)
    {
      var laborCostId = await _orderLaborCost.Create(dto);
      return Ok(laborCostId);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderLaborCostDto dto)
    {
      var laborCostId = await _orderLaborCost.Update(id, dto);
      return Ok(laborCostId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      var laborCostId = await _orderLaborCost.Delete(id);
      if (laborCostId == 0) return NotFound();
      return Ok(laborCostId);
    }
  }
}
