using Dictionary.Structure;

namespace Dictionary.Model
{
    public class WordDictionary
    {
        public WordDictionary()
        {
            WordMeaning = new string[5];
            Example = new LinkedListCustom<string>();
        }
        public WordDictionary(string word)
        {
            Word = word;
        }

        public string Word { get; set; }
        public int Type { get; set; }
        public string[] WordMeaning { get; set; }
        public LinkedListCustom<string> Example { get; set; }
    }
}
