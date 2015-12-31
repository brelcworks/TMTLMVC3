@Modeltype list(of TMTLMVC3.USER1) 

@Code
    Layout = Nothing
    Dim grid = New WebGrid(source:=Model, canPage:=True, rowsPerPage:=10)
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <script src="~/Scripts/jquery-1.9.1.js"></script>
    <script src="~/Scripts/bootstrap.js"></script>
    <title>list</title>
</head>
<body>
    <div>
        @grid.GetHtml
    </div>
</body>
</html>
