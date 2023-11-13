using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientApp.Pages
{
  public partial class Contact : ComponentBase
  {
    public string? Address { get; set; }
    public string? Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
      Address ="mailto:" + "aazimi3731@gmail.com";
      Content = "Contact us";
    }
    
  }
}
