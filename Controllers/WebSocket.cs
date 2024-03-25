using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Data;
using server.Utils;
using server.Extensions;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Responses;
using server.Dtos;

[Route("[controller]")]
[ApiController]
public class WebSocketController : ControllerBase
{
  private readonly DBContext db;
  private readonly Response res;

  public WebSocketController(DBContext _db)
  {
    db = _db;
    res = new Response();
  }

  [HttpGet("/ws")]
  public async Task ConnectWebSocket()
  {
    await WSManager.HandleWebSocket(HttpContext);
  }
}