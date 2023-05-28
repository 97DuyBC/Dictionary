namespace Dictionary.Model
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }

        public Node()
        {
            Data = default;
            Next = null;
            Previous = null;
        }
        public Node(T element)
        {
            Data = element;
            Next = null;
            Previous = null;
        }
    }
}
