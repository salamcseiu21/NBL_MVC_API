function loadTempTripProducts() {
    $.ajax({
        url: RootUrl + "factory/transfer/GetTempTrip",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            // var qty = data.Quantity;
            // $("#RequisitionQty").val(qty);
            $('#Requisitiondetails').html(data);
        }
    });
}