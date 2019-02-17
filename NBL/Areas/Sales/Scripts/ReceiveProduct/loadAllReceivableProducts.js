function loadAllReceivableProducts() {
    var deliveryId = $("#DeliveryId").val();

    $.ajax({
        type: 'GET',
        url: RootUrl + "Sales/Product/LoadReceiveableProduct",
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
                            return data.ProductBarCode;
                        }
                    },
                    {
                        'data': null,
                        className: "text-center",
                        'render': function (data, type, row) {
                            if (data.RecievedProductBarCodes != null) {
                                return data.RecievedProductBarCodes.substr(0, data.RecievedProductBarCodes.length - 1);
                            }
                            return data.RecievedProductBarCodes;
                        }
                    }
                ]
            });
        }
    });
}

