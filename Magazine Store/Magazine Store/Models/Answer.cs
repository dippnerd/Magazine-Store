using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine_Store.Models
{
    public class Answer
    {
        public AnswerData data { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
    }
    public class AnswerData
    {
        public string totalTime { get; set; }
        public bool answerCorrect { get; set; }
        public object shouldBe { get; set; }
    }

}
