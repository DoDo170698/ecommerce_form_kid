var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });
            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json', 
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
        // using add to cart ajax
        $(".btnAddToCartAjax").off("click").on("click", function (e) {
            //alert("keke");
            e.preventDefault();
            var productId = $(this).attr("data-id");
            var quantity = $(this).attr("data-quantity");
            console.log(productId, quantity);
            $.ajax({
                type: "POST",
                url: "/cart/AddItemAjax/",
                data: {
                    productId: productId,
                    quantity: quantity
                },
                success: function (req) {
                    if (req.status) {
                        toastr.success('thêm vào giỏ hàng', 'thông báo');
                        var count = 0;
                        console.log(req.lst);
                        req.lst.forEach(element => {
                            count += element.Quantity * element.Product.Price;
                        });
                        $("#count_cart").html("$" + count);
                    }
                }
            });
        })
    }
}
cart.init();