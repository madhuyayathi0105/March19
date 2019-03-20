using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

/// <summary>
/// Summary description for checkClass
/// </summary>
public class checkClass
{

    static void Main(string[] args)
    {
        test t = new test();
        String data = "test1234test1234";
        String secretkey = "fV9Nkot1ufMviCnLxfDYMz2RTfOGB3iK";
        String iv = "yazXMhDziRigqY5a";
        String encdata = t.Encrypt(data, secretkey, iv);
        Console.WriteLine("Encdata       " + encdata);
        String decdata = t.Decrypt(encdata, secretkey, iv);
        Console.WriteLine("Decdata       " + decdata);
        Console.Read();
    }
}

public class test
{
    public RijndaelManaged GetRijndaelManaged(byte[] secretKey, String iv)
    {
        //  var keyBytes = new byte[32];
        //   var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        //   Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
        return new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            KeySize = 256,
            BlockSize = 128,
            Key = secretKey,
            IV = Encoding.UTF8.GetBytes(iv)
        };
    }

    public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
    {
        return rijndaelManaged.CreateEncryptor()
            .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }

    public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
    {
        return rijndaelManaged.CreateDecryptor()
            .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
    }

    public String Encrypt(String plainText, String key, String iv)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var keybytes = test.getHashSha256(key);
        return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(keybytes, iv)));
    }


    public String Decrypt(String encryptedText, String key, String iv)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var keybytes = test.getHashSha256(key);
        return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(keybytes, iv)));
    }

    public static byte[] getHashSha256(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        return hash;
    }

    public static byte[] HashHMAC(byte[] key, byte[] message)
    {
        var hash = new HMACSHA256(key);
        return hash.ComputeHash(message);

    }
}

