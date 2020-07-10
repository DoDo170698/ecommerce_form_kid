var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        // source-image
        $("#library-image").off("click").on("click", function (e) {
            e.preventDefault();
            // load all image in folder
            $("#listImagesModel").modal("show");
            $("#listImageList").html("");
            $.ajax({
                type: "GET",
                url: "/admin/product/GetAllImage/",
                data: {},
                success: function (res) {
                    if (res.status) {
                        console.log(res.lst);
                        for (let item of res.lst) {
                            $("#listImageList").append('<div style="float: left; margin-right: 5px;"> <img src="/Data/images/' + item + '" style="width:100%" /> <a data-imageName=' + item + ' href="javascript:void(0)" class="btn-delete-source-img"><i class="fa fa-times"></i> </a> </div>');
                        }

                        $(".btn-delete-source-img").off("click").on("click", function (e) {
                            //alert("keke");
                            e.preventDefault();
                            $(this).parent().remove();
                            var imageName = $(this).attr("data-imageName");
                            $.ajax({
                                type: "POST",
                                url: "/admin/product/RemoveImage/",
                                data: {
                                    name: imageName
                                },
                                success: function (res) {
                                    if (res.status) {
                                        alert("xóa thành công");
                                    }
                                }
                            });
                        });

                    }
                },
                error: function (res) {
                    console.log(res);
                    if (res.status) {
                        console.log(res.lst);
                    }
                }
            });
        });
        $(".btn-delete-source-img").off("click").on("click", function (e) {
            //alert("keke");
            e.preventDefault();
            $(this).parent().remove();

        });
        $("#btnChooListImages").off("click").on("click", function (e) {
            //alert('keke');          
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $("#listImageList").append('<div style="float: left; margin-right: 5px;"> <img src="' + url + '" style="width:100%" /> <a href="#" class="btn-delete-list-img"><i class="fa fa-times"></i> </a> </div>');
                $(".btn-delete-list-img").off("click").on("click", function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
            };
            finder.popup();
        });
        // image-product
        $(".btn-images").off("click").on("click", function (e) {
            e.preventDefault();
            $("#imageList").html("");
            $("#imagesModel").modal("show");
            $("#hiProductId").val($(this).data("id"));
            product.loadImage();
        });
        $("#btnChooImages").off("click").on("click", function (e) {
            //alert('keke');          
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                console.log(url);
                $("#imageList").append('<div style="float: left; margin-right: 5px;"> <img data-new="isNew" data-src="' + url + '" src="' + url + '" style="width:100%" /> <a href="#" class="btn-delete-img"><i class="fa fa-times"></i> </a> </div>');
                $(".btn-delete-img").off("click").on("click", function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
            };
            finder.popup();
        });
        $("#btn-save-image").off("click").on("click", function () {
            var images = [];
            var id = $("#hiProductId").val();
            var imgNew = $("#imageList img[data-new=isNew]");
            $.each($(imgNew), function (i, item) {
                console.log(item);
                images.push($(item).attr("data-src"));
            });
            console.log(JSON.stringify(images));
            $.ajax({
                type: "POST",
                url: "/Admin/Product/SaveImages/",
                dataType: "json",
                data: {
                    id: id,
                    images: JSON.stringify(images)
                },
                success: function (res) {
                    $("#imagesModel").modal("hide");
                    $("#imageList").html("");
                }
            })
        });
        $("#btn_image").off("change").on("change", function () {
            if (this.files && this.files[0]) {
                var filerdr = new FileReader();
                filerdr.onload = function (e) {
                    $('#img_product').attr('src', e.target.result);
                }
                filerdr.readAsDataURL(this.files[0]);
            }
        });
        $(".btn-active-product").off("click").on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(btn).attr("data-id");
            $.ajax({
                type: "POST",
                url: "/Admin/Product/ChangeStatus",
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
            });
        })
    },
    loadImage: function () {
        var id = $("#hiProductId").val();
        $.ajax({
            type: "GET",
            url: "/Admin/Product/LoadImages/",
            data: {
                id: id
            },
            success: function (res) {
                if (res.status) {
                    $.each(res.lst, function (i, data) {
                        $("#imageList").append('<div style="float: left; margin-right: 5px;"> <img data-src="' + data + '" src="' + data + '" style="width:100%" /> <a href="#" class="btn-delete-img"><i class="fa fa-times"></i> </a> </div>');
                    });

                    $(".btn-delete-img").off("click").on("click", function (e) {
                        e.preventDefault();
                        $(this).parent().remove();
                    })
                }
            }
        })
    }
}
product.init();