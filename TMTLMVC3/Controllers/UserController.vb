Public Class UserController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /User

    Function Index() As ActionResult
        Return View()
    End Function
    Function list() As ActionResult
        Dim users = New List(Of USER1)
        Dim dc As TMTLMVC = New TMTLMVC()
        users = dc.USER1.ToList()
        Return View(users)
    End Function
End Class