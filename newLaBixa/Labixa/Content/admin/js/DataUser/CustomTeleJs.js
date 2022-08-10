
var _dateTo = _dateEnd = moment().format('DD/MM/YYYY');
$('#DatePick_DayStaff').datepicker({ format: 'dd/mm/yyyy' }).on("change", function () {
    _dateTo = $("#DatePick_DayStaff").val();
    var IdTele = $("#IdTeleSale").val();
    var IdTeleMonth = $("#IdTeleMonth").val();
    $("#dateEdit").attr("href", "/Admin/DataUser/OrderTeleDay?teleId=" + IdTele + "&lastDate=" + _dateTo);
    $("#IdTeleSaleMonth").attr("href", "/Admin/DataUser/OrderTeleMonth?teleId=" + IdTeleMonth + "&lastDate=" + _dateTo);

    GetDataDashboard();
});
//var firebaseConfig = {
//    apiKey: "AIzaSyDspKO7ZYWa-z14md1uVggH8SlRNUc7DM8",
//    authDomain: "kedusale-e8cff.firebaseapp.com",
//    databaseURL: "https://kedusale-e8cff.firebaseio.com",
//    projectId: "kedusale-e8cff",
//    storageBucket: "kedusale-e8cff.appspot.com",
//    messagingSenderId: "164990748141",
//    appId: "1:164990748141:web:9ae5770c9f9cc955"
//};
//var firebaseConfig = {
//    apiKey: "AIzaSyCN38aGkE--JeaZ6b0MDWx6x9PLX27MEeg",
//    authDomain: "crm-test-acc75.firebaseapp.com",
//    databaseURL: "https://crm-test-acc75.firebaseio.com",
//    projectId: "crm-test-acc75",
//    storageBucket: "",
//    messagingSenderId: "575192447322",
//    appId: "1:575192447322:web:5709e9d52e4c3f1c371b63",
//    measurementId: "G-1PCZTHJCNS"
//};
// Initialize Firebase
firebase.initializeApp(_firebaseConfig);
// Get a reference to the database service
var database = firebase.database();
var starCountRef = database.ref('Working/'); //truy cập máy nhánh ext
starCountRef.on('value', function (snapshot) {
    //any change will come here
    CheckStatusExt(snapshot);
});//lấy data, notification, starCountRef.on /starCountRef.once
function CheckStatusExt(snapshot) {
    //console.log(snapshot.val());
    CountOnOff(snapshot.val());
}
function CountOnOff(listObj) {
    var on = 0;
    var off = 0;
    for (x in listObj) {
        //console.log(x);
        //console.log(listObj[x].Working);
        if (listObj[x].Working == "ON") {
            on += 1;
            addRowOnlineList(listObj[x], x);
        } else {
            off += 1;
            removeOfflineList(x);
        }
    }
    //console.log(on);
    //console.log(off);
    document.getElementById("sumOnline").innerHTML = on;
    document.getElementById("sumOffline").innerHTML = off;
}

var starCountRef2 = database.ref('Kedu/'); //truy cập máy nhánh ext
starCountRef2.on('value', function (snapshot) {
    //any change will come here
    //console.log(snapshot);
    CheckStatusExt2(snapshot.val());
});//lấy data, notification, starCountRef.on /starCountRef.once
function CheckStatusExt2(snapshot) {
    //console.log(snapshot);
    for (x in snapshot) {
        var statusId = "status_" + snapshot[x].Ext;
        //console.log(snapshot[x].Status);
        //console.log(snapshot[x].Ext);
        if (document.getElementById(statusId) != null) {

            if (snapshot[x].Status == "Up") {
                document.getElementById(statusId).innerHTML = "Trong Cuộc Gọi";
                document.getElementById(statusId).className = "badge badge-success";
            } else {
                document.getElementById(statusId).innerHTML = "Kết thúc cuộc gọi";
                document.getElementById(statusId).className = "badge badge - warning";
            }
        }
    }
}
function removeOfflineList(ext) {
    var tr_id = "#tr_" + ext;
    $(tr_id).remove();
}
function addRowOnlineList(obj, ext) {
    //<div class="badge badge-success">Trong cuộc gọi<div > badge badge - warning
    //console.log(obj);
    var tr_id = "#tr_" + ext;
    if ($(tr_id)[0]) {
        //
    } else {
        var html_tr =
            "<tr id=\"tr_" + ext + "\">" +
            "<td>" + obj.DisplayName + "</td>" +
            "<td><span class=\"\">" + ext + "</span></td>" +
            "<td>" +
            "<div id=\"status_" + ext + "\" class=\"\">" +
            "Đang cập nhật" +
            "</div>" +
            "</td>" +
            "</tr>";
        document.getElementById("tbody-online").innerHTML += html_tr;
    }
}

