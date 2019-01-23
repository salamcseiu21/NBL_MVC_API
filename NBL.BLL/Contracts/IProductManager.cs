using System.Collections.Generic;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
    public interface IProductManager
    {
        IEnumerable<Product> GetAll();
        IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId);
        int GetProductMaxSerialNo();
        IEnumerable<TransferIssue> GetDeliverableTransferIssueList();
        IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId);
        int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model);
        string Save(Product aProduct);
        bool ApproveTransferIssue(TransferIssue transferIssue);
        int IssueProductToTransfer(TransferIssue aTransferIssue);
        ProductDetails GetProductDetailsByProductId(int productId);
        IEnumerable<TransferIssue> GetTransferIssueList();
        IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id);
        Product GetProductByProductAndClientTypeId(int productId, int clientTypeId);
        Product GetProductByProductId(int productId);
        bool SaveProductionNote(ProductionNote productionNote);
        IEnumerable<ViewProductionNoteModel> PendingProductionNote();
        TransferIssue GetTransferIssueById(int transerIssueId);
        TransferIssue GetDeliverableTransferIssueById(int transerIssueId);

        ICollection<Product> GetProductsFromTextFile(string filePath);
        bool AddProductToTextFile(string productCode, string filePath);
        bool AddProductToInventory(List<Product> products);
    }
}
