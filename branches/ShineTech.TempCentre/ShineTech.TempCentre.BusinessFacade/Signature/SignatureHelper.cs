using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace ShineTech.TempCentre.BusinessFacade
{
    /// <summary>
    /// 数字签名加密解密流
    /// </summary>
    public class SignatureHelper
    {
        private static DSACryptoServiceProvider dsa;
        private SignatureHelper()
        {
        }
        public static SignatureHelper CreateInstance()
        {
            return new SignatureHelper();
        }
        public static DataSignature CreateSignature(byte[] plainText)
        {
            DataSignature ds = new DataSignature();
            if (plainText != null)
            {
                SHA1Managed sha1 = new SHA1Managed();
                //if (dsa == null)
                dsa = new DSACryptoServiceProvider();
                DSASignatureFormatter formatter = new DSASignatureFormatter(dsa);
                byte[] data = plainText;
                byte[] hash = sha1.ComputeHash(data);
                formatter.SetHashAlgorithm("SHA1");
                byte[] signHash = formatter.CreateSignature(hash);
                ds.Data = data;
                ds.Signature = signHash;
                ds.PublicKey = dsa.ToXmlString(false);
            }
            return ds;
        }
        public static bool VerifySignature(DataSignature ds)
        {
            byte[] remoteText = ds.Data;
            byte[] signHash = ds.Signature;
            if (dsa == null)
                dsa = new DSACryptoServiceProvider();
            if (ds.PublicKey != null)
            {
                try
                {
                    dsa.FromXmlString(ds.PublicKey);
                    return dsa.VerifyData(remoteText, signHash);
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
            
        }
    }
    [Serializable]
    public class DataSignature
    {
        public DataSignature(){}
        private byte[]data;
        /// <summary>
        /// 明文
        /// </summary>
        public byte[] Data 
        {
          get { return data; }
          set { data = value; }
        }
        private byte[]signature;
        /// <summary>
        /// hash后签名散列值
        /// </summary>
        public byte[] Signature
        {
          get { return signature; }
          set { signature = value; }
        }
        private string publicKey;
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }
        private List<DAL.DigitalSignature> list;

        public List<DAL.DigitalSignature> List
        {
            get { return list; }
            set { list = value; }
        }
        private DAL.ReportEditor editor;

        /// <summary>
        /// report editor设置
        /// </summary>
        public DAL.ReportEditor Editor
        {
            get { return editor; }
            set { editor = value; }
        }
    }
}