window.onload = function () {
    GetDataDashboard();
};

function GetDataDashboard() {
    showLoading();
    $.ajax({
        type: "POST",
        data: { dateVal: _dateTo },
        url: "/Admin/DataUser/GetCharTotalCall",
        success: function (data) {
            var json = jQuery.parseJSON(data);

            var listOfObjectMonths = [];
            for (x in json.ChartMonths) {
                obj = [];
                //console.log(json.ChartMonths[x].Name);
                obj.push(json.ChartMonths[x].Name);
                obj.push(json.ChartMonths[x].Value);
                //console.log(obj);
                listOfObjectMonths.push(obj);
            }
            //console.log(listOfObjectMonths);
            // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
            var dataChartMonth = [{
                data: listOfObjectMonths,
                color: "#734ba9"
            }];
            drawMonthTotalCall("#TotalCountChartMonth", dataChartMonth);


            var listOfObjects = [];
            for (x in json.ChartOrderMonths) {
                var obj = [];
                // console.log(json.ChartWeeks[x].Name);
                obj.push(json.ChartOrderMonths[x].Name);
                obj.push(json.ChartOrderMonths[x].Value);
                // console.log(obj);
                listOfObjects.push(obj);
            }
            //  console.log(listOfObjects);
            // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
            var dataChart = [{
                data: listOfObjects,
                color: "#734ba9"
            }];
            drawWeekTotalCall("#TotalOrderMonth", dataChart);
            document.getElementById("totalCallDay").innerHTML = json.TotalCall_Day + " Cuộc gọi";
            //document.getElementById("totalCall_M_W").innerHTML = json.TotalCall_Week;
            document.getElementById("totalCall_M_W2").innerHTML = json.TotalCall_Month;
            document.getElementById("totalOrderDay").innerHTML = json.TotalOrder_Day;
            document.getElementById("totalOrder_M_W").innerHTML = json.TotalOrder_Month;
            document.getElementById("TotalAns").innerHTML = json.TotalAnswer;
            document.getElementById("TotalCall").innerHTML = json.TotalCall_Day;
            document.getElementById("AnsPerTotal").innerHTML = ((json.TotalAnswer / json.TotalCall_Day) * 100).toFixed(1) + " %";
            document.getElementById("total-amount-date").innerHTML = addCommas(json.TotalAmount.toFixed(0));
            document.getElementById("TotalAmountMonth").innerHTML = addCommas(json.TotalAmountMonth.toFixed(0));
            document.getElementById("totalCallmonth").innerHTML = json.TotalCall_Month + " Cuộc gọi";
            document.getElementById("totalOrderMonth").innerHTML = json.TotalOrder_Month;
            document.getElementById("TotalAnsMonth").innerHTML = json.TotalAnswerMonth;
            document.getElementById("TotalCallMonth").innerHTML = json.TotalCall_Month;
            document.getElementById("AnsPerTotalMonth").innerHTML = ((json.TotalAnswerMonth / json.TotalCall_Month) * 100).toFixed(1) + " %";
            document.getElementById("AveragePaymentOrderMonth").innerHTML = addCommas(json.AvgPaymentOrder_Month.toFixed(0));
            document.getElementById("AveragePaymentOrderDay").innerHTML = addCommas(json.AvgPaymentOrder_Day.toFixed(0));

            console.log(json.DateChoose + " ngay chosing");
            var date = new Date(json.DateChoose);
            console.log(date);
            var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();

            document.getElementById("Id_ChoosingDate").innerHTML = formatted;
            document.getElementById("Id_ChoosingMonth").innerHTML = ("0" + (date.getMonth() + 1)).slice(-2);


            var jsonLevel = json["ListTotalEachLevel"];
            console.log(JSON.stringify(jsonLevel));
            //list Level
            var html_level = "";
            $.each(jsonLevel, function (key, val) {
                // This function will called for each key-val pair.
                // You can do anything here with them.
                var total = parseInt(val);
                html_level +=
                    "<div class=\"col-xl-4\" id=\"level-detail\">" +
                    "<section class=\"card card-featured-left card-featured-primary mb-3\" >" +
                    "<div class=\"card-body\">" +
                    "<div class=\"widget-summary\">" +
                    "<div class=\"widget-summary-col widget-summary-col-icon\">" +
                    "<div class=\"summary-icon bg-primary\">" +
                    "<i class=\"fas fa-life-ring\"></i>" +
                    "</div>" +
                    "</div>" +
                    "<div class=\"widget-summary-col\">" +
                    "<div class=\"summary\">" +
                    "<h4 class=\"title\"> Tổng " + key.substr(2, key.length - 2) + "</h4>" +
                    "<div class=\"info\">" +
                    "<strong class=\"amount\" id=\"total\">" + total + "</strong>" +
                    "<span class=\"text-primary\" id=\"total percent\">" + ((total / json.TotalData) * 100).toFixed(2) + " %</span>" +
                    "</div>" +
                    "</div>" +
                    "<div class=\"summary-footer\">" +
                    //"<p class=\"text-info\" style=\"align-items: left;float: left;\">"+L2/L1 +": <span style=\"color: red;\">"+10+"%</span></p>"+
                    "<a href=\"/Admin/DataUser/DataUserTele?Id=" + key.split(" ")[0] + "\" class=\"text-info\"> View data </a>" +
                    "</div>" +
                    "</div >" +
                    "</div >" +
                    "</div >" +
                    "</section >" +
                    "</div >";
            });
            document.getElementById("summary-level").innerHTML = html_level;
            /* 
             * old
            document.getElementById("total_L0").innerHTML = jsonLevel["L1"];
            document.getElementById("total_L1").innerHTML = json.TotalDataL1;
            document.getElementById("total_L2").innerHTML = json.TotalDataL2;
            document.getElementById("total_L3").innerHTML = json.TotalDataL3;
            document.getElementById("total_L4").innerHTML = json.TotalDataL4;
            document.getElementById("total_L5").innerHTML = json.TotalDataL5;
            document.getElementById("total_L6").innerHTML = json.TotalDataL6;
            document.getElementById("total_L7").innerHTML = json.TotalDataL7;
            document.getElementById("total_L8").innerHTML = json.TotalDataL8;
            document.getElementById("total_L0_percent").innerHTML = ((json.TotalDataL0 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L1_percent").innerHTML = ((json.TotalDataL1 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L2_percent").innerHTML = ((json.TotalDataL2 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L3_percent").innerHTML = ((json.TotalDataL3 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L4_percent").innerHTML = ((json.TotalDataL4 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L5_percent").innerHTML = ((json.TotalDataL5 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L6_percent").innerHTML = ((json.TotalDataL6 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L7_percent").innerHTML = ((json.TotalDataL7 / json.TotalData) * 100).toFixed(2) + " %";
            document.getElementById("total_L8_percent").innerHTML = ((json.TotalDataL8 / json.TotalData) * 100).toFixed(2) + " %";
            */
            hideLoading();
        }
    });
}

