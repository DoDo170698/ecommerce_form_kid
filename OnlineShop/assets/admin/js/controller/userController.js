var user = {
    init: function () {
        user.registerEvents();
        user.addGroup();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        $(btn).html("<strong> Active <i class='fa fa-check-circle' aria-hidden='true'></i></strong>");
                    }
                    else {
                        $(btn).html("<strong style='color: red;'> Lock </strong>");
                    }
                }
            });
        });
    },
    addGroup: function () {
       
    }
    //OnFailse: function () {
    //    alert('Trạng thái Active, không được xóa');
    //}
}
$(document).ready(function () {
    user.init();
});
