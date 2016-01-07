Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Message") = "Welcome Guest"

        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your app description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
    <HttpPost, ValidateAntiForgeryToken>
    Function Index(u As user1) As ActionResult
        If ModelState.IsValid Then
            ' this is check validity
            Using dc As New DB1Entities()
                Dim v = dc.user1.Where(Function(a) a.uid.Equals(u.uid) AndAlso a.pass.Equals(u.pass)).FirstOrDefault()
                If v IsNot Nothing Then
                    Session("LogedUserID") = v.uid.ToString()
                    Session("LogedUserFullname") = v.fname.ToString()
                    Return RedirectToAction("AfterLogin")
                Else
                    ViewData("Message") = "Login Failed"
                End If
            End Using
        End If
        Return View(u)
    End Function
    Function AfterLogin() As ActionResult
        If Session("LogedUserID") IsNot Nothing Then
            Return View()
        Else
            Return RedirectToAction("Index")
        End If
    End Function
End Class
