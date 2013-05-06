using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LearnLanguages.DataAccess.Mock
{
  public class PhraseBeliefDal : PhraseBeliefDalBase
  {
    protected override PhraseBeliefDto NewImpl(object criteria)
    {
      //to get to this point, we must have already been authenticated
      Debug.Assert(Csla.ApplicationContext.User.Identity.IsAuthenticated);

      if (criteria != null)
        throw new ArgumentException("criteria expected to be null");
      
      var username = Csla.ApplicationContext.User.Identity.Name;
      var userId = (from u in SeedData.Ton.Users
                    where u.Username == username
                    select u.Id).FirstOrDefault();
      if (userId == Guid.Empty)
        throw new Exceptions.UserNotAuthorizedException();


      var dto = new PhraseBeliefDto()
      {
        Id = Guid.NewGuid(),
        Text = "",
        Strength = 1.0d,
        PhraseId = Guid.Empty,
        UserId = userId,
        Username = username
      };

      return dto;
    }
    protected override PhraseBeliefDto FetchImpl(Guid id)
    {
      var results = from item in SeedData.Ton.PhraseBeliefs
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
        return results.First();
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<PhraseBeliefDto> FetchAllRelatedToPhraseImpl(Guid phraseId)
    {
      if (phraseId == Guid.Empty)
        throw new ArgumentException("phraseId");

      var results = from item in SeedData.Ton.PhraseBeliefs
                    where item.PhraseId == phraseId
                    select item;

      var retPhraseBeliefs = new List<PhraseBeliefDto>(results);

      //WE ARE NOT WORRIED IF THERE ARE ZERO BELIEFS FOUND ABOUT PHRASE ID, SO NO EXCEPTION
      //THROWN IF COUNT == 0
      return retPhraseBeliefs;
    }
    protected override PhraseBeliefDto UpdateImpl(PhraseBeliefDto dto)
    {
      var results = from item in SeedData.Ton.PhraseBeliefs
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 1)
      {
        CheckContraints(dto);

        var phraseBeliefToUpdate = results.First();
        SeedData.Ton.PhraseBeliefs.Remove(phraseBeliefToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.PhraseBeliefs.Add(dto);
        UpdateReferences(phraseBeliefToUpdate, dto);
        return dto;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(dto.Id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override PhraseBeliefDto InsertImpl(PhraseBeliefDto dto)
    {
      //MAKE SURE ID ISN'T ALREADY IN DB
      var results = from item in SeedData.Ton.PhraseBeliefs
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 0)
      {
        CheckContraints(dto);

        dto.Id = Guid.NewGuid();
        SeedData.Ton.PhraseBeliefs.Add(dto);

        //ADD PHRASEBELIEF.ID TO USER
        var resultsUser = from u in SeedData.Ton.Users
                          where u.Id == dto.UserId
                          select u;

        var user = resultsUser.First();
        user.PhraseBeliefIds.Add(dto.Id);

        return dto;
      }
      else
      {
        if (results.Count() == 1) //ID ALREADY EXISTS
          throw new Exceptions.IdAlreadyExistsException(dto.Id);
        else                      //MULTIPLE IDS ALREADY EXIST??
          throw new Exceptions.VeryBadException();
      }
    }
    protected override PhraseBeliefDto DeleteImpl(Guid id)
    {
      var results = from item in SeedData.Ton.PhraseBeliefs
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
      {
        var phraseBeliefToRemove = results.First();
        SeedData.Ton.PhraseBeliefs.Remove(phraseBeliefToRemove);
        return phraseBeliefToRemove;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<PhraseBeliefDto> GetAllImpl()
    {
      var allDtos = new List<PhraseBeliefDto>(SeedData.Ton.PhraseBeliefs);
      return allDtos;
    }

    private void CheckContraints(PhraseBeliefDto dto)
    {
      //REFERENTIAL INTEGRITY
      if (dto.PhraseId == Guid.Empty || !SeedData.Ton.ContainsPhraseId(dto.PhraseId))
        throw new Exceptions.IdNotFoundException(dto.PhraseId);
      if (dto.UserId == Guid.Empty || !SeedData.Ton.ContainsUserId(dto.UserId))
        throw new Exceptions.IdNotFoundException(dto.UserId);
      if (string.IsNullOrEmpty(dto.Username) ||
         !(SeedData.Ton.GetUsername(dto.UserId) == dto.Username))
        throw new ArgumentException("dto.Username");
    }
    private void UpdateReferences(PhraseBeliefDto oldPhraseBelief, PhraseBeliefDto newPhraseBelief)
    {
      ////UPDATE USERS WHO REFERENCE THIS LINE
      var referencedUsers = from u in SeedData.Ton.Users
                            where u.PhraseBeliefIds.Contains(oldPhraseBelief.Id)
                            select u;

      foreach (var user in referencedUsers)
      {
        user.PhraseBeliefIds.Remove(oldPhraseBelief.Id);
        user.PhraseBeliefIds.Add(newPhraseBelief.Id);
      }

      ////UPDATE TRANSLATIONS WHO REFERENCE THIS PHRASE
      //var referencedTranslations = from t in SeedData.Ton.Translations
      //                             where t.PhraseBeliefIds.Contains(oldPhraseBelief.Id)
      //                             select t;

      //foreach (var translation in referencedTranslations)
      //{
      //  translation.PhraseBeliefIds.Remove(oldPhraseBelief.Id);
      //  translation.PhraseBeliefIds.Add(newPhraseBelief.Id);
      //}
    }
  }
}
