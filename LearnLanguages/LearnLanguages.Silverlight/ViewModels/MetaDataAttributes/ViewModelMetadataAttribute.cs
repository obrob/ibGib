using System;
using System.ComponentModel.Composition;
using LearnLanguages.Silverlight.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [MetadataAttribute]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
  public class ViewModelMetadataAttribute : ExportAttribute
  {
    public ViewModelMetadataAttribute(string coreText)
      : base(typeof(IViewModelMetadata))
    {
      CoreText = coreText;
    }

    public string CoreText { get; set; }
  }
}
