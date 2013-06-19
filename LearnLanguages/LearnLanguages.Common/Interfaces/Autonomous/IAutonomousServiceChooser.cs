using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousServiceChooser
  {
    Task Load(IList<IAutonomousService> services, IList<IAutonomousServiceContext> contexts);
  }
}
