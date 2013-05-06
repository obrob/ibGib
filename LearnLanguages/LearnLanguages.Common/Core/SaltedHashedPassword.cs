using System;
using System.Security.Cryptography;
using System.Text;
using Csla;
using Csla.Serialization;
using Csla.Core;

/* Credits:
 * I learned most of this through "http://www.aspheute.com/english/20040105.asp".
 * Written by: Christoph Wille
 * Translated by: Bernhard Spuida
 * First published: 1/5/2004 
 */

namespace LearnLanguages.Common
{
  /// <summary>
  /// This class is used to both encode/confirm passwords. So, when you are storing a user's
  /// password info, you create one of these objects with the password, and salt. This
  /// then creates the saltedhashed password. You then store the salt and SaltedHashedPassword string
  /// in DB.
  /// 
  /// Then, create one of these when confirming password. First, get the user's salt from db.
  /// Then create this object with that salt and the possible password.
  /// Then compare the output of this salted hashed password string to the 
  /// salted hashed password string in the DB.
  /// </summary>
  [Serializable]
  public class SaltedHashedPassword : MobileObject
  {
    #region Ctors 
    /// <summary>
    /// This default ctor is for the data portal.
    /// </summary>
    public SaltedHashedPassword()
    {

    }

    /// <summary>
    /// Creates a SaltedHashedPassword object with a randomly generated salt, 
    /// using the given clearUnsaltedPassword.
    /// </summary>
    /// <param name="clearUnsaltedPassword"></param>
    public SaltedHashedPassword(string clearUnsaltedPassword)
      : this(clearUnsaltedPassword, SaltHelper.GenerateRandomSalt())
    {

    }

    public SaltedHashedPassword(string clearUnsaltedPassword, int salt)
    {
      Value = SaltHelper.ComputeHash(clearUnsaltedPassword, salt);
      Salt = salt;      
    }

    #endregion

    public static string GetHashedPasswordValue(string password, int salt)
    {
      return SaltHelper.ComputeHash(password, salt);
    }

    public int Salt { get; private set; }
    public string Value { get; private set; }
    
    
    //#region Equals, ==, and != Overrides
    //public override bool Equals(object obj)
    //{
    //  if (!(obj is SaltedHashedPassword))
    //    return false;

    //  var other = (SaltedHashedPassword)obj;
    //  return (other.Value == this.Value && other.Salt == this.Salt);
    //}
    //public static bool operator ==(SaltedHashedPassword a, SaltedHashedPassword b)
    //{
    //  // If both are null, or both are same instance, return true.
    //  if (System.Object.ReferenceEquals(a, b))
    //  {
    //    return true;
    //  }

    //  // If one is null, but not both, return false.
    //  if (((object)a == null) || ((object)b == null))
    //  {
    //    return false;
    //  }

    //  // Return true if the fields match:
    //  return (a.Value == b.Value && a.Salt == b.Salt);
    //}
    //public static bool operator !=(SaltedHashedPassword a, SaltedHashedPassword b)
    //{
    //  return !(a == b);
    //}
    //public override int GetHashCode()
    //{
    //  int hash = 13;
    //  hash = (hash * 7) + Salt.GetHashCode();
    //  hash = (hash * 7) + Value.GetHashCode();
    //  return hash;
    //}
    //#endregion
  }

  public static class SaltHelper
  {
    public static string GenerateRandomPassword(int passwordLength)
    {
      string allowedChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_<>.?[]{}`~/\";
      byte[] randomBytes = new byte[passwordLength];
      RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
      random.GetBytes(randomBytes); //fills randomBytes array with random bytes
      char[] chars = new char[passwordLength];
      int allowedCharCount = allowedChars.Length;
      for (int i = 0; i < passwordLength; i++)
      {
        chars[i] = allowedChars[((int)randomBytes[i] % allowedCharCount)];
      }

      return new string(chars);
    }
    public static int GenerateRandomSalt()
    {
      byte[] saltBytes = new byte[4]; //4-byte salt
      RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
      random.GetBytes(saltBytes); //fills saltBytes array with random bytes

      return ((((int)saltBytes[0]) << 24) + (((int)saltBytes[1]) << 16) +
        (((int)saltBytes[2]) << 8) + ((int)saltBytes[3]));
    }
    public static string ComputeHash(string stringToHash, int salt)
    {
      //byte array of password
      UnicodeEncoding encoder = new UnicodeEncoding();
      byte[] secretBytes = encoder.GetBytes(stringToHash);

      //new salt
      byte[] saltBytes = new byte[4]; //4-byte hash
      saltBytes[0] = (byte)(salt >> 24);
      saltBytes[1] = (byte)(salt >> 16);
      saltBytes[2] = (byte)(salt >> 8);
      saltBytes[3] = (byte)(salt);

      //concatenate the two arrays
      byte[] hashBytes = new byte[secretBytes.Length + saltBytes.Length];
      Array.Copy(secretBytes, 0, hashBytes, 0, secretBytes.Length);
      Array.Copy(saltBytes, 0, hashBytes, secretBytes.Length, saltBytes.Length);

      SHA1Managed sha1 = new SHA1Managed();
      byte[] computedHash = sha1.ComputeHash(hashBytes);
      var retStr = encoder.GetString(computedHash, 0, computedHash.Length);

      return retStr;
    }

  }
}
