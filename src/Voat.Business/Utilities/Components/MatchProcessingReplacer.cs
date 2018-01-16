using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Voat.Common;

namespace Voat.Utilities.Components
{

    public class MatchProcessingReplacer : IReplacer
    {
        private Func<Match, string, object, string> _replacementFunc = null;

        public MatchProcessingReplacer(string regEx, Func<Match, string, object, string> replacementFunc)
        {
            this.RegEx = regEx;
            this._replacementFunc = replacementFunc;
        }

        public bool IgnoreDuplicateMatches { get; set; } = false;

        public int MatchThreshold { get; set; } = 0;

        public string RegEx { get; set; } = "";

        public List<string> EscapeBlocks { get; set; } = new List<string>() { "~~~", "`" };

        public bool IsInMarkDownAnchor(Match m, string content)
        {
            var markdownAnchors = Regex.Matches(content, @"\[.*?\]\(.+?\)");
            foreach (Match anchor in markdownAnchors)
            {
                if (m.Index > anchor.Index && m.Index < (anchor.Index + anchor.Length))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasAnyTokens(string content, IEnumerable<string> blockTokens)
        {
            foreach (string blockToken in blockTokens)
            {
                if (content.Contains(blockToken))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInBlock(Match m, string content, IEnumerable<string> blockTokens)
        {
            foreach (string blockToken in blockTokens)
            {
                //we have blocks in comment
                int blockIndex = content.IndexOf(blockToken); //find first block start

                //determine if match is in block
                if (blockIndex >= 0)
                {
                    if (m.Index < blockIndex)
                    {
                        //match is before the first block, we continue processing the match
                        return false;
                    }
                    else
                    {
                        int start = blockIndex;
                        while (start >= 0)
                        {
                            int end = content.IndexOf(blockToken, start + 1);
                            if (end >= 0)
                            {
                                if (m.Index > start && m.Index < end)
                                {
                                    return true;
                                }
                                else if (m.Index < start)
                                {
                                    return false;
                                }
                                start = content.IndexOf(blockToken, end + 1);
                            }
                            else
                            {
                                //open block with no end, we bail on this catastrophe of a formatting nightmare
                                break;
                            }
                        }
                    }
                }
            }
            return false;
        }
       
        public virtual string Replace(string content, object state)
        {
            string result = content;

            if (result == null)
            {
                return result;
            }

            var matchmaker = new MatchMaker() { MatchThreshold = MatchThreshold, IgnoreDuplicateMatches = IgnoreDuplicateMatches };
            if (matchmaker.Process(content, RegEx))
            {
                
                int offset = 0;

                //flag content as having ignored areas if it has more than 1 match
                bool requiresAdditionalProcecessing = (matchmaker.Matches.Count() > 0) ? HasAnyTokens(content, EscapeBlocks) : false;

                foreach (Match m in matchmaker.FilteredMatches)
                {
                    //make sure this match isn't in a block
                    if (!requiresAdditionalProcecessing || (requiresAdditionalProcecessing && !IsInBlock(m, content, EscapeBlocks)))
                    {
                        //make sure this match isn't in an anchor
                        if (!IsInMarkDownAnchor(m, content))
                        {
                            //get the replacement value for match
                            string substitution = _replacementFunc(m, content, state);

                            //Concat method (fractions of milliseconds faster)
                            result = String.Concat(result.Substring(0, m.Index + offset), substitution, result.Substring(m.Index + m.Length + offset, result.Length - (m.Length + m.Index + offset)));

                            offset += substitution.Length - m.Length;
                        }
                    }
                }
            }
            return result;
        }
    }
}
