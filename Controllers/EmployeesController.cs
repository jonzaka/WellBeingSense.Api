using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Data;
using WellBeingSense.Api.Models;

namespace WellBeingSense.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
    {
        return await _context.Employees.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return NotFound();
        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        if (id != employee.Id) return BadRequest();

        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
