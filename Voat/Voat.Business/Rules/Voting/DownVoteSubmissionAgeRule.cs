#region LICENSE

/*
    
    Copyright(c) Voat, Inc.

    This file is part of Voat.

    This source file is subject to version 3 of the GPL license,
    that is bundled with this package in the file LICENSE, and is
    available online at http://www.gnu.org/licenses/gpl-3.0.txt;
    you may not use this file except in compliance with the License.

    Software distributed under the License is distributed on an
    "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express
    or implied. See the License for the specific language governing
    rights and limitations under the License.

    All Rights Reserved.

*/

#endregion LICENSE

using System;
using Voat.Data;
using Voat.Domain.Query;
using Voat.RulesEngine;

namespace Voat.Rules.Voting
{
    [RuleDiscovery("Approved if comment is not older than 7 days", "approved = (comment.Age <= 7 days)")]
    public class DownVoteSubmissionAgeRule : VoatRule
    {
        public DownVoteSubmissionAgeRule() : base("Downvote Submission Age", "2.6", RuleScope.DownVoteSubmission)
        {
        }

        protected override RuleOutcome EvaluateRule(VoatRuleContext context)
        {
            var q = new QuerySubmission(context.SubmissionID.Value);
            var submission = q.Execute();

            // do not execute downvoting if comment is older than 7 days
            var commentPostingDate = submission.CreationDate;
            TimeSpan timeElapsed = Repository.CurrentDate - commentPostingDate;
            if (timeElapsed.TotalDays > 7)
            {
                return CreateOutcome(RuleResult.Denied, "Submission downvotes not registered after 7 days");
            }
            return base.EvaluateRule(context);
        }
    }
}
