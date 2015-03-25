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
    $('.color-opt').fadeIn('fast');
    $('#submit-color-option').fadeIn('fast');
    $('#paint-card-btm').removeClass('refresh-btn');
    $('#paint-card-btm').parent().removeClass('col-sm-3 margin-top-bottom');

    $('#hide-search-card-btn').parent().removeClass('margin-top-bottom');
    $('#rand-position-btn').parent().removeClass('margin-top-bottom');
    $('#favourite-card-btm').parent().removeClass('margin-top-bottom');
    $('#show-option-card-btn').parent().removeClass('margin-top-bottom');

    $('#hide-search-card-btn').parent().html("");
    $('#rand-position-btn').parent().html("");
    $('#favourite-card-btm').parent().html("");
    $('#show-option-card-btn').parent().html("");

    $('#header_layout').hide();
    $('#header2-flash-cards').addClass('navbar navbar-default navbar-fixed-top header-fixed');
}

// submit forms flash cards
function submitColorOptions() {
    $('.form-color-option').submit();
}

// show favourite button
function showFavouriteOption() {
    $('.favourite-opt').fadeIn('fast');
    $('#submit-favourite-option').fadeIn('fast');
    $('#favourite-card-btm').removeClass('refresh-btn');
    $('#favourite-card-btm').parent().removeClass('col-sm-3 margin-top-bottom');

    $('#hide-search-card-btn').parent().removeClass('margin-top-bottom');
    $('#rand-position-btn').parent().removeClass('margin-top-bottom');
    $('#paint-card-btm').parent().removeClass('margin-top-bottom');
    $('#show-option-card-btn').parent().removeClass('margin-top-bottom');

    

    $('#hide-search-card-btn').parent().hide();
    $('#rand-position-btn').parent().hide();
    $('#paint-card-btm').parent().hide();
    $('#show-option-card-btn').parent().hide();

    $('#header_layout').hide();
    $('#header2-flash-cards').addClass('navbar navbar-default navbar-fixed-top header-fixed');
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

function rotateAll() {
    var totalGrid = $('#total_grids').val();
    for (var i = 0; i <= totalGrid; ++i) {
        $('#grid-' + i).click();
    }
}