function loadAllDeliverableOrderedProducts() {
    var invoiceId = $("#InvoiceId").val();
    $.ajax({
        type: 'GET',
        url: RootUrl + "Manager/Delivery/LoadDeliverableProduct",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { invoiceId: invoiceId },
        success: function (data) {
            $('#table_deliverable_product_List').dataTable({
                destroy: true,
                responsive:true,
                data: data,
                columns: [
                    { 'data': 'ProductName' },
                    { 'data': 'Quantity' },
                    {
                        data: null,
                        className: "text-center",
                        render: function (data, type, row) {
                            if (data.ScannedProductCodes != null) {
                                return data.ScannedProductCodes.substr(0, data.ScannedProductCodes.length - 1);
                            }
                            return data.ScannedProductCodes;
                        }
                    }

                ]
            });
        }
    });
}