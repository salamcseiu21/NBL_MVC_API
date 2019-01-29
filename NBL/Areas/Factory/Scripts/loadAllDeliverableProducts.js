function loadAllDeliverableProducts() {
    var issueId = $("#TransferIssueId").val();
    $.ajax({
        type: 'GET',
        url: RootUrl + "Factory/Delivery/LoadDeliverableProduct",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { issueId: issueId },
        success: function (data) {
            $('#table_deliverable_issue_details').dataTable({
                destroy: true,
                data: data,
                columns: [
                    { 'data': 'ProductName' },
                    { 'data': 'Quantity' },
                    {
                        data: null,
                        className: "text-center",
                        render: function (data, type, row) {
                            return data.ScannedProductCodes;
                        }
                    }
                    
                ]
            });
        }
    });
}