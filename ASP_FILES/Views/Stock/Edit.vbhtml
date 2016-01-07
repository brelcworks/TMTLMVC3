@ModelType ASP_FILES.TABLE1

@Code
    ViewData("Title") = "Edit"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Edit</h2>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
         <table class="table table-bordered table-hover table-responsive" style="font-size :smaller">
             <tr>
                 <td>
                     @Html.LabelFor(Function(MODEL) MODEL.RID)
                 </td>
                 <td colspan="3">
                     @Html.EditorFor(Function(model) model.RID)
                     @Html.ValidationMessageFor(Function(model) model.RID)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.RID1)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.RID1)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.STOTAL)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.STOTAL)
                     @Html.ValidationMessageFor(Function(model) model.STOTAL)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.PARTI)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.PARTI)
                     @Html.ValidationMessageFor(Function(model) model.PARTI)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.TVAL)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.TVAL)
                     @Html.ValidationMessageFor(Function(model) model.TVAL)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.PART_NO)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.PART_NO)
                     @Html.ValidationMessageFor(Function(model) model.PART_NO)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.TYPE)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.TYPE)
                     @Html.ValidationMessageFor(Function(model) model.TYPE)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.MRP)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.MRP)
                     @Html.ValidationMessageFor(Function(model) model.MRP)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.UNIT)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.UNIT)
                     @Html.ValidationMessageFor(Function(model) model.UNIT)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.TAX)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.TAX)
                     @Html.ValidationMessageFor(Function(model) model.TAX)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.RACKNO)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.RACKNO)
                     @Html.ValidationMessageFor(Function(model) model.RACKNO)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.SPRICE)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.SPRICE)
                     @Html.ValidationMessageFor(Function(model) model.SPRICE)
                 </td>

                 <td>
                     @Html.LabelFor(Function(model) model.EOR)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.EOR)
                     @Html.ValidationMessageFor(Function(model) model.EOR)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.PPRICE)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.PPRICE)
                     @Html.ValidationMessageFor(Function(model) model.PPRICE)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.grop)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.grop)
                     @Html.ValidationMessageFor(Function(model) model.grop)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.QTY)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.QTY)
                     @Html.ValidationMessageFor(Function(model) model.QTY)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.DPCODE)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.DPCODE)
                     @Html.ValidationMessageFor(Function(model) model.DPCODE)
                 </td>
             </tr>
             <tr>
                 <td>
                     @Html.LabelFor(Function(model) model.TOTAL)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.TOTAL)
                     @Html.ValidationMessageFor(Function(model) model.TOTAL)
                 </td>
                 <td>
                     @Html.LabelFor(Function(model) model.USER1)
                 </td>
                 <td>
                     @Html.EditorFor(Function(model) model.USER1)
                     @Html.ValidationMessageFor(Function(model) model.USER1)
                 </td>
             </tr>
         </table>
        <p>
            <input type="submit" value="Edit" class="btn btn-primary btn-xs"/>
        </p>
    </fieldset>
End Using

<div>
    @Html.ActionLink("Back to List", "List")
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
