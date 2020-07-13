﻿var footer = {
    init: function () {
        footer.registerEvent();
    },
    registerEvent: function () {
        $(".btn-active-footer").off("click").on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(btn).attr("data-id");
            $.ajax({
                type: "POST",
                url: "/Admin/Footer/ChangeStatus",
                data: {
                    id: id
                },
                success: function (res) {
                    if (res.status) {
                        toastr.success("Update thành công", "Thông báo");
                        $(btn).html("<strong> Active <i class='fa fa-check-circle' aria-hidden='true'></i></strong>");
                    } else {
                        toastr.success("Update thành công", "Thông báo");
                        $(btn).html("<strong style='color: red;'> Lock </strong>");
                    }
                }
            })
        });
    }
};
footer.init();