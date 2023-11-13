namespace ClientApp.Interfaces
{
  public interface IMicroserviceReqResp
  {
    Task<T?> SendGetRequest<T>(string url_);

    Task<T?> SendPostRequest<T>(string url_, T? object_);
  }
}
