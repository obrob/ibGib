using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ParentPhraseSpawnedChildPhraseEvent : Bases.ParentPhraseChildPhraseEventBase
  {
    public ParentPhraseSpawnedChildPhraseEvent(PhraseEdit parent, PhraseEdit child)
      : base(parent, child)
    {
    }

    public static void Pub(PhraseEdit parent, PhraseEdit child)
    {
      HistoryPublisher.Ton.PublishEvent(new ParentPhraseSpawnedChildPhraseEvent(parent, child));
    }
  }
}
