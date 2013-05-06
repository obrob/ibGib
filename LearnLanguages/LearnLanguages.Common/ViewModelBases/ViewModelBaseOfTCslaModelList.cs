using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Specialized;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Common.ViewModels
{
  public abstract class ViewModelBase<TCslaModelList, TCslaModel, TCslaModelDto> : ViewModelBase
    where TCslaModelList : Common.CslaBases.BusinessListBase<TCslaModelList, TCslaModel, TCslaModelDto>
    where TCslaModel : Common.CslaBases.BusinessBase<TCslaModel, TCslaModelDto>
    where TCslaModelDto : class
  {
    private TCslaModelList _ModelList;
    public TCslaModelList ModelList
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          NotifyOfPropertyChange(() => ModelList);
        }
      }
    }

    protected virtual void HookInto(TCslaModelList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(TCslaModelList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanCancelEdit);
    }

    public bool CanSave
    {
      get
      {
        return (ModelList != null && ModelList.IsSavable);
      }
    }
    public virtual void Save()
    {
      ModelList.BeginSave((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          ModelList = (TCslaModelList)r.NewObject;
          NotifyOfPropertyChange(() => CanSave);
        });
    }

    public bool CanCancelEdit
    {
      get
      {
        return (ModelList != null && ModelList.IsDirty);
      }
    }
    public virtual void CancelEdit()
    {
      ModelList.CancelEdit();
      NotifyOfPropertyChange(() => CanCancelEdit);
      NotifyOfPropertyChange(() => CanSave);
    }


  }
}
