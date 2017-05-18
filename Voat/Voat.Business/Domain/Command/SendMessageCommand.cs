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

using System.Threading.Tasks;
using Voat.Data;
using Voat.Domain.Models;

namespace Voat.Domain.Command
{
    public class SendMessageCommand : Command<CommandResponse<Message>>
    {
        private SendMessage _message;
        private bool _forceSend;
        private bool _ensureUserExists;
        private bool _isAnonymized;

        public SendMessageCommand(SendMessage message, bool forceSend = false, bool ensureUserExists = false, bool isAnonymized = false)
        {
            this._message = message;
            this._forceSend = forceSend;
            this._ensureUserExists = ensureUserExists;
            this._isAnonymized = isAnonymized;
        }

        protected override async Task<CommandResponse<Message>> ProtectedExecute()
        {
            using (var repo = new Repository())
            {
                return await repo.SendMessage(_message, _forceSend, _ensureUserExists, _isAnonymized).ConfigureAwait(false);
            }
        }
    }

    public class SendMessageReplyCommand : Command<CommandResponse<Message>>
    {
        private string _message;
        private int _messageID;

        public SendMessageReplyCommand(int messageID, string message)
        {
            this._message = message;
            this._messageID = messageID;
        }

        protected override async Task<CommandResponse<Message>> ProtectedExecute()
        {
            using (var repo = new Repository())
            {
                return await repo.SendMessageReply(_messageID, _message).ConfigureAwait(false);
            }
        }
    }
}
