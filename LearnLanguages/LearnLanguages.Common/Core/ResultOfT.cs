using System;
using Csla.Serialization;

namespace LearnLanguages
{
  [Serializable]
  public class Result<T>
  {

    public Result(T resultObj, bool isSuccess = true, string msg = "", params Tuple<string, object>[] infoParams)
    {
      Obj = resultObj;
      _IsSuccess = isSuccess;
      Msg = msg;

      if (infoParams != null && infoParams.Length > 0)
      {
        Info = new MagicStringsList(infoParams);
      }
    }

    public T Obj { get; set; }
    public string Msg { get; private set; }
    public MagicStringsList Info { get; set; }
    /// <summary>
    /// If this object has additional info, and that info contains an exception, this will return that exception
    /// as type "Exception".  If not, returns null.
    /// </summary>
    /// <returns></returns>
    public Exception GetExceptionFromInfo()
    {
      Exception retEx = null;
      var ex = Info[CommonResources.InfoKeyExceptionObject];
      if (ex != null)
        retEx = (Exception)ex;
      return retEx;
    }

    #region Boolean Info
    
    private bool _IsSuccess = false;
    public bool IsSuccess { get { return _IsSuccess; } }

    public bool IsError
    {
      get
      {
        //return ObjectIsException(Obj);
        //HACK: Result<T> IsError is redundant.  I don't remember why
        if (!HasAdditionalInfo)
          return !IsSuccess;

        return (Info[CommonResources.InfoKeyExceptionObject] != null);
      }
    }

    public bool HasAdditionalInfo
    {
      get
      {
        return (Info != null && Info.Members.Count > 0);
      }
    }

    #endregion

    #region Common Results

    public static Result<T> Undefined(T resultObj)
    {
      return new Result<T>(resultObj, false, CommonResources.ResultUndefined);
    }

    public static Result<T> UndefinedWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new Result<T>(resultObj, false, CommonResources.ResultUndefinedWithInfo, infos);
    }

    public static Result<T> Success(T resultObj)
    {
      return new Result<T>(resultObj, true, CommonResources.ResultSuccess);
    }

    public static Result<T> SuccessWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new Result<T>(resultObj, false, CommonResources.ResultSuccessWithInfo, infos);
    }

    public static Result<T> Failure(T resultObj)
    {
      return new Result<T>(resultObj, false, CommonResources.ResultFailure);
    }

    public static Result<T> FailureWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new Result<T>(resultObj, false, CommonResources.ResultFailureWithInfo, infos);
    }

    public static Result<T> FailureWithInfo(T resultObj, Exception ex)
    {
      Tuple<string, object> infoExObject =
            new Tuple<string, object>() { Item1 = CommonResources.InfoKeyExceptionObject, Item2 = ex };
      Result<T> retResult = new Result<T>(resultObj, false, ex.Message, infoExObject);
      return retResult;
    }

    
    #endregion
  }
}
