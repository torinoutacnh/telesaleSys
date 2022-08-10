////var firebaseConfig = {
////    apiKey: "AIzaSyDspKO7ZYWa-z14md1uVggH8SlRNUc7DM8",
////    authDomain: "kedusale-e8cff.firebaseapp.com",
////    databaseURL: "https://kedusale-e8cff.firebaseio.com",
////    projectId: "kedusale-e8cff",
////    storageBucket: "kedusale-e8cff.appspot.com",
////    messagingSenderId: "164990748141",
////    appId: "1:164990748141:web:9ae5770c9f9cc955"
////};
//////var firebaseConfig = {
//////    apiKey: "AIzaSyCN38aGkE--JeaZ6b0MDWx6x9PLX27MEeg",
//////    authDomain: "crm-test-acc75.firebaseapp.com",
//////    databaseURL: "https://crm-test-acc75.firebaseio.com",
//////    projectId: "crm-test-acc75",
//////    storageBucket: "",
//////    messagingSenderId: "575192447322",
//////    appId: "1:575192447322:web:5709e9d52e4c3f1c371b63",
//////    measurementId: "G-1PCZTHJCNS"
//////};
////// Initialize Firebase
////firebase.initializeApp(firebaseConfig);
////// Get a reference to the database service
////var database = firebase.database();
////var starCountRef = database.ref('Working/'); //truy cập máy nhánh ext
////starCountRef.on('value', function (snapshot) {
////    //any change will come here
////    CheckStaffController(snapshot.val());
////});//lấy data, notification, starCountRef.on /starCountRef.once
////function CheckStaffController(data) {
////    console.log("aaaaa");
////  //  console.log(data);
////    for (x in data) {
////        //console.log(data[x]);
////        var IdStaff = "Status_" + data[x].Id;
////        var work = data[x].Working;
////            console.log(IdStaff);
////        console.log(work);
////        if (document.getElementById(IdStaff) != null) {
////            if (data[x].Working == "ON") {
////                document.getElementById(IdStaff).innerHTML = "<span class=\"btn btn-warning\">Online</span>";
////            } else {
////                document.getElementById(IdStaff).innerHTML = "<span class=\"btn btn-info\">Offline</span>";
////            }
////        }
////    }
////}

function ParsingProgress(percent) {
    if (percent < 30) {

        return "<div class=\"progress progress-sm progress-half-rounded m-2\">" +
            "<div class=\"progress-bar progress-bar-danger\" role=\"progressbar\" aria-valuenow=\"" + percent + "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + percent + "%;\">"
            + percent + "%</div></div>"
    } else if (percent >= 30 && percent < 50) {
        return "<div class=\"progress progress-sm progress-half-rounded m-2\">" +
            "<div class=\"progress-bar progress-bar-warning\" role=\"progressbar\" aria-valuenow=\"" + percent + "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + percent + "%;\">"
            + percent + "%</div></div>"
    } else if (percent >= 50 && percent < 80) {
        return "<div class=\"progress progress-sm progress-half-rounded m-2\">" +
            "<div class=\"progress-bar progress-bar-info\" role=\"progressbar\" aria-valuenow=\"" + percent + "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + percent + "%;\">"
            + percent + "%</div></div>"
    } else if (percent >= 80) {
        return "<div class=\"progress progress-sm progress-half-rounded m-2\">" +
            "<div class=\"progress-bar progress-bar-success\" role=\"progressbar\" aria-valuenow=\"" + percent + "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + percent + "%;\">"
            + percent + "%</div></div>"
    }
}
function IntToTime(timeNum) {
    console.log("timeNum");
    console.log(timeNum);
    var timers = moment().toDate();
    timers.setHours(0);
    timers.setMinutes(0);
    timers.setSeconds(timeNum);
    console.log(timers.toLocaleTimeString('en-GB'));
    return timers.toLocaleTimeString('en-GB');
}
