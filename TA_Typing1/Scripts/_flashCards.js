// Flash Card
$(function () {
    $(".card-grid").flip({
        trigger: 'click'
    });
});

$(document).ready(function () {
    $('.color-opt').hide();
    $('.card-grid').fadeIn('fast');
})

function showColorOption() {
    $('.favourite-opt').hide();
    $('.color-opt').fadeIn('fast');

    $('#header_layout').hide();
    $('#header2-flash-cards').hide();
    $('#header1-flash-cards').show();
}

// submit forms flash cards
function submitColorOptions() {
    $('.form-color-option').submit();
}

// show favourite button
function showFavouriteOption() {
    $('.color-opt').hide();
    $('.favourite-opt').fadeIn('fast');
    
    $('#header_layout').hide();
    $('#header1-flash-cards').hide();
    $('#header2-flash-cards').show();
}

// add, remomve favourite
function addFavourite(event, id) {
    event.stopPropagation();
   
    $('#front-' + id).find(".false_favourite").hide();
    $('#front-' + id).find(".true_favourite").fadeIn();
    $('#front-' + id).find(".fFavourite-input").val(true);
    
}

function removeFavourite(event, id) {
    event.stopPropagation();

    $('#front-' + id).find(".true_favourite").hide();
    $('#front-' + id).find(".false_favourite").fadeIn();
    $('#front-' + id).find(".fFavourite-input").val(false);

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

// show option
function showOptionsTab() {
    $('#show-option-card-btn').hide();
    $('#hide-option-card-btn').fadeIn('fast');
    $('#advance_option_card').fadeIn('fast');
}

function hideOptionsTab() {
    $('#hide-option-card-btn').hide();
    $('#show-option-card-btn').fadeIn('fast');
    $('#advance_option_card').fadeOut('fast');
}

// function random position
function randomPosition() {
    $('#grid-containter').fadeOut(0);

    var totalGrid = $('#total_grids').val();
    var gridData;
    var pivotPoint;
    var mixTime = 1;

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

function rotateAll() {
    var totalGrid = $('#total_grids').val();
    for (var i = 0; i <= totalGrid; ++i) {
        $('#grid-' + i).click();
    }
}

