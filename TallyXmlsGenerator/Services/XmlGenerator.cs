using System.Text.Json;
using TallyXmlsGenerator.Models;
using TallyConnector.Core.Models;
using System.Reflection;
using System.Text;
using TallyConnector.Core.Models.Masters;

namespace TallyXmlsGenerator.Services;
public class XmlGenerator
{
    private TallyHelperService tallyHelperService;

    private (DateTime, DateTime) FY;
    public XmlGenerator()
    {
        tallyHelperService = new TallyHelperService();
    }

    public void GenerateXmlsOnly()
    {
        string salesreportXml = GetVoucherByVoucherType("$$VchTypeSales");
    }

    public string GeneratePostManCollectionJson()
    {
        PostManCollection postManCollection = new("13855108-70cf4ea5-b8c6-4c9c-8c9b-5cce9061b2fd",
                                                  "Tally XMLS for Integration with Third party Apps",
                                                  "When Developing  [Tally Connector Library](https://github.com/Accounting-Companion/TallyConnector) , I did some research to get" +
                                                  " data from Tally in easier way using XML API and " +
                                                  "found these xmls useful\n\nFollowing are the Xml Requests To " +
                                                  "Communicate with Tally Prime/ Tally ERP 9\n\nIf you want to suggest any changes " +
                                                  "or found any issues contact me\n\nIf you want " +
                                                  "complete integration support contact me at [contact@saivineeth.com]" +
                                                  "(mailto:contact@saivineeth.com) \n\n" +
                                                  "or you can purchase from [Upwork](https://www.upwork.com/services/product/development-it-a-fantastic-app-that-integrates-tally-tally-prime-with-any-application-1376166703485251584?ref=project_share&tier=1)",
                                                  "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
                                                  "13855108");

        postManCollection.Variable = new()
        {
            new("TallyPort", "9000"),
            new("TallyURL", "http://localhost")
        };

        postManCollection.Item = new()
        {
            GetExportCollection(),
            GetImportCollection(),
            //Adds Test Request
            new("Test", "", ""),
            //Adds  Request to get Current Company
            new("GetActiveCompany", PrefixGeneratedByText(tallyHelperService.GetActiveCompanyXml()), "")
        };
        var json = JsonSerializer.Serialize(new CollectionRoot(postManCollection), new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        return json;
    }

    private Item GetExportCollection()
    {
        return new("Export")
        {
            ChildItems = new()
            {
                new("Create"){ChildItems=new(){GetCollectionItem(),GetReportsItem()},Id="7ff8a57d-d4f6-46e0-aa75-0ef50ef4a8be"},

            },
            Id = "51e6fcfd-d260-4ba9-ab28-16df9f2a78bc",
            Description = "XMLS to Export data from Tally"
        };
    }

    private Item GetMastersCollection()
    {
        Item item = new("Masters");
        item.ChildItems = new()
        {
            GetLedgerCollectionItem()
        };
        return item;
    }

    private Item GetLedgerCollectionItem()
    {
        Item item = new("Ledger")
        {
            ChildItems = new()
        };
        item.ChildItems.Add(new("BasicLedger", PrefixGeneratedByText(tallyHelperService.PostObjectToTallyXML<Ledger>(new("Test Ledger", "Sundry Debtors"))), ""));
        item.ChildItems.Add(new("LedgerwithOpeningBalance", PrefixGeneratedByText(tallyHelperService.PostObjectToTallyXML<Ledger>(new("Test Ledger 2", "Sundry Debtors")
        {
            OpeningBal = 8000
        })), ""));
        return item;
    }
    private Item GetVoucherCollectionItem()
    {
        Item item = new("Voucher")
        {
            ChildItems = new()
        };
        Voucher basicVoucher = CreateBasicVoucher();
        Voucher basicInvoiceVoucher = CreateBasicInvoiceVoucher();
        Voucher VoucherwithCostCenter = CreateVoucherwithCostCenter();
        Voucher InvoiceVoucherwithCostCenter = CreateInvoiceVoucherwithCostCenter();

        item.ChildItems.Add(new(nameof(basicVoucher),
                                PrefixGeneratedByText(tallyHelperService.PostVoucherToTallyXML(basicVoucher)),
                                ""));

        item.ChildItems.Add(new(nameof(basicInvoiceVoucher),
                                PrefixGeneratedByText(tallyHelperService.PostVoucherToTallyXML(basicInvoiceVoucher)),
                                ""));

        item.ChildItems.Add(new(nameof(VoucherwithCostCenter),
                                PrefixGeneratedByText(tallyHelperService.PostVoucherToTallyXML(VoucherwithCostCenter)),
                                ""));
        item.ChildItems.Add(new(nameof(InvoiceVoucherwithCostCenter),
                                PrefixGeneratedByText(tallyHelperService.PostVoucherToTallyXML(InvoiceVoucherwithCostCenter)),
                                ""));
        return item;
    }

    private static Voucher CreateBasicVoucher()
    {
        return new()
        {
            VoucherType = "Sales",
            Date = DateTime.Now,
            Action = TallyConnector.Core.Models.Action.Create,
            View = VoucherViewType.AccountingVoucherView,
            Narration = "Being Sales made to Test Customer",
            Ledgers = new()
            {
                new("Test Customer",-9000),
                new("Sales",9000)
            }
        };
    }
    private static Voucher CreateBasicInvoiceVoucher()
    {
        Voucher voucher = CreateBasicVoucher();
        voucher.PartyName = voucher.Ledgers?.First().LedgerName;
        voucher.View = VoucherViewType.InvoiceVoucherView;
        return voucher;
    }
    private static Voucher CreateVoucherwithCostCenter()
    {
        Voucher voucher = CreateBasicVoucher();
        VoucherLedger ledger = voucher.Ledgers!.Last();
        ledger.CostCategoryAllocations = new()
        {
            new("Primary Cost Category",new(){new("Costcent1",9000)})
        };

        return voucher;
    }
    private static Voucher CreateInvoiceVoucherwithCostCenter()
    {
        Voucher voucher = CreateVoucherwithCostCenter();
        voucher.PartyName = voucher.Ledgers?.First().LedgerName;
        voucher.View = VoucherViewType.InvoiceVoucherView;
        return voucher;
    }

    private Item GetImportCollection()
    {
        return new("Import")
        {
            ChildItems = new()
            {
                GetMastersCollection(),
                GetVoucherCollectionItem()
            },
            Description = "XMLS to Import data To Tally"
        };
    }

    private Item GetReportsItem()
    {
        Item ReportItems = new("Reports");
        ReportItems.ChildItems = new();

        ReportItems.ChildItems.Add(new("Stats_Master",
                                        PrefixGeneratedByText(tallyHelperService.GetTDLReportXML<MasterTypeStat, MasterStatistics>(new()
                                        {
                                            Company = "ABC Company Ltd"
                                        })), ""));

        var Finacialyear = GetCurrentFinacialYear();

        ReportItems.ChildItems.Add(new("Stats_Voucher",
                                        PrefixGeneratedByText(tallyHelperService.GetTDLReportXML<VoucherTypeStat, VoucherStatistics>(new()
                                        {
                                            FromDate = Finacialyear.Item1,
                                            ToDate = Finacialyear.Item2,
                                            Company = "ABC Company Ltd"
                                        })), ""));

        ReportItems.ChildItems.Add(new("LicenseInfo", PrefixGeneratedByText(tallyHelperService.GetLicenseInfoXml()), ""));
        return ReportItems;
    }

    public Item GetCollectionItem()
    {
        Item CollectionItems = new("Collections");
        CollectionItems.ChildItems = new();
        //Adds Vouchers By VoucherTypeFolder to Collection
        CollectionItems.ChildItems.Add(new("VouchersByVoucherType")
        {
            ChildItems = GetAllVouchersByVoucherTypeItems(),
            Description = "Test Description"
        });

        //Adds Masters requests to collection
        CollectionItems.ChildItems.Add(new("Masters")
        {
            ChildItems = GetAllMasterItems(),
            Description = "Test Description"
        });

        //Adds Voucher Request to Collection
        CollectionItems.ChildItems.Add(new("Voucher", PrefixGeneratedByText(tallyHelperService.GetObjectsXML<Voucher>()), ""));
        return CollectionItems;
    }

    public async Task UpdateCollection(string CollectionId, string Token)
    {
        HttpClient httpClient = new();
        Console.WriteLine("Generating XML ...");
        var json = GeneratePostManCollectionJson();
        HttpContent content = new StringContent(json, encoding: Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-API-Key", Token);
        HttpRequestMessage requestMessage = new(HttpMethod.Put, $"https://api.getpostman.com/collections/{CollectionId}")
        {
            Content = content
        };
        Console.WriteLine("Updating collection ...");
        var httpResponse = await httpClient.SendAsync(requestMessage);
        if (httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Sucessfully updated collection");
        }
        else
        {
            Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            Console.WriteLine("Failed collection updation");
        }
    }

    private List<Item> GetAllMasterItems()
    {
        List<Item> Items = new();
        MethodInfo MethodInfo = tallyHelperService.GetType().GetMethod(nameof(tallyHelperService.GetObjectsXML))!;
        foreach (var item in XMLMappings.tallyObjectMappings)
        {
            MethodInfo GenMethodInfo = MethodInfo.MakeGenericMethod(item.ObjectType);
            //tallyHelperService.GetObjectsXML();
            Items.Add(new($"{item.TallyObjectType}", PrefixGeneratedByText((string)GenMethodInfo.Invoke(tallyHelperService, new object[] { null })!), ""));
        }
        return Items;
    }

    /// <summary>
    /// Created Request Item for every Voucher Type
    /// </summary>
    /// <returns></returns>
    private List<Item> GetAllVouchersByVoucherTypeItems()
    {
        List<Item> Items = new();
        foreach (var item in XMLMappings.VouchersByVoucherTypeMappings)
        {
            string Name = item.Name.Replace("VoucherType", "");
            Items.Add(new($"{Name}_Vouchers", GetVoucherByVoucherType(item.Expression), ""));
        }
        return Items;
    }

    public Item GetSalesReportItem()
    {

        string Xml = GetVoucherByVoucherType("$$VchTypeSales");
        Item item = new("Sales Vouchers", Xml, "");
        return item;
    }




    private string GetVoucherByVoucherType(string DefaultVoucherType)
    {
        var Finacialyear = GetCurrentFinacialYear();
        RequestEnvelope requestEnvelope = new(HType.Collection, "CustomVoucherCollection", new() { SVFromDate = Finacialyear.Item1, SVToDate = Finacialyear.Item2, SVCompany = "ABC Company Ltd" });
        Collection collection = new()
        {
            Name = "CustomVoucherCollection",
            Type = "Vouchers : VoucherType",
            Childof = DefaultVoucherType,
            NativeFields = new() { "*, *.*" },
            BelongsTo = true
        };
        requestEnvelope.Body.Desc.TDL.TDLMessage.Collection = new() { collection };

        return PrefixGeneratedByText(requestEnvelope.GetXML(indent: true));
    }
    private string PrefixGeneratedByText(string XML)
    {
        XML = $"<!-- \r\nGenerated Using TallyConnector - https://github.com/Accounting-Companion/TallyConnector\r\n" +
            $"Incase of any errors raise a issue here - https://github.com/Accounting-Companion/TallyXmlsGenerator\r\n" +
            $"\r\n--> \r\n{XML}";
        return XML;
    }

    public (DateTime, DateTime) GetCurrentFinacialYear()
    {
        if (FY == (DateTime.MinValue, DateTime.MinValue))
        {
            var currentDate = DateTime.Now;
            var FromDate = new DateTime(currentDate.Month < 4 ? currentDate.Year - 1 : currentDate.Year, 4, 1);
            var ToDate = new DateTime(currentDate.Month < 4 ? currentDate.Year : currentDate.Year + 1, 3, 31);
            FY = (FromDate, ToDate);
            return (FromDate, ToDate);
        }

        return FY;
    }

    public string GetLedgersXml()
    {
        return tallyHelperService.PostObjectToTallyXML<Ledger>(new("Test Ledger Name", "Sundry Debtors"));
    }
}
