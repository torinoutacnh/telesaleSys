var _globalTime = new Date();
var monthMockup = {
    0: "Tháng Một",
    1: "Tháng Hai",
    2: "Tháng Ba",
    3: "Tháng Tư",
    4: "Tháng Năm",
    5: "Tháng Sáu",
    6: "Tháng Bảy",
    7: "Tháng Tám",
    8: "Tháng Chín",
    9: "Tháng Mười",
    10: "Tháng Mười Một",
    11: "Tháng Mười Hai"
}

var weekMockUp = {
    'Sun': 6,
    'Sat': 5,
    'Fri': 4,
    'Thu': 3,
    'Wed': 2,
    'Tue': 1,
    'Mon': 0,
}

const setupCalendar = (time) => {
    var _index = getDateIndex(time);
    var _row = 0;
    var _date = 1;
    var MAX_COL = 7;
    var MAX_DAY = getMaxDate(time);
    $('#cal-schedule').append('<tr class="cal-row"></tr>');  
    for(var i=0;i<_index;i++)
        $('.cal-row').eq(_row).append('<td></td>');  
    while(_date <= MAX_DAY){
        if(_index == 7) {
            _index = 0;
            _row++;
            $('#cal-schedule').append('<tr class="cal-row"></tr>');  
        } else {
            _index++;
            $('.cal-row').eq(_row).append('<td class="cal-col" onclick="showModal(this)">' 
            + '<span class="cal-date-number">' 
            + _date 
            + '</span>'    
            + '</td>');

        }
        _date++;
    }
    $('#currentTime').append(() => {
        var _nameOfMonth = '';
        for (const key in monthMockup) 
            if (monthMockup.hasOwnProperty(key) && key == _globalTime.getMonth()) 
                _nameOfMonth = monthMockup[key];

        return _nameOfMonth + ' - ' + _globalTime.getFullYear();
    });
}
const getDateIndex = (time) => {
    var _firstDate = new Date(time.getFullYear(), time.getMonth(), 1).toString().substr(0,3);
    var _index = '';
    for (const key in weekMockUp) 
        if (weekMockUp.hasOwnProperty(key) && key == _firstDate) 
            _index =  weekMockUp[key];
    return _index;
}
const onChangeNext = (time) => {
        var m = time.getMonth(), y = time.getFullYear();
        if(m == 12) {
            m = 1;
            y++;
        } else m++;
        reset();
        _globalTime = new Date(y, m, 1);
        setupCalendar(_globalTime);
    }
const onChangePrev = (time) => {
        var m = time.getMonth(), y = time.getFullYear();
        if(m == 1) {
            m = 12;
            y--;
        } else  m--;
        reset();
        _globalTime = new Date(y, m, 1);
        setupCalendar(_globalTime);
    }
const getMaxDate = (time) => {
        var m = time.getMonth() + 1;
        var y = time.getFullYear();
        if(m == 4 || m == 6 || m == 9 || m == 11)
            return 30;
        if(m == 2){
            if((y % 4 == 0 && y % 100 != 0) || y % 400 == 0)
                return 29;
            else return 28;
        }

        return 31;
    }
const reset = () => {
    $('.cal-row').remove();
    $('#currentTime').empty();
}

const showModal = (el) => {
    $('#cal-modal').css('display', 'block');
    $('#cal-modal-body').css('display', 'block');
    
    var _wModal = $('#cal-modal-body').css('width').substr(0,3);
    var _hModal = $('#cal-modal-body').css('height').substr(0,3);
    var _wWindow = $(window).width();
    var _hWindow = $(window).height();
    var _posTop = _hWindow/2 - _hModal/2;
    var _posLeft = _wWindow/2 - _wModal/2;
    console.log(_wModal + ' - ' + _hModal);
    $('#cal-modal-body').css('top',_posTop+'px');
    $('#cal-modal-body').css('left',_posLeft+'px');
}

const closeModal = () => {
    $('#cal-modal').css('display', 'none');
    $('#cal-modal-body').css('display', 'none');
}

setupCalendar(_globalTime);


$('#btn-next').click(() => onChangeNext(_globalTime));
$('#btn-prv').click(() => onChangePrev(_globalTime));