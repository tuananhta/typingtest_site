﻿// Flash Card
$(function () {
    $(".card-grid").flip({
        trigger: 'click'
    });
});

$(document).ready(function () {
    $('.color-opt').hide();
    $('.card-grid').fadeIn('fast');

    // check current face of flash cards
    $('.card-grid').click(function () {
        var face = $(this).find(".current_face").val();
        if (face == "front") {
            $(this).find(".current_face").val("back");
        }
        else {
            $(this).find(".current_face").val("front");
        }
    })

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

    $('#front-' + id).find(".true_favourite").hide();
    $('#front-' + id).find(".false_favourite").hide();
    $('#front-' + id).find(".saving_status").show();
    $('#front-' + id).find(".fFavourite-input").val(false);
    $.ajax({
        url: "/flashcards/EditFavourite",
        type: "POST",
        data: $('#front-' + id).find(".change_favourite_form").serialize(),
        beforeSend: function () {
            $('#front-' + id).find(".saving_status").show();
        },
        success: function (data) {
            $('#front-' + id).find(".saving_status").hide();
            $('#front-' + id).find(".true_favourite").fadeIn();
        },
    });
}

function removeFavourite(event, id) {
    event.stopPropagation();
    
    $('#front-' + id).find(".true_favourite").hide();
    $('#front-' + id).find(".false_favourite").hide();
    $('#front-' + id).find(".saving_status").show();
    $('#front-' + id).find(".fFavourite-input").val(false);

    $.ajax({
        url: "/flashcards/EditFavourite",
        type: "POST",
        data: $('#front-' + id).find(".change_favourite_form").serialize(),
        beforeSend: function () {
            $('#front-' + id).find(".saving_status").show();
        },
        success: function (data) {
            $('#front-' + id).find(".saving_status").hide();
            $('#front-' + id).find(".false_favourite").fadeIn();
        },
    });


    //submitChangeFavourite(id);
}

function submitChangeFavourite(id) {
    $("#success_info").empty().show();

    $.ajax({
        url: "/flashcards/EditFavourite",
        type: "POST",
        data: $('#front-' + id).find(".change_favourite_form").serialize(),
        beforeSend: function () {
            $("#success_info").empty().html('<div class="progress"><div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: 45%"><span class="sr-only"> Saving ... </span></div></div>');
            $("#success_info").delay(1000).fadeOut('slow');
        },
        success: function (data) {
            $("#success_info").empty().html('<div class="progress"><div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"><span class="sr-only">Saved!</span></div></div><div style="color:whitesmoke; font-size:18px; font-weight:600;">Saved!</div><br>');
            $("#success_info").delay(1000).fadeOut('slow');
        },
    });
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
    $('#grid-container-form').fadeOut("slow");
    rotateAll();
    if ($('#grid-1').find(".current_face").val() != "front") {
        rotateAll();
    }

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

    $('#grid-container-form').delay(100).fadeIn();
}

function updateColor(event, id, fColor) {
    event.stopPropagation();
    $('#front-' + id).closest("section").find('.fColor-input').val("flash-" + fColor);

    $('#front-' + id).removeClass();
    $('#back-' + id).removeClass();

    $('#front-' + id).addClass("front flash-" + fColor + "-front");
    $('#back-' + id).addClass("back flash-" + fColor + "-back");
}

var rotateTime = 1;

function rotateAll() {
    rotateTime = (rotateTime + 1) % 2;
    var totalGrid = $('#total_grids').val();
    if (rotateTime == 0) {
        for (var i = 0; i <= totalGrid; ++i) {
            if ($('#grid-' + i).find(".current_face").val() == "front") {
                $('#grid-' + i).click();
            }
        }
    }
    else {
        for (var i = 0; i <= totalGrid; ++i) {
            if ($('#grid-' + i).find(".current_face").val() == "back") {
                $('#grid-' + i).click();
            }
        }
    }
}

function changeCardSize(nOfcards) {
    var width = $(window).width() - 42;
    var height;
    var margin;
    width = (width / nOfcards) * 0.83;
    margin = width * 0.03;
    height = width * 0.55;

    $('.card-grid').css({ "width": width, "height": height });
    $('.flip').css({ "width": width, "height": height, "margin": margin, "perspective": width * 2 });
    $('.front').css({ "width": width, "height": height });
    $('.front').find("img").css({ "height": height / 6 });
    $('.front').find(".paint_btn").css({ "font-size": height / 6 });
    $('.front').find(".hash_tag_btn").css({ "font-size": height / 6 });

    $('.back').css({ "width": width, "height": height });
    $('.back').find("span").css({ "font-size": height / 13, "line-height": "1.333em" });
    $('.back').find("td").css({ "margin": "2px", "padding": "2px", "line-height": "1.333em" });
    $('.back').find("tr").css({ "margin": "2px", "padding": "2px", "line-height": "1.333em" });

    if (nOfcards > 3) {
        $('.back').find("span").css({ "line-height": "1.1em", "margin": 0, "padding": 0 });
        $('.back').find("td").css({ "margin": 0, "padding": 0, "line-height": "1.1em" });
        $('.back').find("tr").css({ "margin": 0, "padding": 0, "line-height": "1.1em" });
    }

    $('.front').find("h2").css({ "font-size": height / 5, "margin": height / 20, "margin-top": height / 100 });
    $('.front').find("h4").css({ "font-size": height / 10, "margin": height / 30, "margin-top": height / 100 });
    $('.front').find("h3").css({ "font-size": height / 8, "margin": height / 40, "margin-top": height / 100 });

    $('.front').find(".btn-flash").css({ "padding": height / 13, "border-width": height / 100 });
    $('.front').find(".pin_sticky").find('img').css({ "height": height / 12 });
    $('.front').find(".card_header").css({ "margin": height / 40 });


}