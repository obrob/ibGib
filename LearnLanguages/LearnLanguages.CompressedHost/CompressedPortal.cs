using Compression;

namespace Compression
{
  public class CompressedPortal : Csla.Server.Hosts.Mobile.WcfPortal
  {
    protected override Csla.Server.Hosts.Mobile.CriteriaRequest ConvertRequest(
      Csla.Server.Hosts.Mobile.CriteriaRequest request)
    {
      var returnValue = new Csla.Server.Hosts.Mobile.CriteriaRequest();
      returnValue.ClientContext = CompressionUtility.Decompress(request.ClientContext);
      returnValue.GlobalContext = CompressionUtility.Decompress(request.GlobalContext);
      if (request.CriteriaData != null)
        returnValue.CriteriaData = CompressionUtility.Decompress(request.CriteriaData);
      returnValue.Principal = CompressionUtility.Decompress(request.Principal);
      returnValue.TypeName = request.TypeName;
      return returnValue;
    }

    protected override Csla.Server.Hosts.Mobile.UpdateRequest ConvertRequest(
      Csla.Server.Hosts.Mobile.UpdateRequest request)
    {
      var returnValue = new Csla.Server.Hosts.Mobile.UpdateRequest();
      returnValue.ClientContext = CompressionUtility.Decompress(request.ClientContext);
      returnValue.GlobalContext = CompressionUtility.Decompress(request.GlobalContext);
      returnValue.ObjectData = CompressionUtility.Decompress(request.ObjectData);
      returnValue.Principal = CompressionUtility.Decompress(request.Principal);
      return returnValue;
    }

    protected override Csla.Server.Hosts.Mobile.WcfResponse ConvertResponse(
      Csla.Server.Hosts.Mobile.WcfResponse response)
    {
      var returnValue = new Csla.Server.Hosts.Mobile.WcfResponse();
      returnValue.GlobalContext = CompressionUtility.Compress(response.GlobalContext);
      returnValue.ObjectData = CompressionUtility.Compress(response.ObjectData);
      returnValue.ErrorData = response.ErrorData;
      return returnValue;
    }
  }
}
