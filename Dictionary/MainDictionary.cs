using Dictionary.Dto;
using Dictionary.Model;
using Dictionary.Structure;
using Krypton.Toolkit;
using Newtonsoft.Json;
using System.Text;

namespace Dictionary
{
    public partial class MainDictionary : Form
    {
        public string FilePath = "F:\\Project\\Dictionary\\Dictionary\\DictionaryTextFile.json";
        public static MainDictionary Instance { get; private set; }

        public HashTableWordDictionary Dictionary = new HashTableWordDictionary(10);

        public void SaveToFile()
        {
            var output = new List<WordDictionaryFileDto>();
            //loop hash table
            for (var i = 0; i < Dictionary.size; i++)
            {
                var linkList = Dictionary.bucket[i];
                if (linkList != null)
                {
                    //loop link list word
                    var currentNode = linkList.Header;
                    while (currentNode != null)
                    {
                        if (currentNode.Data != null)
                        {
                            var word = new WordDictionaryFileDto();
                            var wordNode = currentNode.Data;
                            word.Word = wordNode.Word;
                            word.Type = wordNode.Type;
                            word.WordMeaning = wordNode.WordMeaning;

                            //loop link list example
                            var exampleNode = wordNode.Example.Header;
                            while (exampleNode != null)
                            {
                                if (exampleNode.Data != null)
                                {
                                    word.Example.Add(exampleNode.Data);
                                }
                                exampleNode = exampleNode.Next;
                            }
                            output.Add(word);
                        }
                        currentNode = currentNode.Next;

                    }
                }
            }

            //save to file
            string json = JsonConvert.SerializeObject(output);
            File.WriteAllText(FilePath, json);
        }

        public MainDictionary()
        {
            LoadData();
            InitializeComponent();
            this.ActiveControl = kryptonTextBox1;
            Instance = this;
        }

        //load data from file
        public void LoadData()
        {
            //check exist file
            if (File.Exists(FilePath))
            {
                //read file -> list object
                var source = new List<WordDictionaryFileDto>();
                using (StreamReader r = new StreamReader(FilePath))
                {
                    string json = r.ReadToEnd();
                    source = JsonConvert.DeserializeObject<List<WordDictionaryFileDto>>(json);
                }

                //add list to data structure
                foreach (var item in source)
                {
                    var newWord = new WordDictionary();
                    newWord.Word = item.Word;
                    newWord.Type = item.Type;
                    newWord.WordMeaning = item.WordMeaning;
                    var linkListExample = new LinkedListCustom<string>();

                    foreach (var meaning in item.Example)
                    {
                        linkListExample.Add(meaning);
                    }
                    newWord.Example = linkListExample;

                    Dictionary.InsertWord(newWord);
                }
            }
        }

        //text search change
        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {
            kryptonListBox1.Visible = false;
            kryptonListBox1.SelectedIndex = -1;
            var searchWord = kryptonTextBox1.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchWord))
            {
                return;
            }

            var result = new List<string>();

            for (var i = 0; i < Dictionary.size; i++)
            {
                var linkList = Dictionary.bucket[i];
                var curerntNode = linkList?.Header;
                while (curerntNode != null)
                {
                    if (curerntNode.Data != null &&
                        !string.IsNullOrEmpty(curerntNode.Data.Word) &&
                        curerntNode.Data.Word.StartsWith(searchWord, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(curerntNode.Data.Word);
                    };
                    curerntNode = curerntNode.Next;
                }
            }

            if (result.Any())
            {
                kryptonListBox1.Items.Clear();
                foreach (var item in result)
                {
                    kryptonListBox1.Items.Add(item);
                }
                kryptonListBox1.Visible = true;
            }
        }

        //select word when select
        private void kryptonListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            kryptonListBox1.Visible = false;
            guna2GradientButton5.Visible = false;
            guna2GradientButton3.Visible = false;

            var searchKey = kryptonListBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(searchKey))
            {
                var word = Dictionary.GetWord(searchKey);
                if (word != null)
                {
                    //get word meaning
                    var meaning = new StringBuilder();
                    for (var i = 0; i < word.WordMeaning.Length; i++)
                    {
                        var item = word.WordMeaning[i];
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            meaning.AppendLine(item.ToString());
                            if (i != word.WordMeaning.Length -1)
                            {
                                meaning.AppendLine("-----------------######-----------------");
                            }
                        }
                    }

                    //get example from link list
                    var example = new StringBuilder();
                    var currenNode = word.Example.Header;
                    while (currenNode != null)
                    {
                        if (!string.IsNullOrWhiteSpace(currenNode.Data))
                        {
                            example.AppendLine(currenNode.Data);
                            //check not last node
                            if (currenNode.Next.Next != null)
                            {
                                example.AppendLine("-----------------######-----------------");
                            }
                        }
                        currenNode = currenNode.Next;
                    }

                    //set value to UI
                    wordKeyTxt.Text = word.Word;
                    kryptonRichTextBox1.Text = meaning.ToString();
                    kryptonRichTextBox2.Text = example.ToString();

                    //0 : Danh từ
                    //1 : Tính từ
                    //2 : Động từ
                    guna2ComboBox1.SelectedIndex = word.Type;

                    guna2GradientButton5.Visible = true;
                    guna2GradientButton3.Visible = true;
                }
            }
        }

        private void ResetFrom()
        {
            wordKeyTxt.Text = "";
            kryptonRichTextBox1.Text = "";
            kryptonRichTextBox2.Text = "";

            SaveToFile();
        }

        //remove
        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            var key = wordKeyTxt.Text;
            Dictionary.RemoveWord(key);
            SaveToFile();
            ResetFrom();
        }

        //form edit
        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(wordKeyTxt.Text))
            {
                var editForm = new CreateOrEdit(false, wordKeyTxt.Text);
                editForm.ShowDialog();
            }
        }

        //form create
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var editForm = new CreateOrEdit(true, wordKeyTxt.Text);
            editForm.ShowDialog();
        }

        private void kryptonListBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
