namespace Dictionary.Dto
{
    public class WordDictionaryFileDto
    {
        public WordDictionaryFileDto() {
            WordMeaning = new string[5];
            Example = new List<string>();
        }
        public string Word { get; set; }
        public int Type { get; set; }
        public string[] WordMeaning { get; set; } = new string[5];
        public List<string> Example { get; set; }
    }
}
