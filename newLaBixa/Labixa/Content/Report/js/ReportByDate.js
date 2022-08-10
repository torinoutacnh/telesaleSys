
var listAmountDate, listAmountLevel, listAmountAgent;
$('#DatePick_DayStaff').datepicker({ format: 'dd/mm/yyyy' }).on("change", function () {
    GetReportAmountByDate();
});
$('#DatePick_DayStaff1').datepicker({ format: 'dd/mm/yyyy', useCurrent: false }).on("change", function () {
    GetReportAmountByDate();
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
var firebaseConfig = {
    apiKey: "AIzaSyCN38aGkE--JeaZ6b0MDWx6x9PLX27MEeg",
    authDomain: "crm-test-acc75.firebaseapp.com",
    databaseURL: "https://crm-test-acc75.firebaseio.com",
    projectId: "crm-test-acc75",
    storageBucket: "",
    messagingSenderId: "575192447322",
    appId: "1:575192447322:web:5709e9d52e4c3f1c371b63",
    measurementId: "G-1PCZTHJCNS"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
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
function ToAmount(money) {
    var result = "";
    var countUp = 0;
    for (var i = money.length - 1; i >= 0; i--)
        {
        result = money[i] + result;
            countUp++;
            if (countUp == 3 && i != 0) {
                result = "," + result;
                countUp = 0;
            }
        }
    return result;
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
    //$.ajax({
    //    type: "POST",
    //    url: "/Report/Dashboard/GetCharTotalCall",
    //    success: function (data) {
    //        var json = jQuery.parseJSON(data);
    //        var listOfObjects = [];
    //        for (x in json.ChartWeeks) {
    //            var obj = [];
    //            console.log(json.ChartWeeks[x].Name);
    //            obj.push(json.ChartWeeks[x].Name);
    //            obj.push(json.ChartWeeks[x].Value);
    //            console.log(obj);
    //            listOfObjects.push(obj);
    //        }
    //        console.log(listOfObjects);
    //        // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
    //        var dataChart = [{
    //            data: listOfObjects,
    //            color: "#734ba9"
    //        }];

    //        //drawMonthTotalCall("#TotalCountChartWeeks", dataChart);
    //        //document.getElementById("totalCallDay").innerHTML = json.TotalCall_Day + " Cuộc gọi";
    //        // document.getElementById("totalCall_M_W").innerHTML = json.TotalCall_Week;
    //        //document.getElementById("total-amount-date").innerHTML = json.TotalCall_Day;
    //        var jsonLevel = json["ListTotalEachLevel"];
    //        console.log(JSON.stringify(jsonLevel));
    //        //list Level
    //        $.each(jsonLevel, function (key, val) {
    //            // This function will called for each key-val pair.
    //            // You can do anything here with them.
    //            var total = parseInt(val);
    //            var html_level =
    //                "<div class=\"col-xl-4\" id=\"level-detail\">" +
    //                "<section class=\"card card-featured-left card-featured-primary mb-3\" >" +
    //                "<div class=\"card-body\">" +
    //                "<div class=\"widget-summary\">" +
    //                "<div class=\"widget-summary-col widget-summary-col-icon\">" +
    //                "<div class=\"summary-icon bg-primary\">" +
    //                "<i class=\"fas fa-life-ring\"></i>" +
    //                "</div>" +
    //                "</div>" +
    //                "<div class=\"widget-summary-col\">" +
    //                "<div class=\"summary\">" +
    //                "<h4 class=\"title\"> Tổng " + key.substr(2,key.length-2) + "</h4>" +
    //                "<div class=\"info\">" +
    //                "<strong class=\"amount\" id=\"total\">" + total + "</strong>" +
    //                "<span class=\"text-primary\" id=\"total percent\">" + ((total / json.TotalData) * 100).toFixed(2) + " %</span>" +
    //                "</div>" +
    //                "</div>" +
    //                "<div class=\"summary-footer\">" +
    //                "<a href=\"/Report/Dashboard/DataUser?Id=" + key.split(" ")[0] + "\" class=\"text-info\"> View data </a>" +
    //                "</div>" +
    //                "</div >" +
    //                "</div >" +
    //                "</div >" +
    //                "</section >" +
    //                "</div >";
    //            document.getElementById("summary-level").innerHTML += html_level;
    //        });

    //    }
    //});
    GetReportAmountByDate();

};

function GetReportAmountByDate() {
    var dateTo = $("#DatePick_DayStaff").val();
    var dateEnd = $("#DatePick_DayStaff1").val();

    $.ajax({
        type: "POST",
        data: { fromDate: dateTo, toDate: dateEnd },
        url: "/Report/Dashboard/GetAmountByDay",
        success: function (data) {
            var json = jQuery.parseJSON(data);
            var listOfObjects = [];
            for (x in json.date_amount) {
                var obj = [];
                console.log(json.date_amount[x].Name);
                obj.push(json.date_amount[x].Name);
                obj.push(json.date_amount[x].Value);
                listOfObjects.push(obj);
            }
            $("#totalCall_All").val("100");
            // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
            var dataChart = [{
                data: listOfObjects,
                color: "#734ba9"
            }];
            drawMonthTotalCall("#TotalCountChartWeeks", dataChart);
            
            drawMonthTotalCall("#TotalCountChartWeeks2", dataChart);

            var listOfObjectsLevel = [];
            for (x in json.level_amount) {
                var obj_level = [];
                console.log(json.level_amount[x].Name);
                obj_level.push(json.level_amount[x].Name);
                obj_level.push(json.level_amount[x].Value);
                listOfObjectsLevel.push(obj_level);
            }
            // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
            var dataChart_Level = [{
                data: listOfObjectsLevel,
                color: "#734ba9"
            }];
            drawMonthTotalCall("#flotDashSales2", dataChart_Level);
            var listOfObjectsAgent = [];
            for (x in json.agent) {
                var obj_agent = [];
                console.log(json.agent[x].Name);
                obj_agent.push(json.agent[x].Name);
                obj_agent.push(json.agent[x].Value);
                listOfObjectsAgent.push(obj_agent);
            }
            // var dataset = [{ data: json.ChartWeeks, color: "#5482FF" }];
            var dataChart_Agent = [{
                data: listOfObjectsAgent,
                color: "#734ba9"
            }];
            drawMonthTotalCall("#flotDashSales3", dataChart_Agent);
            $("#totalCall_M_W").text(ToAmount(json.totalAmount + "") + " Đồng");
            var html_trt =
                "<tr>" +
                "<th>" + "STT" + "</th>" +
                "<th>" + " Ngày " + "</th>" +
                "<th>" + " Doanh thu" + "</th>" +
                "<th>" + "Tiền tệ" + "</th>" +
                "</tr>";
            document.getElementById("amount-head").innerHTML = html_trt;
            // amount doanh thu
            document.getElementById("totalAmount").innerHTML = ToAmount(json.totalAmount + "") + " Đồng";
            document.getElementById("totalAmount2").innerHTML = ToAmount((json.totalAmount/2) + "") + " Đồng";
            document.getElementById("totalAmount1.2").innerHTML = ToAmount((json.totalAmount / 4 * 3) + "") + " Đồng";
            document.getElementById("totalAmount1.3").innerHTML = ToAmount((json.totalAmount /4) + "") + " Đồng";
            document.getElementById("totalAmount2.2").innerHTML = ToAmount((json.totalAmount /2 /2) + "") + " Đồng";
            document.getElementById("totalAmount2.3").innerHTML = ToAmount((json.totalAmount /2/2) + "") + " Đồng";

            var count = 1;
            document.getElementById("tbody-detail_amount").innerHTML = "";
            listAmountDate = json.date_amount;
            listAmountLevel = json.level_amount;
            listAmountAgent = json.agent;
            for (x in listAmountDate) {
                var detail_html = "<tr>" +
                    "<td>" + count + "</td>" +
                    "<td>" + listAmountDate[x].Name + "</td>" +
                    "<td> " + ToAmount(listAmountDate[x].Value+ "") + "</td>" +
                    "<td>  Đồng </td>" +
                    "</tr>";

                document.getElementById("tbody-detail_amount").innerHTML += detail_html;
                count++;
            }
            // by chanel
            count = 1;
            html_trt =
                "<tr>" +
                "<th>" + "STT" + "</th>" +
                "<th>" + " Chanel " + "</th>" +
                "<th>" + " Doanh thu" + "</th>" +
                "<th>" + "Tiền tệ" + "</th>" +
                "</tr>";
            document.getElementById("amount-chanel").innerHTML = html_trt;
            document.getElementById("tbody-detail_chanel").innerHTML = "";

            for (x in listAmountLevel) {
                var detail_html = "<tr>" +
                    "<td>" + count + "</td>" +
                    "<td>" + listAmountLevel[x].Name + "</td>" +
                    "<td> " + ToAmount(listAmountLevel[x].Value + "") + "</td>" +
                    "<td>  Đồng </td>" +
                    "</tr>";

                document.getElementById("tbody-detail_chanel").innerHTML += detail_html;
                count++;
            }
            // by agent
            count = 1;
            html_trt =
                "<tr>" +
                "<th>" + "STT" + "</th>" +
                "<th>" + " Agent " + "</th>" +
                "<th>" + " Doanh thu" + "</th>" +
                "<th>" + "Tiền tệ" + "</th>" +
                "</tr>";
            document.getElementById("amount-agent").innerHTML = html_trt;
            document.getElementById("tbody-agent").innerHTML ="";
            for (x in json.agent) {
                var agent_html;
                agent_html = "<tr>" +
                    "<td>" + count + "</td>" +
                    "<td> <a href=\"/Report/Staff/Detail?username=" + json.agent[x].Name + "\" style=\"color:#212529;\"> " + json.agent[x].Name + " /<a></td>" +
                    "<td> " + ToAmount(json.agent[x].Value + "") + "</td>" +
                    "<td>  Đồng </td>" +
                    "</tr>";
                document.getElementById("tbody-agent").innerHTML += agent_html;
                count++;
            }

        }
    });
}
//$('select#detailSelecter').on('change', function () {

//    var list;
//    if (this.value == "Amount Date") {
//        list = listAmountDate;
//        var html_trt =
//            "<tr>" +
//            "<th>" + "STT" + "</th>" +
//            "<th>" + " Ngày " + "</th>" +
//            "<th>" + " Doanh thu" + "</th>" +
//            "<th>" + "Tiền tệ" + "</th>" +
//            "</tr>";
//        document.getElementById("amount-head").innerHTML = html_trt;
//    } else {
//        if (this.value == "Amount Level") {
//            list = listAmountLevel;
//            var html_trt =
//                "<tr>" +
//                "<th>" + "STT" + "</th>" +
//                "<th>" + " Chanel " + "</th>" +
//                "<th>" + " Doanh thu" + "</th>" +
//                "<th>" + "Tiền tệ" + "</th>" +
//                "</tr>";
//            document.getElementById("amount-head").innerHTML = html_trt;
//        } else {
//            list = listAmountAgent;
//            var html_trt =
//                "<tr>" +
//                "<th>" + "STT" + "</th>" +
//                "<th>" + " Agent " + "</th>" +
//                "<th>" + " Doanh thu" + "</th>" +
//                "<th>" + "Tiền tệ" + "</th>" +
//                "</tr>";
//            document.getElementById("amount-head").innerHTML = html_trt;
//        }
//    }
//    document.getElementById("tbody-detail_amount").innerHTML = "";
//    var count = 1;
//    for (x in list) {
//        var detail_html;
//        if (this.value == "Amount Agent") {
//            detail_html = "<tr>" +
//                "<td>" + count + "</td>" +
//                "<td> <a href=\"/Report/Staff/Detail?username=" + list[x].Name +"\" style=\"color:#212529;\"> " + list[x].Name + " /<a></td>" +
//                "<td> " + ToAmount(list[x].Value + "") + "</td>" +
//                "<td>  Đồng </td>" +
//                "</tr>";
//        } else {
//            detail_html = "<tr>" +
//                "<td>" + count + "</td>" +
//                "<td>" + list[x].Name + "</td>" +
//                "<td> " + ToAmount(list[x].Value + "") + "</td>" +
//                "<td>  Đồng </td>" +
//                "</tr>";
//        }
        
        
//        document.getElementById("tbody-detail_amount").innerHTML += detail_html;
//        count++;
//    }
//});

function drawMonthTotalCall(id, flotDashSales2Data) {
    console.log("vẽ chart");
    var flotDashSales2 = $.plot(id, flotDashSales2Data, {
        series: {
            lines: {
                show: true,
                lineWidth: 4
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
        },
    });
}

/*
Flot: Sales 3
*/
function drawWeekTotalCall() {
    var flotDashSales3 = $.plot('#flotDashSales1', flotDashSales3Data, {
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

