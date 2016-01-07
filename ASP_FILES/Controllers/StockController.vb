Imports System.Data.Entity
Public Class StockController
    Inherits System.Web.Mvc.Controller

    Dim dc As DB1Entities = New DB1Entities

    Function List() As ActionResult
        Return View(dc.TABLE1.ToList())
    End Function
    Function Details(ByVal id As Integer) As ActionResult
        Return View(dc.TABLE1.Find(id))
    End Function
    Function Create() As ActionResult
        Return View()
    End Function
    <HttpPost, ValidateAntiForgeryToken>
    Function Create(e As TABLE1) As ActionResult
        Using (dc)
            dc.TABLE1.Add(e)
            dc.SaveChanges()
        End Using
        Return RedirectToAction("List")
    End Function
    Function Edit(ByVal id As Integer) As ActionResult
        Return View(dc.TABLE1.Find(id))
    End Function
    <HttpPost, ValidateAntiForgeryToken>
    Function Edit(e1 As TABLE1) As ActionResult
        dc.Entry(e1).State = EntityState.Modified
        dc.SaveChanges()
        Return RedirectToAction("List")
    End Function
    Function Delete(ByVal id As Integer) As ActionResult
        Return View(dc.TABLE1.Find(id))
    End Function
    <HttpPost, ActionName("Delete")>
    Function delete_conf(ByVal id As Integer)
        Dim st As TABLE1 = dc.TABLE1.Find(id)
        dc.TABLE1.Remove(st)
        dc.SaveChanges()
        Return RedirectToAction("List")
    End Function
End Class