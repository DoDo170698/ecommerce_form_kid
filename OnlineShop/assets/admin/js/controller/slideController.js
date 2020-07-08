var slide = {
    init: function () {
        slide.registerEvent();
    },
    registerEvent: function () {
        $("#btn_file").off("change").on("change", function () {
            if (this.files && this.files[0]) {
                var filerdr = new FileReader();
                filerdr.onload = function (e) {
                    $('#user_img').attr('src', e.target.result);
                }
                filerdr.readAsDataURL(this.files[0]);
            }
        });
        $(".btn-change-status").off("click").on("click", function (e) {
            e.preventDefault();
            var id = $(this).attr("data-id");
            var btn = $(this);
            $.ajax({
                type: "POST",
                url: "/Admin/Slide/ChangeStatus/",
                data: {
                    id: id
                },
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        toastr.success("Update thành công", "Thông báo");
                        $(btn).html("<strong> Active <i class='fa fa-check-circle' aria-hidden='true'></i></strong>");
                    }
                    else {
                        toastr.success("Update thành công", "Thông báo");
                        $(btn).html("<strong style='color: red;'> Lock </strong>");
                    }
                }
            });
        });
    }
};
slide.init();