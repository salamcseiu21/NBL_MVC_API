﻿function Update(btnClicked) {
    $("#productIdToRemove").val(0);
    var $form = $(btnClicked).parents('form');
    var pId = btnClicked.id;
    var json = { productId: pId };

    $.ajax({
        type: "POST",
        url: RootUrl + 'Common/GetProductQuantityInStockById',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(json),
        success: function (data) {
            $.ajax({
                type: "POST",
                url: RootUrl + 'Nsm/Order/Update',
                data: $form.serialize(),
                error: function (xhr, status, error) {
                    //do something about the error
                },
                success: function (response) {
                    //alert("Saved Successfully");
                    ViewTempOrders();
                }
            });

            return false; // if it's a link to prevent post
        }
    });

}