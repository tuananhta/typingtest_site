// OPEN POPUP MODAL
$('.openModal').click(function () {
    $('.modalFrame').modal('show');
    var url = $(this).data('url');
    $.get(url, function (data) {
        $('.modalFrame').html(data);
    });
});

$('.modalFrame').on('hidden.bs.modal', function () {
    var data = "<div class='modal-dialog'><div class='modal-content'><div class='modal-header'><button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button><h4 class='modal-title' id='myModalLabel'>Loading ... </h4></div><div id='bodyEditModal'><div class='modal-body text-justify'><!-- Content is here --><div class='container text-center'><h4 style='margin-top:30px; margin-bottom:30px;'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span> Loading</h4></div></div><div class='modal-footer'><button type='button' class='btn btn-default' data-dismiss='modal'>Close</button></div></div></div></div>";
    $('.modalFrame').html(data);
})

// HIDE SHOW WINDOWS SEARCH WORD INDEX
// show search tab
function showSearchWordsTab() {
    $('#search-words-tab').fadeIn('slow');
    $('#show-search-word-btn').hide();
    $('#hide-search-word-btn').fadeIn('fast');
}

function hideSearchWordsTab() {
    $('#search-words-tab').fadeOut(300);
    $('#hide-search-word-btn').hide();
    $('#show-search-word-btn').fadeIn('fast');
}

$(function () {
    $('#myTab a:last').tab('show')
})
