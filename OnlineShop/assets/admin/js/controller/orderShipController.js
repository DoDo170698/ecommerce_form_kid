var ship = {
    init: function () {
        ship.registerEvents();
    },
    registerEvents: function () {
        $(".btn-status-order").off("click").on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var orderID = $(btn).attr("data-id");
            $.ajax({
                type: "POST",
                url: "/ShipperOrder/ChangeStatus",
                data: {
                    orderID: orderID
                },
                success: function (res) {
                    if (res.status) {
                        toastr.success("chuyển trạng thái thành công", "thông báo");
                        $(btn).html("<strong> Giao hàng thành công <i class='fa fa-check-circle' aria-hidden='true'></i></strong>");
                    } else {
                        toastr.success("chuyển trạng thái thành công", "thông báo");
                        $(btn).html("<strong style='color: red;'> Chưa giao hàng </strong>");
                    }
                }
            })
        });
    }
};
ship.init();