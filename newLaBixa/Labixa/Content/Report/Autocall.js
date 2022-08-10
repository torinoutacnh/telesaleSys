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
//var firebaseConfig = {
//    apiKey: "AIzaSyDspKO7ZYWa-z14md1uVggH8SlRNUc7DM8",
//    authDomain: "kedusale-e8cff.firebaseapp.com",
//    databaseURL: "https://kedusale-e8cff.firebaseio.com",
//    projectId: "kedusale-e8cff",
//    storageBucket: "kedusale-e8cff.appspot.com",
//    messagingSenderId: "164990748141",
//    appId: "1:164990748141:web:9ae5770c9f9cc955"
//};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
// Get a reference to the database service
var database = firebase.database();
var starCountRef = database.ref('Kedu/'); //truy cập máy nhánh ext
starCountRef.on('value', function (snapshot) {
    //any change will come here
    CheckStaffController(snapshot.val());
});//lấy data, notification, starCountRef.on /starCountRef.once
function CheckStaffController(data) {
    countAvailable = 0;
    totalCallOnline = 0;
    for (x in data) {
        //console.log(data[x]);
        var IdStaff = "Status_" + data[x].Ext;
        var IdPhone = "Phone_" + data[x].Ext;
        if (document.getElementById(IdStaff) != null) {
            if (data[x].Status === "Up") {
                totalCallOnline += 1;
                document.getElementById(IdStaff).innerHTML = "<span class=\"btn btn-warning\">Đang trong cuộc gọi</span>";
            } else if (data[x].Status === "Down") {
                document.getElementById(IdStaff).innerHTML = "<span class=\"btn btn-info\">Đang rãnh</span>";
                countAvailable += 1;
            } else {
                document.getElementById(IdStaff).innerHTML = "<span class=\"btn btn-success\">Đang bận</span>";
            }
        }
        if (document.getElementById(IdPhone) != null) {
            if (data[x].Status === "Up") {
                document.getElementById(IdPhone).innerHTML = "<span>" + data[x].Phone.split('@')[1] + "</span>";
            } else {
                document.getElementById(IdPhone).innerHTML = "<span>Đang cập nhật</span>";

            }
        }
    }
    CalcNumber();
    //if (Flag) {
    //    ExecutiveCall(countExecuteCall, listPhone);
    //console.log("so cuoc goi " + countExecuteCall);
    //}
}

function CalcNumber() {
    countExecuteCall = Math.ceil((countAvailable));
    console.log("CalcNumber() " + countExecuteCall);
}
function ExecutiveCall(num, PhoneList) {
    if (num != 0) {
        var listPhoneTemp = PhoneList;
        for (var i = 0; i <= countExecuteCall; i++) {
           // console.log("so " + i + " " + PhoneList[i]);
            AutoCallV2(PhoneList[i]);
           // AutoCallV2("0969394936");
        }
        for (var i = countExecuteCall; i >= 0; i--) {
            //console.log("so " + i + " " + PhoneList[i]);
            PhoneList.splice(i, 1);;
        }
    }
}
var timers = moment().toDate(); //khai bao timer
var myVar; //tao interval 
var i = 0;//bien dem second
function StartAutocall() {
    document.getElementById("startAutocall").disabled = true;
    $("#startAutocall").addClass("btn-primary").removeClass("btn-success");
    $("#stopAutocall").addClass("btn-info").removeClass("btn-primary");
    document.getElementById("stopAutocall").disabled = false;
    console.log("countNumberCall " + countNumberCall);
    //set gio thanh 00:00:00
    timers.setHours(0);
    timers.setMinutes(0);
    timers.setSeconds(0);
    //goi ham dem thoi gian
    callTimerCountUp();
    Flag = true;
    CalcNumber();
    console.log("countExecuteCall start " + countExecuteCall);
    Interval_Autocall = setInterval(function () { ExecutiveCall(countExecuteCall, listPhone); }, countNumberCall * 1000);
}
function StopAutocall() {
    document.getElementById("startAutocall").disabled = false;
    $("#startAutocall").addClass("btn-success").removeClass("btn-primary");
    $("#stopAutocall").addClass("btn-primary").removeClass("btn-info");
    document.getElementById("stopAutocall").disabled = true;
    //xoa interval 
    clearInterval(myVar);
    clearInterval(Interval_Autocall);
    Flag = false;
}
//Tao interval, 1 giay sẽ chạy hàm mytimer
function callTimerCountUp() {
    _myVar = setInterval(myTimer, 1000);
    // console.log("timer " + timers);

}

//tăng i + 1 để set giây cho timer
function myTimer() {
    i += 1;
    timers.setSeconds(i % 60);
    timers.setMinutes(i / 60);
    timers.setHours(i / 3600);
    //console.log("a " + i);
    //console.log("timers.toLocaleTimeString() " + timers.toLocaleTimeString());
    //console.log("timers " + timers);
    //GetTimeNowToString(myVar);
    document.getElementById("timer_counter").innerHTML = timers.toLocaleTimeString();
}

function AutoCallV2(phone) {
    console.log("alo "+phone);
    //test so ca nhan
    phone = "0969394936";
    var customer = { ServiceName: "CF-B0000501", AuthUser: "CF000596", AuthKey: "6ff57c5f-e6db-49e0-9172-f65b9e3d9308", Ext: 100, PhoneNumber: telePhone + "/" + phone };
    console.log(customer);
    $.ajax({
        type: "POST",
        data: JSON.stringify(customer),
        url: "https://api.cloudfone.vn/api/CloudFone/AutoCallV2",
        contentType: "application/json"
    });//jquery
}

