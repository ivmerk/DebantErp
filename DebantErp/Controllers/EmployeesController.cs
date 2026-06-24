using DebantErp.BL.Employee;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Работники». Требует логина (deny-by-default fallback policy).
[Route("workspace/employees")]
public class EmployeesController : WorkspaceBaseController
{
    private readonly IEmployee _employee;
    private readonly IEmployeeDetails _employeeDetails;

    public EmployeesController(IEmployee employee, IEmployeeDetails employeeDetails)
    {
        _employee = employee;
        _employeeDetails = employeeDetails;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var all = (await _employee.Get())
            .Where(e => bool.TryParse(e.IsActual, out var actual) && actual) // мягко удалённых не показываем
            .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)                // сортировка по фамилии
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var pageItems = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var rows = new List<EmployeeRow>();
        foreach (var e in pageItems)
        {
            var details = await _employeeDetails.GetEmployeeDetailsByEmployeeId(e.Id);
            rows.Add(new EmployeeRow { Employee = e, Details = details });
        }

        return View(new EmployeeListViewModel { Items = rows, Page = page, TotalPages = totalPages });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Проверьте корректность заполнения полей.";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            await _employee.Create(dto);
            TempData["Success"] = $"Работник «{dto.LastName} {dto.FirstName}» добавлен.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Не удалось добавить работника.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Проверьте корректность заполнения полей.";
            return RedirectToAction(nameof(Index), new { page });
        }
        await _employee.Update(id, dto);
        TempData["Success"] = "Данные работника обновлены.";
        return RedirectToAction(nameof(Index), new { page });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _employee.Delete(id);
        TempData["Success"] = "Работник удалён.";
        return RedirectToAction(nameof(Index), new { page });
    }
}
