@ModelType ASP_FILES.TABLE1

@Code
    ViewData("Title") = "Delete"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h4>Are you sure you want to delete this?</h4>
<fieldset>
    @*<div class="display-label">
                @Html.DisplayNameFor(Function(model) model.RID1)
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.RID1)
            </div>

            <div class="display-label">
                @Html.DisplayNameFor(Function(model) model.RID)
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.RID)
            </div>
                <div class="display-label">
                @Html.DisplayNameFor(Function(model) model.SSTA)
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.SSTA)
            </div>
                 <div class="display-label">
                @Html.DisplayNameFor(Function(model) model.DPCODE)
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.DPCODE)
            </div>

            <div class="display-label">
                @Html.DisplayNameFor(Function(model) model.lmodi)
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.lmodi)
            </div>
                 <div class="display-label">
            @Html.DisplayNameFor(Function(model) model.AEDT)
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.AEDT)
        </div>

        <div class="display-label">
            @Html.DisplayNameFor(Function(model) model.USER1)
        </div>
        <div class="display-field">
            @Html.DisplayFor(Function(model) model.USER1)
        </div>
    *@
    <table class="table table-bordered table-hover table-responsive" style="font-size :smaller">
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.TYPE)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.TYPE)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.PART_NO)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.PART_NO)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.PARTI)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.PARTI)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.MRP)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.MRP)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.QTY)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.QTY)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.TOTAL)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.TOTAL)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.RACKNO)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.RACKNO)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.TAX)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.TAX)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.TVAL)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.TVAL)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.STOTAL)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.STOTAL)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.PPRICE)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.PPRICE)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.UNIT)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.UNIT)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.SPRICE)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.SPRICE)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.EOR)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.EOR)
            </td>
        </tr>
        <tr>
            <td>
                @Html.DisplayNameFor(Function(model) model.grop)
            </td>
            <td>
                @Html.DisplayFor(Function(model) model.grop)
            </td>
        </tr>
    </table>
</fieldset>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<p>
        <input type="submit" value="Delete" /> |
        @Html.ActionLink("Back to List", "List")
    </p>
End Using
