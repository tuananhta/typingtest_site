// OPEN POPUP MODAL
$('.openModal').click(function () {
    $('.modalFrame').modal('show');
    var url = $(this).data('url');
    $.get(url, function (data) {
        $('.modalFrame').html(data);
    });
});

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
