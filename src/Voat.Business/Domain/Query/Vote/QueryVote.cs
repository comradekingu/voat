using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voat.Data;
using Voat.Domain.Models;

namespace Voat.Domain.Query
{
    public class QueryVote : CachedQuery<Domain.Models.Vote>
    {
        private int _id;
        public QueryVote(int id) : base(new Caching.CachePolicy(TimeSpan.FromDays(60)))
        {
            _id = id;
        }
        protected override string FullCacheKey => "Vote:Dictionary";
        public override string CacheKey => "Vote:Dictionary";

        public override async Task<Vote> ExecuteAsync()
        {
            var domainVote = CacheHandler.DictionaryRetrieve<int, Voat.Domain.Models.Vote>("CacheKey", _id);
            if (domainVote == null)
            {
                domainVote = await GetData();
                CacheHandler.DictionaryReplace("CacheKey", domainVote.ID, domainVote);
            }

            return domainVote;
        }

        protected override async Task<Vote> GetData()
        {
            using (var repo = new Repository(User))
            {
                return await repo.GetVote(_id);
            }
        }
    }
}
