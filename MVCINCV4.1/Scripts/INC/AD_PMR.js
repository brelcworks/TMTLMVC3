$(function () {
    $(document).ready(function () {
        $('INPUT[type="text"]').focus(function () {
            $(this).addClass("focus");
        });

        $('INPUT[type="text"]').blur(function () {
            $(this).removeClass("focus");
        });


        $.ajax({
            type: "GET",
            url: "/Pop/gdata2",
            data: { aData: $("#txtPOPENS").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
            $.each(data, function (i, val) {
                $('#txtSNAME').val(val.SNAME)
                $('#txtSID').val(val.SID)
                $('#txtENS').val(val.ENS)
                $('#txtRID').val(new Date($.now()))
                $('#txtDGNO').val(val.DGNO)
                $('#txtKVA').val(val.RAT_PH)
                $('#txtMODEL').val(val.MODEL)
                $('#txtAMAKE').val(val.AMAKE)
                $('#txtALSN').val(val.ALSN)
                $('#txtBSN').val(val.BSN)
                $('#txtDIST').val(val.DIST)
                $('#txtSTATE').val(val.STATE)
                $('#txtCUST').val(val.CNAME)
                $('#txtDOI').val($('#txtPOPDOC').val())
                $('#txtWARR').val(val.WARR)
                $('#txtOEA').val(val.OEA)
                $('#txtAMC').val(val.AMC)
                $('#txtAEDT').val("NEW")
                $('#txtSSTA').val("NEW")
                $('#txtLMODI').val(new Date($.now()))
                $('#txtPOPAEDT').val("NEW")
                $('#txtPOPSSTA').val("MOD")
                $('#txtPOPLMODI').val(new Date($.now()))
                $('#txtDOS').focus()
            });
        },
        error: function OnErrorCall(response) {
            alert(response.status + " " + response.statusText);
        }
    });
});

$("#txtSTYPE").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/Pop/stype',
            type: "POST",
            dataType: "json",
            data: { term: request.term },
            success: function (data) {
                response(data);
            }
        });
    }
});

$("#txtTECHNICIAN").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/Pop/tech',
            type: "POST",
            dataType: "json",
            data: { term: request.term },
            success: function (data) {
                response(data);
            }
        });
    }
});

$('#txtDOS').datetimepicker({
    format: 'd-M-Y H:i',
    step: 1,
    theme: 'dark'
});

$('#txtCDATI').datetimepicker({
    format: 'd-M-Y H:i',
    step: 1,
    theme: 'dark'
});


$('#txtHMR').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtSTYPE').focus();
        e.preventDefault();
        var newhmr = $('#txtHMR').val();
        var oldhmr = $('#txtPOPCHMR').val();
        var ldt = new Date($('#txtPOPCHMD').val());
        var ndt = new Date($('#txtCDATI').val());
        if (parseFloat(newhmr) <= parseFloat(oldhmr)) {
            alert("LAST PM / CM HMR IS GREATER THEN THE CURRENT HMR! PLEASE REVIEW");
            $('#txtHMR').focus();
        }
        if (ndt <= ldt) {
            alert("LAST PM / CM DATE IS GREATER THEN THE CURRENT DATE! PLEASE REVIEW");
            $('#txtCDATI').focus();
        }
    }
});

