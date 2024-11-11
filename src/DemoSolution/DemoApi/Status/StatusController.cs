namespace HtTemplate.Status;

public class StatusController : ControllerBase
{

    // "Document" resouce.

    [HttpGet("/status")]
    public async Task<ActionResult> GetTheStatus()
    {
        return Ok("This is the status for right now.");
    }

    [HttpGet("/status/{year}/{month}/{day}")]
    public async Task<ActionResult> GetTheStatus(int year, int month, int day)
    {
        return Ok($"The Status for {year}/{month}/{day}");
    }


}
// anytime we call a process over a network, you MUST use async and await at a minimum
