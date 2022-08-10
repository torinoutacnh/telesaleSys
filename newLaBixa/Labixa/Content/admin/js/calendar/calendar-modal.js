// open modal notice
$('#open-modal').click(() => {
    $('#cal-modal-wrapper').css('transform', 'translateY(0rem)');
});
// close modal notice
$('#close-modal').click(() => {
    $('#cal-modal-wrapper').css('transform', 'translateY(25rem)');
});

// automatically set color for priority of notices
const setColorPriority = () => {
    var _priorityArr = $('.cal-priority-des');
    for(var i=0;i<_priorityArr.length;i++){
        console.log($(_priorityArr[i]).text().toLowerCase());
        $(_priorityArr[i]).css('color', () => {
            var _priority = $(_priorityArr[i]).text().toLowerCase();
            if(_priority == 'high')
                return '#ff0000';
            else if(_priority == 'medium') 
                return '#d7db00';
            return '#7c7c7c';
        });
    }
}
setColorPriority();

// open cal detail notice
const openDetailNotice = (el) => {
    var _isCollasible = $(el).text().toLowerCase();
    var _detail = $(el).next('.cal-detail-notice');
    if(_isCollasible === '+') {
        $(el).html('&minus;').text();
        $(_detail).css('max-height', $(_detail)[0].scrollHeight + 'px');
        console.log($(_detail).css('max-height'));
    } else {
        $(el).html('&plus;').text();
        $(_detail).css('max-height', 0);
    }
}