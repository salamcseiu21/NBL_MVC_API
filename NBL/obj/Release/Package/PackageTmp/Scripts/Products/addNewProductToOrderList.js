function AddItemToList(btnClicked) {
    var id = $("#ProductId").val();
    //alert(btnClicked.id);

    var a = $.inArray(id, productIdlist);

    if (a < 0) {
        productIdlist.push(id);
        var $form = $(btnClicked).parents('form');
        $.ajax({
            type: "POST",
            url: RootUrl + 'Nsm/Order/AddNewItemToExistingOrder',
            data: $form.serialize(),
            error: function (xhr, status, error) {
                //do something about the error
            },
            success: function (response) {
                //alert("Saved Successfully");
                ViewTempOrders();
            }
        });
        
    }
    else {
        alert("This Product already exits in the list");
    }

}