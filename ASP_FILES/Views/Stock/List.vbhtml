@ModelType IEnumerable(Of ASP_FILES.TABLE1)

@Code
    ViewData("Title") = "List"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div style="width :100%;text-align :center;">
    <h6>STOCK MANAGEMENT</h6>
</div>
<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table table-bordered table-hover table-responsive" style="font-size :smaller ">
    <tr>
                                @*<th>
                @Html.DisplayNameFor(Function(model) model.RID1)
            </th>
            <th>
            @Html.DisplayNameFor(Function(model) model.RID)
        </th>
         <th>
        @Html.DisplayNameFor(Function(model) model.PPRICE)
    </th>
                 <th>
        @Html.DisplayNameFor(Function(model) model.TAX)
    </th>
    <th>
        @Html.DisplayNameFor(Function(model) model.TVAL)
    </th>
                        <th>
        @Html.DisplayNameFor(Function(model) model.SSTA)
    </th>
                                *@
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.TYPE)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.PART_NO)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.PARTI)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.MRP)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.QTY)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.TOTAL)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.RACKNO)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.STOTAL)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.UNIT)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.SPRICE)
        </th>
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.EOR)
        </th>
        @* <th>
                @Html.DisplayNameFor(Function(model) model.DPCODE)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.lmodi)
            </th>*@
        <th nowrap bgcolor="#00ffe1">
            @Html.DisplayNameFor(Function(model) model.grop)
        </th>
        @*<th>
                @Html.DisplayNameFor(Function(model) model.AEDT)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.USER1)
            </th>*@
        <th nowrap bgcolor="#00ffe1">ITEM OPERATION</th>
    </tr>

    @For Each item In Model
        Dim currentItem = item
        @<tr>
                                                    @*<td>
                @Html.DisplayFor(Function(modelItem) currentItem.RID1)
            </td>
            <td>
            @Html.DisplayFor(Function(modelItem) currentItem.RID)
        </td>
        <td nowrap>
        @Html.DisplayFor(Function(modelItem) currentItem.PPRICE)
    </td>
                          <td nowrap>
            @Html.DisplayFor(Function(modelItem) currentItem.TAX)
        </td>
        <td nowrap>
            @Html.DisplayFor(Function(modelItem) currentItem.TVAL)
        </td>
                                      <td nowrap>
            @Html.DisplayFor(Function(modelItem) currentItem.SSTA)
        </td>
                                                    *@
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.TYPE)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.PART_NO)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.PARTI)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.MRP)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.QTY)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.TOTAL)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.RACKNO)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.STOTAL)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.UNIT)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.SPRICE)
            </td>
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.EOR)
            </td>
            @* <td>
                    @Html.DisplayFor(Function(modelItem) currentItem.DPCODE)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) currentItem.lmodi)
                </td>*@
            <td nowrap>
                @Html.DisplayFor(Function(modelItem) currentItem.grop)
            </td>
            @*<td>
                    @Html.DisplayFor(Function(modelItem) currentItem.AEDT)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) currentItem.USER1)
                </td>*@
            <td nowrap>
                @Html.ActionLink("Edit", "Edit", New With {.id = item.RID1}) |
                @Html.ActionLink("Details", "Details", New With {.id = item.RID1}) |
                @Html.ActionLink("Delete", "Delete", New With {.id = item.RID1})
            </td>
        </tr>
    Next

</table>
