using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study.Interfaces
{
  public interface IGetStudyData
  {
    void AskUserExtraData(object criteria, AsyncCallback<StudyDataEdit> callback);
  }
}
