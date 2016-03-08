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
        // initialize jqxGrid
        $("#grid").jqxGrid(
            {
                width: '98%',
                height: '100%',
                source: dataAdapter,
                sortable: true,
                filterable: true,
                altrows: true,
                theme: 'energyblue',
                showtoolbar: true,
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
                    var span = $("<span style='float: left; margin-top: 5px; margin-right: 4px;'>Search Site: </span>");
                    var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: left; width: 223px;' />");
                    var btn1 = $("<a href='/Pop/Create' style='float: left; margin-left:5px; position: relative; top: -4px;'>Add New Site</a>");
                    var btn2 = $("<a href='/Pop/ExportData' style='float: left; margin-left:5px; position: relative; top: -4px;'>Export Data</a>");
                    var btn3 = $("<a href='/Pop/List_rm1' style='float: left; margin-left:5px; position: relative; top: -4px;'>Manage Issues</a>");
                    toolbar.append(container);
                    container.append(span);
                    container.append(input);
                    container.append(btn1);
                    container.append(btn2);
                    container.append(btn3);
                    btn1.jqxButton({ template: "success" });
                    btn2.jqxButton({ template: "warning" });
                    btn3.jqxButton({ template: "primary" });
                    if (theme != "") {
                        input.addClass('jqx-widget-content-' + theme);
                        input.addClass('jqx-rc-all-' + theme);
                    }
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
        $("#grid").jqxGrid('addfilter', 'SNAME', filtergroup);
        // apply the filters.
        $("#grid").jqxGrid('applyfilters');
    }
});