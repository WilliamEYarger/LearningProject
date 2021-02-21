
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using LearningProject.HelperMethods;
using LearningProject.DataModels;
using System.Windows.Controls;
using System.Collections.Generic;

namespace LearningProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region Properties
        private static SubjectNodes SelectedNode;
        private static SubjectNodes NewParentNode;
        private static SubjectNodes NewChildNode;

        #endregion Properties
        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            // Get the Name of the Subject folder
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            string FolderPath = "";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName + '\\';
            }

            // Get the number of '\\'s in FolderPath
            var NumberOfSlashes = StringHelper.ReturnNumberOfDeliniters(FolderPath, '\\');


            // Get the Subjects Name from the item a position NumberOfSlashes -1
            var FolderName = StringHelper.ReturnItemAtPos(FolderPath, '\\', NumberOfSlashes - 1);
            tblkSubjectName.Text = FolderName;

            //Communicate the FolderPath to the ViewModel.SubjectNodeViewModel's OpenFile method
            SubjectStaticMembers.OpenFiles(FolderPath);

           //lvSubjects.ItemsSource = SubjectStaticMembers.DisplayList;
            string rootItem = (string)SubjectStaticMembers.DisplayList[0];
            lvSubjects.Items.Add((object)rootItem);

        }

        private void mnuiOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To Select or Create the folder which will hold the data for this subject:\r\n " +
                "1. Click the File -> Select Data Folder\r\n " + 
                "2. If choosing an existing folder highlight the folder and click \"Select Folder\"\r\n" +
                "2. If creating a new folder click the holding folder area and add an new folder and select it.");
        }

        private void rbNewChild_Click(object sender, RoutedEventArgs e)
        {
            if(tbxNodeName.Text == "")
            {
                MessageBox.Show("You must Enter text into the Enter Node Text TextBox and select a Parent Node");
            }
            if(SubjectStaticMembers.SelectedNode == null)
            {
                MessageBox.Show("You Must select a Parent Node before Clicking Create a New Child Node");
            }
        }

        //Select an item in the lvSubjects and create a SelectedNode from its NodeLevelName
        private void lvSubjects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(lvSubjects.SelectedItems != null)
            {

                int SelectedNodePosition = lvSubjects.SelectedIndex;
                string SelectedNodesLevelName = SubjectStaticMembers.SubjectNodesLevelNameList[SelectedNodePosition];
                SelectedNode = SubjectStaticMembers.SubjectNodeDictionary[SelectedNodesLevelName];
            }
            
        }

        private void rbNewChild_Checked(object sender, RoutedEventArgs e)
        {
            var rbNewChild = sender as RadioButton;

            if (tbxNodeName.Text == "")
            {
                MessageBox.Show("You must Enter text into the Enter Node Text TextBox and select a Parent Node");
            }
            if (SelectedNode == null)
            {
                MessageBox.Show("You Must select a Parent Node before Clicking Create a New Child Node");
            }

           
            // Get ItemIndex

            int CurrentItemCount = SubjectStaticMembers.GetCurrentItemCount();


            // Create a new node

            CreateNewChildSubjectNode(CurrentItemCount);
           
        }

        private void CreateNewChildSubjectNode(int currentItemCount)
        {

            // Instantiate a SubjectNode with the currentItemCount
            NewChildNode = new SubjectNodes(currentItemCount);

            // Get the Parent Node
            NewParentNode = GetParentNode();

            //Get the Parent's Number of Children to calcuate the child's NodeLevelName
            int ParentsNumchildren = NewParentNode.NOC;

            //Get the Child's NodeLevelPosition
            string ChildsNodeLevelPosition = SubjectStaticMembers.GetNodeLevelPosition(ParentsNumchildren);

            // Set the NewChildNodes NodeLevel
            NewChildNode.NodeLevelName = NewParentNode.NodeLevelName + ChildsNodeLevelPosition;
            NewChildNode.LeadingChars = SubjectStaticMembers.GetLeadingChars(NewChildNode.NodeLevelName);
            NewChildNode.LeadingChars = "- ";
            NewChildNode.TitleText = tbxNodeName.Text;
            NewChildNode.HasData = false;
            NewChildNode.NOC = 0;

            // Add this child to the dictionary
            SubjectStaticMembers.AddNodeToDictionary(NewChildNode);

            // reeset the displau
            List<string> DisplayList =      SubjectStaticMembers.ResetDisplayList(NewChildNode.NodeLevelName);
            lvSubjects.Items.Clear();
            foreach(string item in DisplayList)
            {
                lvSubjects.Items.Add(item);
            }




            //Increment and save the ItemsIndex
        }

        private SubjectNodes GetParentNode()
        {
            int SelectedIndex = lvSubjects.SelectedIndex;
            string SelectedNodesLevelName = SubjectStaticMembers.SubjectNodesLevelNameList[SelectedIndex];
            // Create ParentNode
            NewParentNode = SubjectStaticMembers.SubjectNodeDictionary[SelectedNodesLevelName];
            return NewParentNode;
        }

        private void lvSubjects_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(lvSubjects.SelectedItems != null)
            {
                int PosSelectedNode = lvSubjects.SelectedIndex;
            }
        }



        private void lvSubjects_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show(lvSubjects.SelectedValue.ToString());
        }
    }
}
