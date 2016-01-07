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
                        <ul class="nav navbar-nav">
                            <li>@Html.ActionLink("Stock", "List", "Stock")</li>
                        </ul>
                        <div class="navbar-form navbar-right" role="search">
                           
                        </div>
                    </div>
                </div>
            </nav>
        </header>
        <div id="body">
            @RenderSection("featured", required:=False)
            <section class="content-wrapper main-content clear-fix">
                @RenderBody()
            </section>
        </div>
        <footer>
            <div class="content-wrapper">
                <div class="float-left">
                    <p>&copy; @DateTime.Now.Year - ASP.NET MVC Application</p>
                </div>
            </div>
        </footer>

        @Scripts.Render("~/bundles/jquery")
        @RenderSection("scripts", required:=False)
    </body>
</html>
