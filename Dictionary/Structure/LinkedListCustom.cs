using Dictionary.Model;

namespace Dictionary.Structure
{
    public class LinkedListCustom<T>
    {
        public Node<T> Header { get; set; } = new Node<T>();
        public Node<T> Last { get; set; } = new Node<T>();

        public LinkedListCustom()
        {
            Header.Next = Last;
            Last.Previous = Header;
        }

        //find from header
        public Node<T> Find(T data)
        {
            //bắt đầu từ node đầu tiên
            var currentNode = Header;
            while (currentNode.Next != null && !CompareData(currentNode.Data, data))
            {
                //loop qua các node để tìm node thỏa mãn
                currentNode = currentNode.Next;
            }

            //nếu node đó ko phải đầu, cuối return node
            if (currentNode != Header && currentNode != Last)
            {
                return currentNode;
            }

            return null;
        }

        //public Node<T> FindFromLast(T data)
        //{
        //    var currentNode = Last;
        //    while (currentNode.Previous != null && currentNode.Data?.CompareTo(data) != 0)
        //    {
        //        currentNode = currentNode.Previous;
        //    }
        //    if (currentNode != Header && currentNode != Last)
        //    {
        //      return currentNode;
        //    }
        //    return null;
        //}

        public Node<T> Insert(T data, T afterValue)
        {
            var newNode = new Node<T>(data);
            var currentNode = Find(afterValue);
            if (currentNode != null)    
            {
                newNode.Previous = currentNode;
                newNode.Next = currentNode.Next;

                currentNode.Next = newNode;
                return newNode;
            }
            else
            {
                return null;
            }
        }

        //add to last list
        public Node<T> Add(T data)
        {
            var newNode = new Node<T>(data);

            //get node before last node
            var beforeLastNode = Last.Previous;
            beforeLastNode.Next = newNode;

            //new node
            newNode.Next = Last;
            newNode.Previous = beforeLastNode;

            Last.Previous = newNode;
            return newNode;
        }

        //add to header list
        //public Node<T> AddToHeader(T data)
        //{
        //    var newNode = new Node<T>(data);

        //    //get node before last node
        //    var afterHeadNode = Header.Previous;
        //    afterHeadNode.Previous = newNode;

        //    //new node
        //    newNode.Next = afterHeadNode;
        //    newNode.Previous = Header;

        //    Header.Next = newNode;
        //    return newNode;
        //}


        //remove node
        public Node<T> Remove(T data)
        {
            var currenNode = Find(data);
            if (currenNode != null && currenNode != Header && currenNode != Last)
            {
                currenNode.Previous.Next = currenNode.Next;
                currenNode.Next.Previous = currenNode.Previous;
            }
            return currenNode;
        }

        //public void Traverse(Action<T> action)
        //{
        //    var currentNode = Header;
        //    while (currentNode.Next != null)
        //    {
        //        action?.Invoke(currentNode.Next.Data);
        //        currentNode = currentNode.Next;
        //    }
        //}

        private bool CompareData<T>(T value1, T value2)
        {
            //WordDictionary
            if (typeof(T) == typeof(WordDictionary))
            {
                var word1 = Convert.ChangeType(value1, typeof(WordDictionary)) as WordDictionary;
                var word2 = Convert.ChangeType(value2, typeof(WordDictionary)) as WordDictionary;
                if (word1 != null && word2 != null && word1.Word == word2.Word)
                {
                    return true;
                }
            }

            //String
            if (typeof(T) == typeof(string))
            {
                var str1 = Convert.ChangeType(value1, typeof(string)) as string;
                var str2 = Convert.ChangeType(value2, typeof(string)) as string;
                if (str1 != null && str2 != null && str1 == str2)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
