using DataSeeder.DataSets;
using DataSeeder.DataSets.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DataSeeder.Controllers;

public record DataSetRequest(string Id);

[ApiController]
[Route("[controller]")]
public class DataSetController : ControllerBase
{
    private readonly DataClearer _dataClearer;
    private readonly Dictionary<string, IDataSet> _dataSets;
    private readonly MongoContext _context;

    public DataSetController(
        DataClearer dataClearer,
        Dictionary<string, IDataSet> dataSets,
        MongoContext context)
    {
        _dataClearer = dataClearer;
        _dataSets = dataSets;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> SeedDataSet([FromBody] DataSetRequest request)
    {
        await _dataClearer.ClearAllData();
        await _dataSets[request.Id].Seed(_context);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteDataSet([FromBody] DataSetRequest request)
    {
        // await _dataSets[request.Id].Delete(_context);
        
        return Ok();
    }
}