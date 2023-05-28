using Dictionary.Model;
using Dictionary.Structure;

namespace Dictionary
{
    public partial class CreateOrEdit : Form
    {

        private bool isCreated { get; set; }
        private WordDictionary word { get; set; } = new WordDictionary();

        public CreateOrEdit() { }

        public CreateOrEdit(bool isCreated = true, string key = "")
        {
            this.isCreated = isCreated;
            InitializeComponent();
            if (!isCreated)
            {
                var currentWord = MainDictionary.Instance.Dictionary.GetWord(key);
                if (currentWord != null)
                {
                    word.Word = currentWord.Word;
                    word.WordMeaning = currentWord.WordMeaning;
                    word.Example = currentWord.Example;
                    word.Type = currentWord.Type;
                }
            }

            this.Text = isCreated ? "Create word" : "Edit word";
            wordKeyTxt.ReadOnly = !isCreated;
            wordKeyTxt.Text = word.Word;
            guna2ComboBox1.SelectedIndex = word.Type;

            foreach (var item in word.WordMeaning)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    listView1.Items.Add(item);
                }
            }

            var currentNodeExample = word.Example?.Header;
            while (currentNodeExample != null)
            {
                if (currentNodeExample.Data != null)
                {
                    listView2.Items.Add(currentNodeExample.Data);
                }
                currentNodeExample = currentNodeExample.Next;
            }
        }

        //save
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            try
            {
                word.Word = wordKeyTxt.Text.ToLower();
                word.Type = guna2ComboBox1.SelectedIndex;

                if (string.IsNullOrEmpty(word.Word))
                {
                    MessageBox.Show("Word is required");
                    return;
                }

                //get data meaning
                word.WordMeaning = new string[5];
                for (var i = 0; i < listView1.Items.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listView1.Items[i].SubItems[0].Text))
                    {
                        word.WordMeaning[i] = listView1.Items[i].SubItems[0].Text;
                    }
                }

                if (!word.WordMeaning.Any(x => x != null))
                {
                    MessageBox.Show("Word meaning is required");
                    return;
                }

                //get data example
                word.Example = new LinkedListCustom<string>();
                for (var i = 0; i < listView2.Items.Count; i++)
                {
                    word.Example.Add(listView2.Items[i].SubItems[0].Text);
                }

                if (isCreated)
                {
                    //create
                    MainDictionary.Instance.Dictionary.InsertWord(word);
                }
                else
                {
                    //update
                    MainDictionary.Instance.Dictionary.UpdateWord(word);
                }

                MainDictionary.Instance.SaveToFile();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //add meaning
        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(kryptonRichTextBox1.Text))
            {
                if (listView1.Items.Count < 5)
                {
                    var item = new ListViewItem(kryptonRichTextBox1.Text);
                    listView1.Items.Add(item);
                    kryptonRichTextBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("Limit number meaning of word");
                }
            }
            else
            {
                MessageBox.Show("Please enter content first");
            }
        }

        //remove meaning
        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            else
            {
                MessageBox.Show("Please select row to remove");
            }
        }

        //load update example
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                kryptonRichTextBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            }
        }

        //add example
        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(kryptonRichTextBox2.Text))
            {
                var item = new ListViewItem(kryptonRichTextBox2.Text);
                listView2.Items.Add(item);
                kryptonRichTextBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter content first");
            }
        }

        //remove example
        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                listView2.Items.Remove(listView2.SelectedItems[0]);
            }
            else
            {
                MessageBox.Show("Please select row to remove");
            }
        }

        //load update example
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                kryptonRichTextBox2.Text = listView2.SelectedItems[0].SubItems[0].Text;
            }
        }

        //update meaning
        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && !string.IsNullOrEmpty(kryptonRichTextBox1.Text))
            {
                listView1.SelectedItems[0].SubItems[0].Text = kryptonRichTextBox1.Text;
            }
        }

        //update example
        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0 && !string.IsNullOrEmpty(kryptonRichTextBox2.Text))
            {
                listView2.SelectedItems[0].SubItems[0].Text = kryptonRichTextBox2.Text;
            }
        }
    }
}
