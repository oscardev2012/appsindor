using Microsoft.AspNetCore.Mvc;

namespace SafeManagement.Data;

[ApiController]
[Route("api/safe")]
public class SafeApiController : ControllerBase
{
    private readonly SafeHubService _hub;
    public SafeApiController(SafeHubService hub) => _hub = hub;

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { ok = true, database = "IndorDB" });

    [HttpGet("snapshot")]
    public async Task<IActionResult> Snapshot([FromQuery] string portal = "operations")
    {
        try { return Ok(await _hub.Snapshot(portal)); }
        catch (Exception ex) { return StatusCode(503, new { ok = false, error = ex.Message }); }
    }

    [HttpPost("requests")]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto dto)
        => Ok(await _hub.CreateRequest(dto));

    [HttpPatch("requests/{id}")]
    public async Task<IActionResult> UpdateRequest(string id, [FromBody] UpdateRequestDto dto)
    {
        var row = await _hub.UpdateRequest(id, dto);
        return row is null ? NotFound() : Ok(row);
    }

    [HttpPost("work-orders")]
    public async Task<IActionResult> CreateWorkOrder([FromBody] CreateWorkOrderDto dto)
        => Ok(await _hub.CreateWorkOrder(dto));

    [HttpPost("work-orders/{id}/check-in")]
    public async Task<IActionResult> CheckIn(string id)
    {
        var row = await _hub.CheckIn(id);
        return row is null ? NotFound() : Ok(row);
    }

    [HttpPatch("work-orders/{id}")]
    public async Task<IActionResult> UpdateWorkOrder(string id, [FromBody] UpdateWorkOrderDto dto)
    {
        var row = await _hub.UpdateWorkOrder(id, dto);
        return row is null ? NotFound() : Ok(row);
    }

    [HttpPost("messages")]
    public async Task<IActionResult> Message([FromBody] CreateMessageDto dto)
        => Ok(await _hub.PostMessage(dto));

    [HttpPost("promotions")]
    public async Task<IActionResult> Promotion([FromBody] CreatePromotionDto dto)
        => Ok(await _hub.CreatePromotion(dto));

    [HttpPost("campaigns")]
    public async Task<IActionResult> Campaign([FromBody] CreateCampaignDto dto)
        => Ok(await _hub.CreateCampaign(dto));

    [HttpPost("leads")]
    public async Task<IActionResult> Lead([FromBody] CreateLeadDto dto)
        => Ok(await _hub.CreateLead(dto));

    [HttpPost("clients")]
    public async Task<IActionResult> Client([FromBody] CreateClientDto dto)
        => Ok(await _hub.CreateClient(dto));

    [HttpPost("gift-cards")]
    public async Task<IActionResult> GiftCard([FromBody] CreateGiftCardDto dto)
        => Ok(await _hub.CreateGiftCard(dto));

    [HttpGet("email/status")]
    public IActionResult EmailStatus([FromServices] SafeMailer mail) => Ok(new { ready = mail.Ready });

    [HttpPost("email")]
    public async Task<IActionResult> Email([FromBody] SendEmailDto dto, [FromServices] SafeMailer mail, CancellationToken ct)
    {
        if (!mail.Ready) return StatusCode(503, new { ok = false, error = "Company email is not configured." });
        var to = dto.To?.Trim() ?? "";
        var subject = (dto.Subject ?? "").Trim();
        var body = dto.Body ?? "";
        if (!SafeMailer.IsAddress(to)) return BadRequest(new { ok = false, error = "Invalid recipient email." });
        if (subject.Length is 0 or > 200 || body.Length is 0 or > 20000)
            return BadRequest(new { ok = false, error = "Subject or message is empty or too long." });
        if (!mail.Allow(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown")) return StatusCode(429, new { ok = false, error = "Too many emails. Try again later." });
        try
        {
            await mail.SendAsync(to, subject, body, dto.ReplyTo, dto.ReplyName, ct);
            return Ok(new { ok = true });
        }
        catch (Exception ex)
        {
            return StatusCode(502, new { ok = false, error = ex.Message, detail = ex.GetBaseException().Message });
        }
    }
}