$('#txtSTYPE').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtTTR').focus();
        e.preventDefault();
        var date1 = new Date($('#txtDOS').val())
        var date2 = new Date($('#txtCDATI').val())
        var difference_ms = date2 - date1;
        difference_ms = difference_ms / 1000;
        var seconds = Math.floor(difference_ms % 60);
        difference_ms = difference_ms / 60;
        var minutes = Math.floor(difference_ms % 60);
        difference_ms = difference_ms / 60;
        var hours = Math.floor(difference_ms % 24);
        var days = Math.floor(difference_ms / 24);
        var one_day = 1000 * 60 * 60 * 24;
        var difference_ms1 = date2 - date1;
        var day1 = difference_ms1 / one_day;
        var tval = day1 * 24;
        $('#txtTTR').val(tval.toFixed(0));
        switch (true) {
            case (tval >= 72):
                $('#txtTSLOT').val(">72");
                break;
            case (tval >= 48):
                $('#txtTSLOT').val("48-72");
                break;
            case (tval >= 24):
                $('#txtTSLOT').val("24-48");
                break;
            case (tval >= 10):
                $('#txtTSLOT').val("10-24");
                break;
            default:
                $('#txtTSLOT').val("0-10");
        }

        var hm = $('#txtHMR').val();
        switch (true) {
            case (hm >= 15000):
                $('#txtHMAGE').val(">15000");
                break;
            case (hm >= 12500):
                $('#txtHMAGE').val("12500-15000");
                break;
            case (hm >= 10000):
                $('#txtHMAGE').val("10000-12500");
                break;
            case (hm >= 7500):
                $('#txtHMAGE').val("7500-10000");
                break;
            case (hm >= 5000):
                $('#txtHMAGE').val("5000-7500");
                break;
            case (hm >= 2500):
                $('#txtHMAGE').val("2500-5000");
                break;
            default:
                $('#txtHMAGE').val("0-2500");
        }
    }
});

$('#txtTTR').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtTSLOT').focus();
        e.preventDefault();
        var stp = $('#txtSTYPE').val();
        if (stp == "OIL SERVICE") {
            var rat = $('#txtKVA').val();
            switch (rat) {
                case '7.5':
                    $('#txtMETERIAL').val("1)LUBE OIL 7.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 1 NOS");
                    break;
                case '10':
                    $('#txtMETERIAL').val("1)LUBE OIL 7.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 1 NOS");
                    break;
                case '15':
                    $('#txtMETERIAL').val("1)LUBE OIL 8 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '20':
                    $('#txtMETERIAL').val("1)LUBE OIL 9.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '25':
                    $('#txtMETERIAL').val("1)LUBE OIL 9.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '30':
                    $('#txtMETERIAL').val("1)LUBE OIL 9.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '35':
                    $('#txtMETERIAL').val("1)LUBE OIL 9.5 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '40':
                    $('#txtMETERIAL').val("1)LUBE OIL 8 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '50':
                    $('#txtMETERIAL').val("1)LUBE OIL 8 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                case '62.5':
                    $('#txtMETERIAL').val("1)LUBE OIL 8 LTR , 2)LUBE OIL FILTER 1 NOS , 3)FUEL FILTER 2 NOS");
                    break;
                default:
                    $('#txtMETERIAL').val("FILL THE DATA MANUALLY");
                    break;
            }
            $('#txtCCATE').val("ENGINE");
            $('#txtCNAT').val("NA");
            $('#txtSERV').val("NA");
            $('#txtSTA').val("CLOSED");
            $('#txtACTION').val("SERVICING DONE");
            $('#txtRFAIL').val("NA");
        }
    }
});

$('#txtTSLOT').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtMETERIAL').focus();
        e.preventDefault();
    }
});

$('#txtMETERIAL').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtHMAGE').focus();
        e.preventDefault();
    }
});

$('#txtHMAGE').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtCCATE').focus();
        e.preventDefault();
    }
});

$('#txtCCATE').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtCNAT').focus();
        e.preventDefault();
    }
});

$('#txtCNAT').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtSERV').focus();
        e.preventDefault();
    }
});

$('#txtSERV').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtSTA').focus();
        e.preventDefault();
    }
});

$('#txtSTA').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtRFAIL').focus();
        e.preventDefault();
        var ser = $('#txtSERV').val();
        var tt = $('#txtTTR').val();
        if (ser == "MAJOR") {
            switch (true) {
                case (tt > 12):
                    $('#txtSLA').val("OUT OF SLA");
                    break;
                default:
                    $('#txtSLA').val("WITHIN SLA");
                    $('#txtRESLA').val("NA");
                    break;
            }
        }
        else {
            switch (true) {
                case (tt > 6):
                    $('#txtSLA').val("OUT OF SLA");
                    break;
                default:
                    $('#txtSLA').val("WITHIN SLA");
                    $('#txtRESLA').val("NA");
                    break;
            }
        }
    }
});

