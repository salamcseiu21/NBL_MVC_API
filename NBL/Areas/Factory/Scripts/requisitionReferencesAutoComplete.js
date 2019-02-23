$(function () {
    $("#RequisitionRef").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl + 'factory/transfer/GetRequisitionRefeAutoComplete/',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            return item;
                        }));
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#RequisitionId").val(i.item.val);
            var requisitionId = i.item.val;
            var json = { requisitionId: requisitionId };

            $.ajax({
                type: "POST",
                url: RootUrl + 'factory/transfer/GetRequisitionById',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    var qty = data.Quantity;
                    $("#RequisitionQty").val(qty);
                }
            });
        },
        minLength: 1
    });
});