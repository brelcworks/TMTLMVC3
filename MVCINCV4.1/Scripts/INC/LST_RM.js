$(function () {
    $(document).ready(function () {
        var source =
   {
       datatype: "json",
       datafields: [
            { name: 'RECID1', type: 'string' },
            { name: 'SID', type: 'string' },
            { name: 'SNAME', type: 'string' },
            { name: 'CUST', type: 'string' },
            { name: 'STYPE', type: 'string' },
            { name: 'CCATE', type: 'string' },
            { name: 'DOS', type: 'date' },
            { name: 'CDATI', type: 'date' },
            { name: 'ENGINE_No', type: 'string' },
            { name: 'HMR', type: 'string' },
            { name: 'STA', type: 'string' },
       ],
       url: '/Pop/List_rm'
   };
        var editrow = -1;
        var dataAdapter = new $.jqx.dataAdapter(source);
        var height1 = $(window).height() - 170;
        // initialize jqxGrid
        $("#grid").jqxGrid(
            {
                width: '99%',
                height: height1,
                source: dataAdapter,
                sortable: true,
                filterable: true,
                altrows: true,
                theme: 'ui-redmond',
                showtoolbar: true,
                showstatusbar: true,
                editable: true,
                columns: [
                    { text: "RECORD NO", datafield: "RECID1", hidden: true },
                    { text: "SITE ID", datafield: "SID" },
                    { text: "SITE NAME", datafield: "SNAME" },
                    { text: "CUSTOMER", datafield: "CUST" },
                    { text: "ISSUE TYPE", datafield: "STYPE" },
                    { text: "ISSUE CATEGORY", datafield: "CCATE" },
                    { text: "DT. OF ISSUE", datafield: "DOS", cellsformat: 'dd-MM-yyyy HH:mm' },
                    { text: "DT. OF CLOSURE", datafield: "CDATI", cellsformat: 'dd-MM-yyyy HH:mm' },
                    { text: "ENGINE NO", datafield: "ENGINE_No" },
                    { text: "HMR", datafield: "HMR" },
                    { text: "ISSUE STATUS", datafield: "STA" },
                    {
                        text: 'Details', datafield: 'Details', columntype: 'button', width: 80, sortable: false, filterable: false, cellsrenderer: function () {
                            return "Details";
                        }, buttonclick: function (row) {
                            editrow = row;
                            var offset = $("#grid").offset();
                            $("#popupWindow").jqxWindow({ position: 'top, left' });
                            var dataRecord = $("#grid").jqxGrid('getrowdata', editrow);
                            var ens = dataRecord.RECID1;
                            var td = dataRecord.ENGINE_No;
                            var options = { "backdrop": "static", keyboard: true };
                            $.ajax({
                                type: "GET",
                                url: '/Pop/PMR_Dtls',
                                contentType: "application/json; charset=utf-8",
                                data: { "ens": ens },
                                datatype: "json",
                                success: function (data) {
                                    debugger;
                                    $('#myModalContent').html(data);
                                    $('#myModal').modal(options);
                                    $('#myModal').modal('show');
                                },
                                error: function () {
                                    alert("Dynamic content load failed.");
                                }
                            });
                        }
                    }
                ],
                rendertoolbar: function (toolbar) {
                    var me = this;
                    var container = $("<div style='margin: 5px;'></div>");
                    var input = $("<input id='searchField' type='text' style='float: left;' />");
                    var btn1 = $("<a href='/Pop/Create' style='float: left; margin-left:5px; position: relative; top: -4px;' role='button'>Add New Site</a>");
                    container.append(input);
                    container.append(btn1);
                    var tbl = $("<table></table>");
                    var tr = $("<tr></tr>");
                    var td2 = $("<td></td>");
                    td2.append($('#dp1'));
                    tr.append(td2);
                    var td1 = $("<td></td>");
                    td1.append(container);
                    tr.append(td1);
                    tbl.append(tr);
                    toolbar.append(tbl);
                    btn1.jqxButton({ template: "success" });
                    $("#searchField").jqxInput({ placeHolder: "SEARCH ITEM", height: 23, width: 200, minLength: 1, theme: 'energyblue' });
                    var oldVal = "";
                    input.on('keydown', function (event) {
                        if (input.val().length >= 2) {
                            if (me.timer) {
                                clearTimeout(me.timer);
                            }
                            if (oldVal != input.val()) {
                                me.timer = setTimeout(function () {
                                    addFiter(input.val());
                                }, 1000);
                                oldVal = input.val();
                            }
                        }
                        else {
                            $("#grid").jqxGrid('clearfilters');
                        }
                    });
                },
                renderstatusbar: function (sbar) {
                    sbar.append($('#dv1'));
                    $('#btnEx').jqxButton({ template: "info" });
                    $('#btnBCK').jqxButton({ template: "danger" });
                    $("#dtFrm").jqxInput({ placeHolder: "From Date", height: 25, width: 200, minLength: 1 });
                    $("#dtTo").jqxInput({ placeHolder: "To Date", height: 25, width: 200, minLength: 1 });
                }
            });
        $("#closbtn").click(function () {
            $('#myModal').modal('hide');
        });
        $("#popupWindow").jqxWindow({
            width: '100%', height: '90%', theme: 'energyblue', isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
        });

        $("#popupWindow").on('open', function () {
            $("#sid").jqxInput('selectAll');
        });
        $("#Cancel").jqxButton({ theme: 'energyblue' });
    });
    $('#dtFrm').datepicker({ dateFormat: 'dd-M-yy' });
    $('#dtTo').datepicker({ dateFormat: 'dd-M-yy' });
    
    function addFiter(value) {
        $("#grid").jqxGrid('clearfilters');
        var filtertype = 'stringfilter';
        var filtergroup = new $.jqx.filter();
        var filter = filtergroup.createfilter('stringfilter', value, 'CONTAINS');
        filtergroup.addfilter(2, filter);
        // add the filters.
        var searchColumnIndex = $("#dp1").jqxDropDownList('selectedIndex');
        switch (searchColumnIndex) {
            case 0:
                $("#grid").jqxGrid('addfilter', 'SNAME', filtergroup);
                break;
            case 1:
                $("#grid").jqxGrid('addfilter', 'SID', filtergroup);
                break;
            case 2:
                $("#grid").jqxGrid('addfilter', 'ENGINE_No', filtergroup);
                break;
        }
        // apply the filters.
        $("#grid").jqxGrid('applyfilters');
    }
    $("#dp1").jqxDropDownList({
        autoDropDownHeight: true, selectedIndex: 0, width: 200,
        source: [
            'SITE NAME', 'SITE ID', 'ENGINE NO'
        ]
    });
});