function drawMonthTotalCall(id, flotDashSales2Data) {
    console.log("vẽ chart");
    var month = $.plot(id, flotDashSales2Data, {
        series: {
            lines: {
                show: true,
                lineWidth: 2
            },
            points: {
                show: true
            },
            shadowSize: 0
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderColor: 'rgba(0,0,0,0.1)',
            borderWidth: 1,
            labelMargin: 15,
            backgroundColor: 'transparent'
        },
        yaxis: {
            min: 0,
            color: 'rgba(0,0,0,0.1)'
        },
        xaxis: {
            mode: 'categories',
            color: 'rgba(0,0,0,0)'
        },
        legend: {
            show: false
        },
        tooltip: true,
        tooltipOpts: {
            content: '%y',
            shifts: {
                x: -30,
                y: 25
            },
            defaultTheme: false
        }
    });
}

/*
Flot: Sales 3
*/
function drawWeekTotalCall(id2, flotDashSales3Data) {
    var week = $.plot(id2, flotDashSales3Data, {
        series: {
            lines: {
                show: true,
                lineWidth: 2
            },
            points: {
                show: true
            },
            shadowSize: 0
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderColor: 'rgba(0,0,0,0.1)',
            borderWidth: 1,
            labelMargin: 15,
            backgroundColor: 'transparent'
        },
        yaxis: {
            min: 0,
            color: 'rgba(0,0,0,0.1)'
        },
        xaxis: {
            mode: 'categories',
            color: 'rgba(0,0,0,0)'
        },
        legend: {
            show: false
        },
        tooltip: true,
        tooltipOpts: {
            content: '%x: %y',
            shifts: {
                x: -30,
                y: 25
            },
            defaultTheme: false
        }
    });
}///sadadasdsadasdasds
// why dont changes

// Add comma to output
//-------------------------------------------------------------------//
function addCommas(num) {
    var str = num.toString().split('.');
    if (str[0].length >= 4) {
        //add comma every 3 digits befor decimal
        str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
    }
    /* Optional formating for decimal places
    if (str[1] && str[1].length >= 4) {
        //add space every 3 digits after decimal
        str[1] = str[1].replace(/(\d{3})/g, '$1 ');
    }*/
    return str.join('.');
}

