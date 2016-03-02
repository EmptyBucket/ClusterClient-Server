using System;
using System.Security.Cryptography;
using System.Text;

namespace ClusterServer.ResponseBuilder
{
    public class HashResponseBuilder : IResponseBuilder
    {
        private readonly Encoding _encoding;
        private readonly byte[] _key;

        public HashResponseBuilder(Encoding encoding, byte[] key)
        {
            _encoding = encoding;
            _key = key;
        }

        public byte[] Build(string content) => GetBase64HashBytes(content);

        private byte[] GetBase64HashBytes(string query)
        {
            using (var hasher = new HMACMD5(_key))
            {
                var hash = Convert.ToBase64String(hasher.ComputeHash(_encoding.GetBytes(query)));
                return _encoding.GetBytes(hash);
            }
        }
    }
}