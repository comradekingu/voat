using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voat.Caching;
using Voat.Common;
using Voat.Data;
using Voat.Domain.Models;

namespace Voat.Domain.Query
{
    public class QueryUserVotes : Query<IEnumerable<Domain.Models.Vote>>
    {
        private string _userName;
        private SearchOptions _options;

        //protected override string FullCacheKey => CachingKey.UserVotes(_userName, _options);

        //public override string CacheKey => throw new NotImplementedException();


        public QueryUserVotes(string userName, SearchOptions options)
        {
            _userName = userName;
            _options = options;
        }

        public override async Task<IEnumerable<Vote>> ExecuteAsync()
        {
            var ids = await CacheHandler.Instance.RegisterAsync<IEnumerable<int>>(CachingKey.UserVotes(_userName, _options), GetData, TimeSpan.FromMinutes(60));

            var votes = new List<Voat.Domain.Models.Vote>();

            foreach (int id in ids)
            {
                var q = new QueryVote(id);

                votes.Add(await q.ExecuteAsync());

            }

            return votes;

        }

        protected async Task<IEnumerable<int>> GetData()
        {
            using (var repo = new Repository(User))
            {
                var result = await repo.GetUserVotes(_userName, _options);
                return result;
            }
        }
    }
}
