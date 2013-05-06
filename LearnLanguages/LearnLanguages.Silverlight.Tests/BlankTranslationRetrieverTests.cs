using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using Csla;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("translation")]
  public class BlankTranslationRetrieverTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW()
    {
      var isCreated = false;

      BlankTranslationCreator retriever = null;
      TranslationEdit blankTranslation = null;

      BlankTranslationCreator.CreateNew((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          retriever = r.Object;
          blankTranslation = retriever.Translation;
          isCreated = true;
        });

      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(retriever); },
                      () => { Assert.IsNotNull(blankTranslation); },
                      () => { Assert.IsNotNull(blankTranslation.Phrases); },
                      () => { Assert.IsTrue(blankTranslation.Phrases.Count == 2); },
                      () => { Assert.IsNotNull(blankTranslation.Phrases[0]); },
                      () => { Assert.IsNotNull(blankTranslation.Phrases[1]); },
                      () => { Assert.IsNotNull(blankTranslation.Phrases[0].Language); },
                      () => { Assert.IsNotNull(blankTranslation.Phrases[1].Language); }
                      //() => { Assert.IsNull(newError); }
                      );
      EnqueueTestComplete();
    }
  }
}