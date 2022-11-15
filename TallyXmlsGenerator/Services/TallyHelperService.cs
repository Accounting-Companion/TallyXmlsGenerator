using System.Xml.Serialization;
using TallyConnector.Core.Models;
using TallyConnector.Services;

namespace TallyXmlsGenerator.Services;
public class TallyHelperService : TallyService
{
    public string GetLicenseInfoXml()
    {
        return GetTDLReportXML<LicenseInfo, LicenseInfo>();
    }


    public string GetActiveCompanyXml()
    {
        PaginatedRequestOptions? paginatedRequestOptions = new()
        {
            FetchList = new() { "NAME", "GUID", "BOOKSFROM", "STARTINGFROM", "COMPANYNUMBER", "ENDINGAT" },
            Filters = new() { new("ActiveCompFilt", "$Name = ##SVCURRENTCOMPANY") },
            IsInitialize = YesNo.Yes,
        };
        return GetObjectsXML<BaseCompany>(paginatedRequestOptions);

    }

    public string PostObjectToTallyXML<ObjType>(ObjType Object,
                                                PostRequestOptions? postRequestOptions = null) where ObjType : TallyXmlJson, ITallyObject
    {
        Object.PrepareForExport();
        Envelope<ObjType> Objectenvelope = new(Object,
                                               new()
                                               {
                                                   SVCompany = postRequestOptions?.Company ?? Company?.Name
                                               });
        return Objectenvelope.GetXML(postRequestOptions?.XMLAttributeOverrides, true);
    }
    public string PostVoucherToTallyXML<ObjType>(ObjType Object,
                                                PostRequestOptions? postRequestOptions = null) where ObjType : Voucher
    {
        Object.PrepareForExport();
        postRequestOptions ??= new();
        postRequestOptions.XMLAttributeOverrides ??= new();

        if (Object.View != VoucherViewType.AccountingVoucherView)
        {
            XmlAttributes xmlattribute = new();
            xmlattribute.XmlElements.Add(new("LEDGERENTRIES.LIST"));
            postRequestOptions.XMLAttributeOverrides.Add(typeof(Voucher), "Ledgers", xmlattribute);
        }

        Envelope<ObjType> Objectenvelope = new(Object,
                                               new()
                                               {
                                                   SVCompany = postRequestOptions?.Company ?? Company?.Name
                                               });
        return  Objectenvelope.GetXML(postRequestOptions?.XMLAttributeOverrides, true);
    }
    public string GetObjectXML<ObjType>(string lookupValue,
                                        MasterRequestOptions? requestOptions = null) where ObjType : TallyBaseObject, INamedTallyObject
    {
        // If received FetchList in collectionOptions we will use that else use default fetchlist
        requestOptions ??= new();
        requestOptions.FetchList ??= new() { "MasterId", "CanDelete", "*" };
        string filterformulae;
        if (requestOptions.LookupField is MasterLookupField.MasterId or MasterLookupField.AlterId)
        {
            filterformulae = $"${requestOptions.LookupField} = {lookupValue}";
        }
        else
        {
            filterformulae = $"${requestOptions.LookupField} = \"{lookupValue}\"";
        }
        List<Filter> filters = new() { new Filter() { FilterName = "Objfilter", FilterFormulae = filterformulae } };

        CollectionRequestOptions collectionRequestOptions = new() { FetchList = requestOptions.FetchList, Filters = filters };

        return GetObjectsXML<ObjType>(collectionRequestOptions);


    }




    public string GetObjectsXML<ObjType>(PaginatedRequestOptions? objectOptions = null) where ObjType : TallyBaseObject
    {
        //Gets Root attribute of ReturnObject
        string? RootElemet = AttributeHelper.GetXmlRootElement(typeof(ObjType));

        CollectionRequestOptions collectionOptions = new()
        {
            CollectionType = RootElemet ?? typeof(ObjType).Name,
            FromDate = objectOptions?.FromDate,
            ToDate = objectOptions?.ToDate,
            FetchList = (objectOptions?.FetchList) != null ? new(objectOptions.FetchList) : null,
            Filters = (objectOptions?.Filters) != null ? new(objectOptions.Filters) : null,
            Compute = (objectOptions?.Compute) != null ? new(objectOptions.Compute) : null,
            ComputeVar = (objectOptions?.ComputeVar) != null ? new(objectOptions.ComputeVar) : null,
            Pagination = objectOptions?.Pagination,
            Objects = objectOptions?.Objects,
            XMLAttributeOverrides = objectOptions?.XMLAttributeOverrides,
            IsInitialize = objectOptions?.IsInitialize ?? YesNo.No,
        };
        var mapping = Mappings.TallyObjectMappings
                .FirstOrDefault(map => map.TallyMasterType.Equals(collectionOptions.CollectionType, StringComparison.OrdinalIgnoreCase));
        collectionOptions.Compute ??= new();
        collectionOptions.Filters ??= new();
        collectionOptions.Objects ??= new();
        if (mapping != null)
        {

            if (mapping.Filters != null)
            {
                collectionOptions.Filters.AddRange(mapping.Filters);
            }

        }
        //Adding xmlelement name according to RootElement name of ReturnObject
        collectionOptions.XMLAttributeOverrides ??= new();
        XmlAttributes attrs = new();
        attrs.XmlElements.Add(new(collectionOptions.CollectionType));
        collectionOptions.XMLAttributeOverrides.Add(typeof(Colllection<ObjType>), "Objects", attrs);



        return GenerateCollectionXML(collectionOptions, true);

    }



    public string GetTDLReportXML<ReportType, ReturnType>(DateFilterRequestOptions? requestOptions = null)
    {
        StaticVariables sv = new()
        {
            SVCompany = requestOptions?.Company ?? Company?.Name,
            SVExportFormat = "XML",
            SVFromDate = requestOptions?.FromDate,
            SVToDate = requestOptions?.ToDate
        };
        TDLReport report = TDLReportHelper.CreateTDLReport(typeof(ReportType));

        RequestEnvelope requestEnvelope = new(report, sv);

        return requestEnvelope.GetXML(indent: true);
    }

}
