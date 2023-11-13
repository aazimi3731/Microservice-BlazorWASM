using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientApp.Pages
{
  public partial class CheckoutComplete : ComponentBase
  {
    [Inject]
    public IPaymentService PaymentService { get; set; }

    public string? Message { get; set; }

    protected override async Task OnInitializedAsync()
    {
      Message = "Thanks for your order. You'll soon enjoy our delicious pies!";

      PaymentService.Send(Message);

      await base.OnInitializedAsync();
    }
  }
}
