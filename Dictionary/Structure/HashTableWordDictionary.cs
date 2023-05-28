using Dictionary.Model;

namespace Dictionary.Structure
{
    public class HashTableWordDictionary
    {
        public LinkedListCustom<WordDictionary>[] bucket;
        public readonly int size;

        public HashTableWordDictionary(int tableSize)
        {
            this.size = tableSize;
            bucket = new LinkedListCustom<WordDictionary>[size];
        }

        //Hàm băm
        private int HashFuncation(string key)
        {
            return key[0] % size;
        }

        //add word to hash table
        public void InsertWord(WordDictionary value)
        {
            int index = HashFuncation(value.Word);
            var linkList = bucket[index];

            //chưa tồn tại link list
            if (linkList == null)
            {
                //thêm mới link list và word
                var newLinkList = new LinkedListCustom<WordDictionary>();
                newLinkList.Add(value);
                bucket[index] = newLinkList;
                return;
            }

            //kiểm tra đã tồn tại word chưa
            var node = linkList.Find(value);
            if (node != null)
            {
                //tồn đã tồn tại
                throw new Exception("Word is exist in dictionary");
            }

            //thêm mới vào link list
            linkList.Add(value);
        }

        //get word
        public WordDictionary GetWord(string key)
        {
            int index = HashFuncation(key);
            var linkList = bucket[index];

            if (linkList != null)
            {
                var word = linkList.Find(new WordDictionary(key));
                return word?.Data;
            }
            return null;
        }

        //remove word
        public bool RemoveWord(string key)
        {
            int index = HashFuncation(key);
            var linkList = bucket[index];

            if (linkList != null)
            {
                var nodeRemove = linkList.Remove(new WordDictionary(key));
                if(nodeRemove != null)
                {
                    return true;
                }
            }
            return false;
        }

        //update word
        public bool UpdateWord(WordDictionary input)
        {
            int index = HashFuncation(input.Word);
            var linkList = bucket[index];

            if (linkList != null)
            {
                var word = linkList.Find(new WordDictionary(input.Word));
                word.Data = input;
                return true;
            }
            return false;
        }
    }
}
