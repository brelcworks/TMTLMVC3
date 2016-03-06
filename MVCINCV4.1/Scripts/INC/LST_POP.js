$(function () {
    $(".dtl").click(function () {
        debugger;
        var $btdtl = $(this);
        var dtlid = $btdtl.attr('data-id');
        var dtlopt = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: '/Pop/Details',
            contentType: "application/json; charset=utf-8",
            data: { "id": dtlid },
            datatype: "json",
            success: function (data) {
                debugger;
                $('#myModalContent').html(data);
                $('#myModal').modal(dtlopt);
                $('#myModal').modal('show');
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });

    $(".dtl1").click(function () {
        debugger;
        var $btdtl = $(this);
        var dtlid = $btdtl.attr('data-id');
        var dtlopt = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: '/Pop/View_PMR',
            contentType: "application/json; charset=utf-8",
            data: { "id": dtlid },
            datatype: "json",
            success: function (data) {
                debugger;
                $('#myModalContent').html(data);
                $('#myModal').modal(dtlopt);
                $('#myModal').modal('show');
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
    $(".anchorDetail").click(function () {
        debugger;
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var td = $buttonClicked.text();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: '/Pop/Delete',
            contentType: "application/json; charset=utf-8",
            data: { "id": id },
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
    });


    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });

    $(".anchorDetail1").click(function (e) {
        InitializeDialog($("#departmentdialog"));
        var uri = '/Pop/View_PMR';
        debugger;
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json; charset=utf-8",
            data: { "id": id },
            datatype: "json",
            success: function (data) {
                debugger;
                $('#myModalContent1').html(data);
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
        $("#departmentdialog").dialog("open");
    });
    //Method to Initialize the DialogBox
    function InitializeDialog($element) {
        $element.dialog({
            autoOpen: false,
            resizable: true,
            width: 1000,
            position: { my: "left top", at: "left+50 top+100", of: window },
            draggable: true,
            title: "LIST OF SERVICE",
            modal: true,
            show: 'slide',
            closeText: 'x',
            dialogClass: 'alert',
            closeOnEscape: true,
            close: function () {
                $(this).dialog('close');
            }
        });
    }

    $(document).ready(function () {
        var source =
   {
       datatype: "json",
       datafields: [
            { name: 'RID', type: 'string' },
            { name: 'SID', type: 'string' },
            { name: 'CNAME', type: 'string' },
            { name: 'SNAME', type: 'string' },
            { name: 'ENS', type: 'string' },
            { name: 'RAT_PH', type: 'string' },
            { name: 'PHASE', type: 'string' },
            { name: 'CPN', type: 'string' },
            { name: 'PHNO', type: 'string' },
            { name: 'DOC', type: 'date' },
            { name: 'lhmr', type: 'string' },
       ],
       url: '/Pop/List_POP'
   };
        var editrow = -1;
        var dataAdapter = new $.jqx.dataAdapter(source);
        // initialize jqxGrid
        $("#grid").jqxGrid(
            {
                width: '99%',
                source: dataAdapter,
                sortable: true,
                filterable: true,
                altrows: true,
                theme: 'energyblue',
                editable: true,
                showtoolbar: true,
                columns: [
                    { text: "RECORD NO", datafield: "RID", hidden: true },
                    { text: "SITE ID", datafield: "SID" },
                    { text: "SITE NAME", datafield: "SNAME" },
                    { text: "CUSTOMER", datafield: "CNAME" },
                    { text: "ENGINE NO", datafield: "ENS" },
                    { text: "RATING", datafield: "RAT_PH" },
                    { text: "PHASE", datafield: "PHASE" },
                    { text: "CONTACT PERSON", datafield: "CPN" },
                    { text: "PHONE NO", datafield: "PHNO" },
                    { text: "DT. OF COMMISSIONING", datafield: "DOC", cellsformat: 'dd-MM-yyyy', width: '10%' },
                    { text: "HMR", datafield: "lhmr" },
                    {
                        text: 'Details', datafield: 'Details', columntype: 'button', width: 80, sortable: false, filterable: false, cellsrenderer: function () {
                            return "Details";
                        },
                        buttonclick: function (row) {
                            editrow = row;
                            var offset = $("#grid").offset();
                            var dataRecord = $("#grid").jqxGrid('getrowdata', editrow);                            var ens = dataRecord.RID;
                            var options = { "backdrop": "static", keyboard: true };
                            $.ajax({
                                type: "GET",
                                url: '/Pop/Details',
                                contentType: "application/json; charset=utf-8",
                                data: { "id": ens },
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
                    },
                    {
                        text: 'Add Issue', datafield: 'Add', columntype: 'button', width: 80, sortable: false, filterable: false, cellsrenderer: function () {
                            return "Add Issue";
                        },
                        buttonclick: function (row) {
                            editrow = row;
                            var offset = $("#grid").offset();
                            var dataRecord = $("#grid").jqxGrid('getrowdata', editrow);                            var ens = dataRecord.RID;
                            window.location = "/Pop/Add_Pmr?id=" + ens;
                        }
                    },
                    {
                        text: 'History', datafield: 'History', columntype: 'button', width: 80, sortable: false, filterable: false, cellsrenderer: function () {
                            return "History";
                        },
                        buttonclick: function (row) {
                            editrow = row;
                            var offset = $("#grid").offset();
                            var dataRecord = $("#grid").jqxGrid('getrowdata', editrow);                            var ens = dataRecord.ENS;
                            var options = { "backdrop": "static", keyboard: true };
                            $.ajax({
                                type: "GET",
                                url: '/Pop/View_PMR',
                                contentType: "application/json; charset=utf-8",
                                data: { "id": ens },
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
                ]
            });
        $("#closbtn").click(function () {
            $('#myModal').modal('hide');
        });
    });
});