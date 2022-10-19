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

    public string PostObjectToTallyXML<ObjType>(ObjType Object,
                                                PostRequestOptions? postRequestOptions = null) where ObjType : TallyXmlJson, ITallyObject
    {
        Object.PrepareForExport();
        Envelope<ObjType> Objectenvelope = new(Object,
                                               new()
                                               {
                                                   SVCompany = postRequestOptions?.Company ?? Company?.Name
                                               });
        return Objectenvelope.GetXML(postRequestOptions?.XMLAttributeOverrides);
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

    public string GetObjectsXML<ObjType>(string lookupValue,
                                         VoucherRequestOptions? requestOptions = null) where ObjType : Voucher
    {
        // If received FetchList in collectionOptions we will use that else use default fetchlist
        requestOptions ??= new();
        requestOptions.FetchList ??=
                new List<string>()
                {
                    "MasterId", "*",
                };
        string filterformulae;
        if (requestOptions.LookupField is VoucherLookupField.MasterId or VoucherLookupField.AlterId)
        {
            filterformulae = $"${requestOptions.LookupField} = {lookupValue}";
        }
        else
        {
            filterformulae = $"${requestOptions.LookupField} = \"{lookupValue}\"";
        }
        List<Filter> filters = new() { new Filter() { FilterName = "Objfilter", FilterFormulae = filterformulae } };

        PaginatedRequestOptions paginatedRequestOptions = new() { FetchList = requestOptions.FetchList, Filters = filters, Objects = requestOptions.Objects };

        return GetObjectsXML<ObjType>(paginatedRequestOptions);
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
            if (mapping.ComputeFields != null)
            {
                collectionOptions.Compute.AddRange(mapping.ComputeFields);
            }
            if (mapping.Filters != null)
            {
                collectionOptions.Filters.AddRange(mapping.Filters);
            }
            if (mapping.Objects != null)
            {
                mapping.Objects.ForEach(obj =>
                {
                    if (!collectionOptions.Objects.Contains(obj))
                    {
                        collectionOptions.Objects.Add(obj);
                    }
                });
            }
        }
        //Adding xmlelement name according to RootElement name of ReturnObject
        collectionOptions.XMLAttributeOverrides ??= new();
        XmlAttributes attrs = new();
        attrs.XmlElements.Add(new(collectionOptions.CollectionType));
        collectionOptions.XMLAttributeOverrides.Add(typeof(Colllection<ObjType>), "Objects", attrs);



        return GenerateCollectionXML(collectionOptions);

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

        return requestEnvelope.GetXML();
    }

}
