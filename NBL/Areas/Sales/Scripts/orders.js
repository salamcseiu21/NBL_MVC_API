
$(function () {
    $("#ProductName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl+'Sales/Order/ProductNameAutoComplete/',
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
            $("#ProductId").val(i.item.val);
            var json = { productId: i.item.val };
            $.ajax({
                type: "POST",
                url: RootUrl + 'Common/GetProductById/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {

                    var unitPrice = data.UnitPrice;
                    var dealerPrice = data.DealerPrice;
                    var vat = data.Vat;
                    var dealerComision = data.DealerComision;
                    $("#UnitPrice").val(unitPrice);
                    $("#DealerPrice").val(dealerPrice);
                    $("#Vat").val(vat);
                    $("#DealerComision").val(dealerComision);
                }
            });

            $.ajax({
                type: "POST",
                url: RootUrl + 'Common/GetProductQuantityInStockById/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    var qty = data.StockQty;
                    $("#StockQty").val(qty);
                    $("#Quantity").attr("max", qty);
                }
            });
        },
        minLength: 1
    });
});

var productIdlist = [];
function AddItemToList(btnClicked) {
    var id = $("#ProductId").val();
    var a = $.inArray(id, productIdlist);
    if (a < 0) {
        productIdlist.push(id);
        var stock = $("#StockQty").val();
        var qty = $("#Quantity").val();
        if (stock - qty >= 0) {
            var $form = $(btnClicked).parents('form');
            $.ajax({
                type: "POST",
                url: RootUrl + 'Sales/Order/Order',
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
        } else {
            alert("Quantity Out of Stock!");
        }
    }
    else {
        alert("This Product already exits in the list");
    }

}
function Update(btnClicked) {
    $("#productIdToRemove").val(0);
    var $form = $(btnClicked).parents('form');
    //var quantiy = btnClicked.id;
    //var oldQty = btnClicked.value;
    var oq = $("#StockQty").val();
    var q = oq - btnClicked.value;
    if (q >= 0) {
        //alert("OK");
        $.ajax({
            type: "POST",
            url: RootUrl + 'Sales/Order/Update',
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
    } else {
        alert("Quantity out of stock");
        ViewTempOrders();
        return $form;
    }


}
function RemoveProduct(btnClicked) {


    if (confirm("Are you sure to remove this product ??")) {
        //alert(btnClicked);
        $("#productIdToRemove").val(btnClicked.id);
        //alert(btnClicked.id);
        //alert(btnClicked["delBtn"]);
        var $form = $(btnClicked).parents('form');
        $.ajax({
            type: "POST",
            url: RootUrl + 'Sales/Order/Update',
            data: $form.serialize(),
            error: function (xhr, status, error) {
                //do something about the error
            },
            success: function (response) {
                //alert("Saved Successfully");
                for (var i = productIdlist.length - 1; i >= 0; i--) {
                    if (productIdlist[i] === btnClicked.id) {
                        productIdlist.splice(i, 1);
                    }
                }
                ViewTempOrders();
            }
        });
    } else {
        return false;// if it's a link to prevent post
    }

    return false;// if it's a link to prevent post

}




function ViewTempOrders() {
    $("#orders").html("");

    $.ajax({
        type: "GET",
        url: RootUrl + 'Sales/Order/GetProductList',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var total = 0;
            for (var i = 0; i < data.length; i++) {
                total = total + data[i].SubTotal;

            }
            $.each(data, function (key, value) {

                //total =+ value.SubTotal;
                //alert(key);
                //$("#orders").append('<option value=' + value.ClientId + '>' + value.ProductId + '</option>');
                var row = $("<tr><td style='border: 1px solid black; padding: 5px 10px;'>" + value.ProductName + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.UnitPrice + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.Vat + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.DiscountAmount + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.SalePrice + "</td><td style='border: 1px solid black; padding: 5px 10px;'>  <input type='number' min='1' value='" + value.Quantity + "' class='form-control text-right' id='" + value.ProductId + "' name='NewQuantity_" + value.ProductId + "'  onchange='Update(this)'/>" + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'><input type='hidden' name='product_Id_" + value.ProductId + "' value='" + value.ProductId + "'> " + value.SubTotal + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-center'><button id='" + value.ProductId + "' type='button' onclick='RemoveProduct(this)' class='btn btn-default btn-sm'><i class='fa fa-times' style='color:red;'></i></button>" + "</td></tr>");

                $("#orders").append(row);
            });
            $("#Total").val(total.toFixed(2));
            var discount = $("#SD").val();
            var net = total - discount;
            $("#NetAmount").val(net.toFixed(2));
        }
    });
}