$(document).ready(function () {
    $("#activityDate").on('change',
        function() {
            var $form = $(this).closest('form');
            $form.submit();
        });
});

function deletePost(id) {
    var ask = window.confirm("Aktivitenizin aktiflikten çıkartılacak. Onaylıyor Musunuz?");
    if (ask) {
        window.location.href = "/Account/Delete/" + id;

    }
}