$('#txtRFAIL').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtACTION').focus();
        $('#txtDPCODE').val("A1587");
        e.preventDefault();
    }
});

$('#txtACTION').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtPOPACTION').val($('#txtACTION').val());
        $('#txtREPORT').focus();
        e.preventDefault();
    }
});

$('#txtREPORT').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtTECHNICIAN').focus();
        e.preventDefault();
    }
});

$('#txtTECHNICIAN').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtSLA').focus();
        e.preventDefault();
    }
});

$('#txtREM').focus(function (e) {
    var chmd = new Date($('#txtPOPCHMD1').val());
    var chmr = $('#txtPOPCHMR1').val();
    if (chmd == "") {
        alert("BLANK");
    }
    else {
        var start = new Date($('#txtCDATI').val());
        var diff = new Date(start - chmd);
        var days = diff / 1000 / 60 / 60 / 24;
        var difhm = $('#txtHMR').val() - chmr;
        var ahm = difhm / days;
        var xy = 500 / ahm;
        var styp = $('#txtSTYPE').val();
        if (xy >= 180) {
            $('#txtAHM').val(ahm);
            $('#txtPOPCHMR').val($('#txtHMR').val());
            $('#txtPOPCHMD').val($('#txtCDATI').val());
            start.setDate(start.getDate() + 180);
            if (styp == 'OIL SERVICE') {
                $('#txtPOPLHMR').val($('#txtHMR').val());
                $('#txtPOPLSD').val($('#txtCDATI').val());
                var month = new Array();
                month[0] = "Jan";
                month[1] = "Feb";
                month[2] = "Mar";
                month[3] = "Apr";
                month[4] = "May";
                month[5] = "Jun";
                month[6] = "Jul";
                month[7] = "Aug";
                month[8] = "Sep";
                month[9] = "Oct";
                month[10] = "Nov";
                month[11] = "Dec";
                var mon1 = month[start.getMonth()];
                $('#txtNSD').val(start.getDate() + "-" + mon1 + "-" + start.getFullYear() + " " + start.getHours() + ":" + start.getMinutes());
            }
        }
        else {
            $('#txtAHM').val(ahm);
            $('#txtPOPCHMR').val($('#txtHMR').val());
            $('#txtPOPCHMD').val($('#txtCDATI').val());
            start.setDate(start.getDate() + xy);
            if (styp == 'OIL SERVICE') {
                $('#txtPOPLHMR').val($('#txtHMR').val());
                $('#txtPOPLSD').val($('#txtCDATI').val());
                var month = new Array();
                month[0] = "Jan";
                month[1] = "Feb";
                month[2] = "Mar";
                month[3] = "Apr";
                month[4] = "May";
                month[5] = "Jun";
                month[6] = "Jul";
                month[7] = "Aug";
                month[8] = "Sep";
                month[9] = "Oct";
                month[10] = "Nov";
                month[11] = "Dec";
                var mon1 = month[start.getMonth()];
                $('#txtNSD').val(start.getDate() + "-" + mon1 + "-" + start.getFullYear() + " " + start.getHours() + ":" + start.getMinutes());
            }
        }
    }
});

$('#txtSLA').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtRESLA').focus();
        e.preventDefault();
    }
});

$('#txtRESLA').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtREM').focus();
        e.preventDefault();
    }
});

$('#txtREM').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtNSD').focus();
        e.preventDefault();
    }
});

$('#txtNSD').keypress(function (e) {
    if (e.keyCode == 13) {
        $('#txtDPCODE').focus();
        e.preventDefault();
    }
});

$('input:text:first').focus();
var $inp = $('input:text');
$inp.bind('keydown', function (e) {
    //var key = (e.keyCode ? e.keyCode : e.charCode);
    var key = e.which;
    if (key == 27) {
        e.preventDefault();
        var nxtIdx = $inp.index(this) + 1;
        $(":input:text:eq(" + nxtIdx + ")").focus();
    }
});
});