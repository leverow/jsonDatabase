using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task11.Attributes;

namespace Task11.Controllers;

[ApiController]
[Route("api/[controller]")]
[Role("Administrator")]
public class CalculateController : ControllerBase
{
    [HttpPost]
    public IActionResult Do(int k, int n)
    {
        var logicResult = CountHowManyKContainsInN(k, n);

        if (!logicResult.Item2) return BadRequest($"Qiymatlar kiritilishda xatolik yuz berdi. Sabab: {logicResult.Item3}");
        
        return Ok(logicResult.Item1);
    }
    private (int, bool, string?) CountHowManyKContainsInN(int k, int n)
    {
        var count = 0;
        if (k is not > 0 and < int.MaxValue) return (0, false, "K 0dan katta bolishi kerak va k max intdan kichik bolishi kerak");
        if (n is not > 0 and < int.MaxValue) return (0, false, "N 0dan katta bolishi kerak va n max intdan kichik bolishi kerak");
        if (k > n) return (0, false, "K soni N sonidan kichik bo'lishi kerak");

        for (int i = 0; i <= n; i++)
            count += i.ToString().Split(k.ToString()).Length - 1;
        return (count,true,null);
    }
}