# **@** RazorToString

Render razor template to string.

## Usage

```csharp
// Make sure to use Razor pages or MVC in your project to add the engine.
services.AddControllersWithViews(); // Add MVC
services.AddRazorPages();           // Add Razor
```

```csharp
// register the service 'RazorToStringRenderer' and configure
// views path.
services.AddRazorToString(config => config.ViewsPath = "/Emails");
```

- Create The model and template in the specified path.

![Image](./assets/model.png)

- Model must be derived from `RazorViewModel`

```csharp
public class EmailTemplate : RazorViewModel
{
    public string Content { get; set; }

    public string Url { get; set; }
}
```

- Create template

```csharp
@model Sample.RazorEmailTemplate.Emails.EmailTemplate
@{
  Layout = "./_Layout.cshtml";
  ViewBag.Title = "RazorToString";
}

@if (ViewBag.Message is {})
{
  <div>
    ViewBag.Content: @ViewBag.Message
  </div>
}

<div>
  @Model.Content
  <a href="@Model.Url"></a>
</div>
```

- To use the template render it using `IRazorToStringRenderer` service

```csharp
private readonly IRazorToStringRenderer _razor;

public HomeController(IRazorToStringRenderer razor) => _razor = razor;

[HttpGet("/")]
public async Task<ActionResult> SendEmail()
{
    var result = await _razor.RenderAsync(new EmailTemplate
    {
        Content = "Hi Shaheen, to confirm your email please use this link.",
        Url = "https://github.com/xshaheen/RazorToString",
        ViewData =
        {
            ["Message"] = "This is message"
        }
    });

    // just output the string
    return Content($"The email is\n\n{result}");
}
```

-- see sample project for more details
