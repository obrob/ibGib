using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeOtherLibrary
{
  [Serializable]
  public class MyOwnAwesomeClass
  {
    public MyOwnAwesomeClass(string privateField, string publicField, string publicState, string privateState)
    {
      _privateField = privateField;
      PublicField = publicField;
      PublicState = publicState;
      _PrivateState = privateState;
    }

    private string _privateField;
    public string PublicField;
    public string PublicState { get; set; }
    private string _PrivateState { get; set; }
  }
}
