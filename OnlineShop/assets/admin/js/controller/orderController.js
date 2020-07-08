var order = {
    init: function () {
        order.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active-order').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(btn).data('id');
            $.ajax({
                url: "/Admin/Order/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true && response.check == true) {
                        $(btn).html("<strong> Đã duyệt <i class='fa fa-check-circle' aria-hidden='true'></i></strong>");
                    }
                    else if (response.status == false && response.check == true) {
                        $(btn).html("<strong style='color: red;'> Chưa duyệt </strong>");
                    }
                    else if (response.check == false && response.status == false) {
                        toastr.error(response.mess, "thông báo");
                    }                  
                }
            });
        });

        $(".btn-reg-shipper").off("click").on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var orderID = $(btn).attr("data-id");
            var userID = $(btn).attr("data-userID");
            $.ajax({
                type: "POST",
                url: "/Admin/ShipperOrder/CreateShipper/",
                data: {
                    userID: userID,
                    orderID: orderID
                },
                success: function (res) {
                    if (res.status) {
                        toastr.success(res.mess, 'thông báo');
                        $(btn).parent().html(`<a href="javascript:void(0)" class="btn-remov-shipper" data-userID="${userID}" data-id="${orderID}"> <strong> Đã đặt ship <i class='fa fa-check-circle' aria-hidden='true'></i></strong></a>`);
                        $(".btn-remov-shipper").off("click").on("click", function (e) {
                            e.preventDefault();  
                            var btn2 = $(this);
                            var orderID2 = $(btn).attr("data-id");
                            var userID2 = $(btn).attr("data-userID");
                            $.ajax({
                                type: "POST",
                                url: "/Admin/ShipperOrder/DeleteShipper",
                                data: {
                                    orderID: orderID
                                },
                                success: function (res) {
                                    if (res.status) {
                                        $(btn2).parent().html(`<a href="javascript:void(0)" class="btn-reg-shipper" data-userID="${userID2}" data-id="${orderID2}"> <strong style='color: red;'> Chưa đặt ship </strong> </a>`);
                                        location.reload();
                                    }
                                    else if (!res.status && !res.check) {
                                        toastr.error(res.mess, "Thông báo");
                                    }
                                }
                            });
                        });
                    } else {
                        toastr.error(res.mess, 'thông báo')
                        $(btn).html("<strong style='color: red;'> Chưa đặt ship </strong>");
                    }
                }
            });
        });

        $(".btn-remov-shipper").off("click").on("click", function (e) {
            e.preventDefault();
            var orderID = $(this).attr("data-id");
            var userID = $(this).attr("data-userID");
            var btn = $(this);
            $.ajax({
                type: "POST",
                url: "/Admin/ShipperOrder/DeleteShipper",
                data: {
                    orderID: orderID
                },
                success: function (res) {
                    if (res.status) {
                        $(btn).parent().html(`<a href="javascript:void(0)" class="btn-reg-shipper" data-userID="${userID}" data-id="${orderID}"> <strong style='color: red;'> Chưa đặt ship </strong> </a>`);
                        $(".btn-reg-shipper").off("click").on("click", function (e) {
                            e.preventDefault();  
                            var orderID1 = $(this).attr("data-id");
                            var userID1 = $(this).attr("data-userID");
                            var btn1 = $(this);
                            $.ajax({
                                type: "POST",
                                url: "/Admin/ShipperOrder/CreateShipper/",
                                data: {
                                    userID: userID1,
                                    orderID: orderID1
                                },
                                success: function (res) {
                                    if (res.status) {
                                        toastr.success(res.mess, 'thông báo');
                                        $(btn1).parent().html(`<a href="javascript:void(0)" class="btn-remov-shipper" data-userID="${userID1}" data-id="${orderID1}"> <strong> Đã đặt ship <i class='fa fa-check-circle' aria-hidden='true'></i></strong></a>`);                                       
                                        location.reload();
                                    } else {
                                        toastr.error(res.mess, 'thông báo')
                                        $(btn1).html("<strong style='color: red;'> Chưa đặt ship </strong>");
                                    }
                                }
                            });
                        });
                    }
                    else if (!res.status && !res.check) {
                        toastr.error(res.mess, "Thông báo");
                    }
                }
            });
        });
    }
};
order.init();