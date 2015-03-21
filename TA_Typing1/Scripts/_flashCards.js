// Flash Card
$(function () {
    $(".card-grid").flip({
        trigger: 'click'
    });
});

$(document).ready(function () {
    $('.color-opt').hide();
})

function showColorOption() {
    $('.color-opt').fadeIn('fast');
    $('#submit-color-option').fadeIn('fast');
    $('#paint-card-btm').removeClass('refresh-btn');
    $('#paint-card-btm').parent().removeClass('col-md-2 col-md-offset-3 margin-top-bottom');

    $('#hide-search-card-btn').parent().removeClass('margin-top-bottom');
    $('#rand-position-btn').parent().removeClass('margin-top-bottom');
    
    $('#hide-search-card-btn').parent().html("");
    $('#rand-position-btn').parent().html("");

    $('#header_layout').hide();
    $('#header2-flash-cards').addClass('navbar navbar-default navbar-fixed-top header-fixed');
}

// submit forms flash cards
function submitColorOptions() {
    $('.form-color-option').submit();
}

// show search tab
function showSearchCardTab() {
    $('#search-card-tab').fadeIn('slow');
    $('#show-search-card-btn').hide();
    $('#hide-search-card-btn').fadeIn('fast');
}

function hideSearchCard() {
    $('#search-card-tab').fadeOut(300);
    $('#hide-search-card-btn').hide();
    $('#show-search-card-btn').fadeIn('fast');
}
// function random position
function randomPosition() {
    $('#grid-containter').fadeOut(0);

    var totalGrid = $('#total_grids').val();
    var gridData;
    var pivotPoint;
    var mixTime = 5;

    for (var j = mixTime; j >= 0 ; --j) {
        for (var i = 0; i <= totalGrid; ++i) {
            pivotPoint = Math.floor((Math.random() * 1000)) % (totalGrid + 1);
            gridData = $('#grid-' + pivotPoint).html();

            $('#grid-' + pivotPoint).html($('#grid-' + i).html());
            $('#grid-' + i).html(gridData);
        }
    }

    $('#grid-containter').delay(100).fadeIn("300");
}

function updateColor(event, id, fColor) {
    event.stopPropagation();
    $('#front-' + id).closest("section").find('.fColor-input').val("flash-" + fColor);

    $('#front-' + id).removeClass();
    $('#back-' + id).removeClass();

    $('#front-' + id).addClass("front flash-" + fColor + "-front");
    $('#back-' + id).addClass("back flash-"+fColor+"-back");
}
