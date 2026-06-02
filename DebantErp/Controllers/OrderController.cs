using Microsoft.AspNetCore.Mvc;
using DebantErp.Dtos;
using DebantErp.Rdos;
using DebantErp.BL.Order;

namespace DebantErp.Controllers
{
  [Route("api/orders")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    private readonly IOrder _order;

    public OrderController(IOrder order)
    {
      _order = order;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderRdo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult>  GetList()
    {
      var orders = await _order.Get();
      if (orders == null) return NotFound();
      return Ok(orders);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderRdo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
      var order = await _order.Get(id);
      if (order == null) return NotFound();
      return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
      var orderId = await _order.Create(dto);
      return Ok(orderId);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto dto)
    {
      var orderId = await _order.Update(id, dto);
      return Ok(orderId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      var orderId = await _order.Delete(id);
      if (orderId == 0) return NotFound();
      return Ok(orderId);
    }
  }
}
