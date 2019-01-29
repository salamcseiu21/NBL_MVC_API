function loadAllReceivableProducts() {
    var deliveryId = $("#DeliveryId").val();
    $.ajax({
        type: 'GET',
        url: RootUrl + "Sales/Product/LoadReceiveableProduct",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { deliveryId: deliveryId },
        success: function (data) {
            $('#tbl_receivable_list').dataTable({
                destroy: true,
                data: data,
                columns: [
                    { 'data': 'ProductName' },
                    { 'data': 'Quantity' },
                    {
                        data: null,
                        className: "text-center",
                        render: function (data, type, row) {
                            return data.ProductBarCodes;
                        }
                    },
                    {
                        data: null,
                        className: "text-center",
                        render: function (data, type, row) {
                            return data.RecievedProductBarCodes;
                        }
                    }
                ]
            });
        }
    });
}