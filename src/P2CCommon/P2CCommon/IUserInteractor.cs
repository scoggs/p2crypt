using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2CCommon
{
    public interface IUserInteractor
    {
        void Notify(string information);
        void Notify(string text, string caption);
        ITextList TextMessage { get; }
        ITextList TextChatWindow { get; }
        int DefaultPort { get; }
        ITextList TextFriendsList { get; }
    }

    public interface ITextList
    {
        void AppendText(string text);
        void Clear();
        string Text { get; }
    }
}
