namespace server.Utils
{
  public class IResponse
  {
    public required string Message { get; set; }
    public required int Status { get; set; }
    public required dynamic Data { get; set; }
  }
  
  public class IResponseSocket
  {
      public required string Message { get; set; }
      public required string Type { get; set; }
      public required dynamic Data { get; set; }
  }

  public class Response
  {
    public IResult SuccessResponse(string message, dynamic data)
    {
      return Results.Ok(new IResponse
      {
        Message = message,
        Data = data,
        Status = 200
      });
    }

    public IResult NotFoundResponse(string message)
    {
      return Results.NotFound(new IResponse
      {
        Message = message,
        Data = "",
        Status = 404
      });
    }

    public IResult BadRequestResponse(string message)
    {
      return Results.BadRequest(new IResponse
      {
        Message = message,
        Data = "",
        Status = 500
      });
    }
    
    public IResponseSocket SocketResponse(string type, string message, dynamic data)
    {
      var parsed = JsonUtils.ParseJson(data);
      return new IResponseSocket
      {
        Type = type,
        Message = message,
        Data = parsed,
      };
    }
  }
}
