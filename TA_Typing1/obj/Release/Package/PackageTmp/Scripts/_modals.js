// OPEN POPUP MODAL
$('.openModal').click(function () {
    $('.modalFrame').modal('show');
    var url = $(this).data('url');
    $.get(url, function (data) {
        $('.modalFrame').html(data);
    });
});