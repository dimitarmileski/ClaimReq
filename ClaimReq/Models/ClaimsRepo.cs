using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;

namespace ClaimReq.Models
{
    // tag::class[]
    public class ClaimsRepo
    {
        private readonly IBucket _bucket;

        public ClaimsRepo(IBucket bucket)
        {
            _bucket = bucket;
        }

        public Dictionary<string, Claim> GetAll()
        {
            var request = QueryRequest.Create(@"
                SELECT
                    META().id AS `key`,
                    TOOBJECT(sb) as `value`
                FROM `starterbucket` as sb
                WHERE type='Claim';");
            request.ScanConsistency(ScanConsistency.RequestPlus);
            var response = _bucket.Query<KeyValuePair<string, Claim>>(request);
            return response.Rows.ToDictionary(x => x.Key, x => x.Value);
        }

        public KeyValuePair<string, Claim> GetClaimByKey(string key)
        {
            var claim = _bucket.Get<Claim>(key).Value;
            return new KeyValuePair<string, Claim>(key, claim);
        }

        public void Save(KeyValuePair<string, Claim> model)
        {
            var doc = new Document<Claim>
            {
                Id = model.Key,
                Content = model.Value
            };
            _bucket.Upsert(doc);
        }

        public void Delete(string key)
        {
            _bucket.Remove(key);
        }
    }
    // end::class[]
}
