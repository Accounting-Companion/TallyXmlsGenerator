using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallyConnector.Core.Converters.XMLConverterHelpers;
using TallyConnector.Core.Models;
using TallyXmlsGenerator.Models;

namespace TallyXmlsGenerator.Services;
public class XmlGenerator
{
    private TallyHelperService tallyHelperService;


    public XmlGenerator()
    {
        tallyHelperService = new TallyHelperService();
    }

    public void GenerateXmlsOnly()
    {
        string salesreportXml = GetSalesReportXML();
    }

    public void GeneratePostManCollectionJson()
    {
        PostManCollection postManCollection = new("9530b1ce-7099-4886-b0d9-5cf7970c1151",
                                                  "TallyXML",
                                                  "When Developing [Tally Connector Library](https://github.com/Accounting-Companion/TallyConnector) , I did some research to get" +
                                                  " data from Tally in easier way using XML API and " +
                                                  "found these xmls useful\n\nFollowing are the Xml Requests To " +
                                                  "Communicate with Tally Prime/ERP 9\n\nIf you want to suggest any changes " +
                                                  "or found any issues contact me\n\nIf you want " +
                                                  "complete integration support contact me at [contact@saivineeth.com]" +
                                                  "(mailto:contact@saivineeth.com)",
                                                  "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
                                                  "13855108");

        postManCollection.Variable = new()
        {
            new("TallyPort", "9000"),
            new("TallyURL", "http://localhost")
        };


    }
    public void GetSalesReportItem()
    {
        string Xml = GetSalesReportXML();
    }

    private string GetSalesReportXML()
    {
        RequestEnvelope requestEnvelope = new(HType.Collection, "CustomVoucherCollection", new() { SVFromDate = DateTime.Now.AddMonths(-1), SVToDate = DateTime.Now, SVCompany = "ABC Company" });
        Collection collection = new()
        {
            Name = "CustomVoucherCollection",
            Type = "Vouchers : VoucherType",
            Childof = "$$VchTypeSales",
            NativeFields = new() { "*, *.*" },
            BelongsTo = true
        };
        requestEnvelope.Body.Desc.TDL.TDLMessage.Collection = new() { collection };
        return PrefixGeneratedByText(requestEnvelope.GetXML(indent: true));

    }

    private string PrefixGeneratedByText(string XML)
    {
        XML = $"<!-- \r\nGenerated Using TallyConnector - https://github.com/Accounting-Companion/TallyConnector\r\n" +
            $"Incase of any Errors raise a issue here - " +
            $"\r\n--> \r\n{XML}";
        return XML;
    }

}
