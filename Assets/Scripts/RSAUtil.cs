using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class RSAUtil : MonoBehaviour
{
    public static int MAX_BLOCK_LENGTH = 122;

    private static RSAParameters XmlToRsa(string xml)
    {
        //get a stream from the string
        var sr = new System.IO.StringReader(xml);
        //we need a deserializer
        var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
        //get the object back from the stream

        var result = xs.Deserialize(sr);

        if (result == null)
        {
            throw new InvalidOperationException("Problem when encrypting file. Key is not valid!");
        }

        return (RSAParameters)result;
    }

    public static void GenerateKeys(out string pubKeyXml, out string privKeyXml)
    {
        // lets take a new CSP with a new 2048 bit rsa key pair
        var csp = new RSACryptoServiceProvider(2048);

        // get the private key
        var privKey = csp.ExportParameters(true);
        {
            // we need some buffer
            var sw = new System.IO.StringWriter();
            // we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            // serialize the key into the stream
            xs.Serialize(sw, privKey);
            // get the string from the stream
            privKeyXml = sw.ToString();
        }

        //and the public key ...
        var pubKey = csp.ExportParameters(false);
        {
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, pubKey);
            //get the string from the stream
            pubKeyXml = sw.ToString();
        }
    }

    private static string BaseEncrypt(RSAParameters pubKey, string msg)
    {
        // we have a public key ... let's get a new csp and load that key
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(pubKey);

        //for encryption, always handle bytes...
        var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(msg);

        //apply pkcs#1.5 padding and encrypt our data 
        var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

        //we might want a string representation of our cypher text... base64 will do
        var result = Convert.ToBase64String(bytesCypherText);

        return result;
    }

    public static string Encrypt(string pubKeyXml, string msg)
    {
        // convert it from xml text to RSAParameters
        RSAParameters pubKey = XmlToRsa(pubKeyXml);

        var numBlocks = (int)Math.Ceiling(msg.Length / (double)MAX_BLOCK_LENGTH);

        var result = numBlocks.ToString() + ";";
        for (var i = 0; i < numBlocks; i++)
        {
            result += BaseEncrypt(pubKey, msg.Substring(i * MAX_BLOCK_LENGTH, Math.Min(MAX_BLOCK_LENGTH, msg.Length - i * MAX_BLOCK_LENGTH)));
        }

        return result;
    }

    private static string BaseDecrypt(RSAParameters privKey, string cryptMsg)
    {
        //we want to decrypt, therefore we need a csp and load our private key
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(privKey);

        //first, get our bytes back from the base64 string ...
        var bytesCypherText = Convert.FromBase64String(cryptMsg);

        //decrypt and strip pkcs#1.5 padding
        var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

        //get our original plainText back...
        var result = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

        return result;
    }

    public static string Decrypt(string privKeyXml, string cryptMsg)
    {
        // convert it from xml text to RSAParameters
        RSAParameters privKey = XmlToRsa(privKeyXml);

        var index = cryptMsg.IndexOf(";");
        var numBlocksStr = cryptMsg.Substring(0, index);
        int numBlocks;
        if (!Int32.TryParse(numBlocksStr, out numBlocks))
        {
            throw new InvalidOperationException("Crypt message doesn't have the number of blocks");
        }
        index++;
        var countBlocks = 0;

        var result = "";
        var nextIndex = cryptMsg.IndexOf("==", index);
        while (nextIndex != -1)
        {
            try
            {
                result += BaseDecrypt(privKey, cryptMsg.Substring(index, nextIndex - index + 2));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            index = nextIndex + 2;
            nextIndex = cryptMsg.IndexOf("==", index);
            countBlocks++;
        }

        if (countBlocks != numBlocks)
        {
            throw new InvalidOperationException("Number of blocks doesn't match");
        }

        return result;
    }
}
