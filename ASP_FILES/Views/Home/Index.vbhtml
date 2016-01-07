@ModelType ASP_FILES.user1 
@Code
    ViewData("Title") = "Home Page"
    Layout = Nothing
End Code

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>@ViewData("Title")</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <script src="~/Scripts/jquery-1.9.1.js"></script>
    <script src="~/Scripts/bootstrap.js"></script>
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <header>
        <nav class="navbar navbar-inverse" style="background-color :indigo  ">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                </div>

                <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                    <ul class="nav navbar-nav">
                        <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    </ul>
                    <ul class="nav navbar-nav">
                        <li>@Html.ActionLink("About", "About", "Home")</li>
                    </ul>
                    <ul class="nav navbar-nav">
                        <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                    </ul>
                    <div class="navbar-form navbar-right" role="search">
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)
    @<fieldset>
        @Html.TextBoxFor(Function(model) model.uid, New With {.HTMLATTRIBUTES = New With {.PLACEHOLDER = "uSER NAME"}})
        @Html.PasswordFor(Function(model) model.pass)
        <INPUT type="submit" class="btn btn-primary btn-xs" value="Login"/>
    </fieldset>
End Using 
                    </div>
                </div>
            </div>
        </nav>
    </header>
    <div id = "body" >
        <h1>@ViewData("Title").</h1>
        <h2>@ViewData("Message")</h2>
    </div>
    <footer>
<div Class="content-wrapper">
            <div Class="float-left">
                <p>&copy; @DateTime.Now.Year - ASP.NET MVC Application</p>
            </div>
        </div>
    </footer>
    @Section Scripts
        @Scripts.Render("~/bundles/jqueryval")
    End Section
</body>
</html>
