using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voat.Business.Tests.Infrastructure;
using Voat.Common;
using Voat.Domain;
using Voat.Domain.Models;
using Voat.Voting.Outcomes;
using Voat.Voting.Restrictions;

namespace Voat.Business.Tests.Voting
{
    [TestClass]
    public class MappingTests : BaseUnitTest
    {
        [TestMethod]
        public void DomainMapTest()
        {

            var vote = new CreateVote();
            vote.Title = "Title Here";
            vote.Content = "Content Here";
            vote.Subverse = SUBVERSES.Unit;

            var outcome = new AddModeratorOutcome() { UserName = USERNAMES.Unit, Subverse = SUBVERSES.Unit, Level = ModeratorLevel.Owner };

            vote.Options.Add(new CreateVote.CreateVoteOption() { Title = "Vote Option 1", Content = "Vote Content Option 1",
                Outcomes = new List<CreateVote.CreateVoteType>() {
                    new CreateVote.CreateVoteType(){ TypeName = outcome.GetType().Name, Options = outcome.ToJson() }
                }
            });

            var outcome2 = new RemoveModeratorOutcome() { UserName = USERNAMES.Unit, Subverse = SUBVERSES.Unit };

            vote.Options.Add(new CreateVote.CreateVoteOption()
            {
                Title = "Vote Option 2",
                Content = "Vote Content Option 2",
                Outcomes = new List<CreateVote.CreateVoteType>() {
                    new CreateVote.CreateVoteType(){ TypeName = outcome2.GetType().Name, Options = outcome2.ToJson() }
                }
            });

            var restriction = new ContributionPointRestriction() { ContentType = ContentTypeRestriction.Any, Duration = TimeSpan.FromDays(90), Group = "Group 1", MinimumCount = 10, MaximumCount = 100, EndDate = DateTime.UtcNow };

            vote.Restrictions.Add(
                new CreateVote.CreateVoteType() {
                    TypeName = restriction.GetType().Name, Options = restriction.ToJson()   
                });

            var domainVote = vote.Map();

            Assert.IsNotNull(domainVote);
            Assert.AreEqual(vote.Title, domainVote.Title);
            Assert.AreEqual(vote.Content, domainVote.Content);
            Assert.AreEqual(vote.Options.Count, domainVote.Options.Count);

            vote.Options.ForEachIndex((x,i) => {
                Assert.AreEqual(x.Title, domainVote.Options[i].Title);
                Assert.AreEqual(x.Content, domainVote.Options[i].Content);
                x.Outcomes.ForEachIndex((o, i2) => {
                    Assert.AreEqual(o.TypeName, domainVote.Options[i].Outcomes[i2].GetType().Name);

                    domainVote.Options[i].Outcomes[i2].AssertObjectEqualsJson(o.Options.ToString(), null, "ID");

                });
            });

            vote.Restrictions.ForEachIndex((x, i) => {
                Assert.AreEqual(x.TypeName, domainVote.Restrictions[i].GetType().Name);
                domainVote.Restrictions[i].AssertObjectEqualsJson(x.Options.ToString(), null, "ID", "DateRange");
                //Assert.AreEqual(x.Content, domainVote.Options[i].Content);
            });

            

        }
       
    }
}
