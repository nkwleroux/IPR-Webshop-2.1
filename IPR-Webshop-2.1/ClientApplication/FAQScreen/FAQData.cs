using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{
    class FAQData
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public FAQData(string question, string answer)
        {
            this.Question = question;
            this.Answer = answer;
        }
    }
}
