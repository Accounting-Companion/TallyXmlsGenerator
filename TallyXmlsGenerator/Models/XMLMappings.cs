using TallyConnector.Core.Models;
using TallyConnector.Core.Models.Masters;
using TallyConnector.Core.Models.Masters.CostCenter;
using TallyConnector.Core.Models.Masters.Inventory;
using TallyConnector.Core.Models.Masters.Payroll;

namespace TallyXmlsGenerator.Models;
public record VouchersByVoucherTypeMapping(string Name, string Expression);
public record TallyObjectMapping(TallyObjectType TallyObjectType, Type ObjectType);
public static class XMLMappings
{
    public static List<VouchersByVoucherTypeMapping> VouchersByVoucherTypeMappings = new()
    {
        new(nameof(Constants.VoucherType.PaymentVoucherType),Constants.VoucherType.PaymentVoucherType),
        new(nameof(Constants.VoucherType.ReceiptNoteVoucherType),Constants.VoucherType.ReceiptVoucherType),
        new(nameof(Constants.VoucherType.ContraVoucherType),Constants.VoucherType.ContraVoucherType),

        new(nameof(Constants.VoucherType.JournalVoucherType),Constants.VoucherType.JournalVoucherType),
        new(nameof(Constants.VoucherType.ReversingJournalVoucherType),Constants.VoucherType.ReversingJournalVoucherType),

        new(nameof(Constants.VoucherType.SalesVoucherType),Constants.VoucherType.SalesVoucherType),
        new(nameof(Constants.VoucherType.PurchaseVoucherType),Constants.VoucherType.PurchaseVoucherType),
        new(nameof(Constants.VoucherType.SalesOrderVoucherType),Constants.VoucherType.SalesOrderVoucherType),
        new(nameof(Constants.VoucherType.PurchaseOrderVoucherType),Constants.VoucherType.PurchaseOrderVoucherType),
        new(nameof(Constants.VoucherType.DebitNoteVoucherType),Constants.VoucherType.DebitNoteVoucherType),
        new(nameof(Constants.VoucherType.CreditNoteVoucherType),Constants.VoucherType.CreditNoteVoucherType),

        new(nameof(Constants.VoucherType.JobWorkOutVoucherType),Constants.VoucherType.JobWorkOutVoucherType),
        new(nameof(Constants.VoucherType.JobWorkInVoucherType),Constants.VoucherType.JobWorkInVoucherType),

        new(nameof(Constants.VoucherType.MaterialInVoucherType),Constants.VoucherType.MaterialInVoucherType),
        new(nameof(Constants.VoucherType.MaterialOutVoucherType),Constants.VoucherType.MaterialOutVoucherType),
        new(nameof(Constants.VoucherType.MemoVoucherType),Constants.VoucherType.MemoVoucherType),

        new(nameof(Constants.VoucherType.DeliveryNoteVoucherType),Constants.VoucherType.DeliveryNoteVoucherType),
        new(nameof(Constants.VoucherType.ReceiptNoteVoucherType),Constants.VoucherType.ReceiptNoteVoucherType),

        new(nameof(Constants.VoucherType.StockJournalVoucherType),Constants.VoucherType.StockJournalVoucherType),
        new(nameof(Constants.VoucherType.PhysicalStockVoucherType),Constants.VoucherType.PhysicalStockVoucherType),

        new(nameof(Constants.VoucherType.RejectionsInVoucherType),Constants.VoucherType.RejectionsInVoucherType),
        new(nameof(Constants.VoucherType.RejectionsOutVoucherType),Constants.VoucherType.RejectionsOutVoucherType),

        new(nameof(Constants.VoucherType.PayrollVoucherType),Constants.VoucherType.PayrollVoucherType),
        new(nameof(Constants.VoucherType.AttendanceVoucherType),Constants.VoucherType.AttendanceVoucherType),
    };

    public static List<TallyObjectMapping> tallyObjectMappings = new()
    {
        new(TallyObjectType.Currencies,typeof(Currency)),

        new(TallyObjectType.Groups,typeof(Group)),
        new(TallyObjectType.Ledgers,typeof(Ledger)),

        new(TallyObjectType.CostCategories,typeof(CostCategory)),
        new(TallyObjectType.CostCentres,typeof(CostCenter)),

        new(TallyObjectType.StockCategories,typeof(StockCategory)),
        new(TallyObjectType.StockGroups,typeof(StockGroup)),
        new(TallyObjectType.StockItems,typeof(StockItem)),
        new(TallyObjectType.Units,typeof(Unit)),
        new(TallyObjectType.Godowns,typeof(Godown)),

        new(TallyObjectType.VoucherTypes,typeof(VoucherType)),


        new(TallyObjectType.EmployeeGroups,typeof(EmployeeGroup)),
        new(TallyObjectType.Employees,typeof(Employee)),
    };
}
