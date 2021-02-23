
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
        private static SubjectNodes ParentNode;
        private static SubjectNodes NewChildNode;

        #endregion Properties

        #region Menu Item

        #region File Menu

        #region FileOpen_Click

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

        }// End FileOpen_Click


        #endregion FileOpen_Click


        #region Save File (FileSave_Click)

        /// <summary>
        /// Saves all the files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            SubjectStaticMembers.SaveFiles();
        }// End FileSave_Click
        #endregion FileSave_Click

        #endregion File Menu

        #region Instructions Menu

        #region Menu Item Open Folder Click Instructions


        private void mnuiOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To Select or Create the folder which will hold the data for this subject:\r\n " +
                "1. Click the File -> Select Data Folder\r\n " +
                "2. If choosing an existing folder highlight the folder and click \"Select Folder\"\r\n" +
                "2. If creating a new folder click the holding folder area and add an new folder and select it.");
        }// End Menu Item Open Folder Click Instructions

        #endregion Menu Item Open Folder Click Instructions

        #endregion Instructions Menu


        #endregion  Menu Items

    #region Radio buttons


        #region RadioButton NewChild Click

        private void rbNewChild_Checked(object sender, RoutedEventArgs e)
        {
            var rbNewChild = sender as RadioButton;

            if (tbxNodeName.Text == "")
            {
                MessageBox.Show("You must Enter text into the Enter Node Text TextBox and select a Parent Node");
                rbNewChild.IsChecked = false;
                return;
            }
            if (SelectedNode == null)
            {
                MessageBox.Show("You Must select a Parent Node before Clicking Create a New Child Node");
                rbNewChild.IsChecked = false;
                return;
            }
            if (SelectedNode.CI == "T ")
            {
                MessageBox.Show("You Cannot add a child to a Terminal node");
                rbNewChild.IsChecked = false;
                return;
            }

            // Get ItemIndex
            int CurrentItemCount = SubjectStaticMembers.ItemCount;

            // Create a new node
            CreateNewChildSubjectNode(CurrentItemCount);

        }// End rbNewChild_Checked

        #endregion RadioButton NewChild Click


        #region Radio button Expand/Contract Checked  (rbExpandCollapse_Checked)

        /// <summary>
        /// When a node's children are not showing clicking
        /// will show this node, its parents and it children
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbExpandCollapse_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedNode == null)
            {
                MessageBox.Show("You must select a subject node before proceeding!");
                rbExpandCollapse.IsChecked = false;
                return;
            }
            string SelectedNodeNameLevelName = SelectedNode.NodeLevelName;
            SubjectStaticMembers.DisplayParentsAndChildren(SelectedNodeNameLevelName);
            lvSubjects.Items.Clear();
            foreach (string item in SubjectStaticMembers.DisplayList)
            {
                lvSubjects.Items.Add(item);
            }
            rbExpandCollapse.IsChecked = false;
            SelectedNode = null;
        }// End rbExpandCollapse_Checked

        #endregion rbExpandCollapse_Checked


        #region Radio button Change Title Text of selected node (rbText_Checked)
        /// <summary>
        /// The purpose of this method is to 
        /// change the TitleText of the selected node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbText_Checked(object sender, RoutedEventArgs e)
        {
            if (tbxNodeName.Text == "")
            {
                MessageBox.Show("You must Enter text into the Enter Node Text TextBox and select the node to Change");
                rbText.IsChecked = false;
                return;
            }
            if (SelectedNode == null)
            {
                MessageBox.Show("You Must select a Node before Clicking Change Title Text");
                rbText.IsChecked = false;
                return;
            }

            string NewTitleText = tbxNodeName.Text;
            SelectedNode.TitleText = NewTitleText;
            string ThisNodeLevelName = SelectedNode.NodeLevelName;
            SubjectStaticMembers.SubjectNodeDictionary[ThisNodeLevelName] = SelectedNode;

            List<string> NewDisplayList = SubjectStaticMembers.DisplayParentsAndChildren(ThisNodeLevelName);

            lvSubjects.Items.Clear();

            foreach(string DisplayLine in NewDisplayList)
            {
                lvSubjects.Items.Add(DisplayLine);
            }


        }// End rbText_Checked
        #endregion rbText_Checked


        #region Radio button (rbTerminal_Checked)
        private void rbTerminal_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedNode == null)
            {
                MessageBox.Show("You must select a subject node before proceeding!");
                rbTerminal.IsChecked = false;
                return;
            }

            SelectedNode.CI = "T ";
            SubjectStaticMembers.SubjectNodeDictionary[SelectedNode.NodeLevelName] = SelectedNode;


            List<string> NewDisplayList = SubjectStaticMembers.DisplayParentsAndChildren(SelectedNode.NodeLevelName);

            lvSubjects.Items.Clear();

            foreach (string DisplayLine in NewDisplayList)
            {
                lvSubjects.Items.Add(DisplayLine);
            }


        }// End rbTerminal_Checked

        #endregion(rbTerminal_Checked)


        #region Radio Button Delete (rbDelete_Checked)

        private void rbDelete_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedNode == null)
            {
                MessageBox.Show("You must select a subject node To delete!");
                rbDelete.IsChecked = false;
                return;
            }

            if(SelectedNode.HasData == true)
            {
                MessageBox.Show("You cannot delete a Node that has associated data files");
                rbDelete.IsChecked = false;
                SelectedNode = null;
                return;
            }

            if(SelectedNode.NOC > 0)
            {
                MessageBox.Show("You cannot delete a Node that has children");
                rbDelete.IsChecked = false;
                SelectedNode = null;
                return;
            }

            //Get the parent's NodeLevelName
            string ThisNodesLevelName = SelectedNode.NodeLevelName;
            string ParentsNodeLevelName = ThisNodesLevelName.Substring(0, ThisNodesLevelName.Length - 1);

            // Delete the node from the dictionary

            SubjectStaticMembers.RemoveNodeFromDictionary(ThisNodesLevelName);

            // Reset the display
            lvSubjects.Items.Clear();

            // get the  the parent display list
            List<string> ParentsDisplayList = SubjectStaticMembers.DisplayParentsAndChildren(ParentsNodeLevelName);

            // display them
            foreach(string DisplayLine in ParentsDisplayList)
            {
                lvSubjects.Items.Add(DisplayLine);
            }

            rbDelete.IsChecked = false;
        }// End rbDelete_Checked
        #endregion rbDelete_Checked
        #endregion Radio buttons

        #region Private Methods


        #region Create a new child node  (CreateNewChildSubjectNode)
        /// <summary>
        /// This method receives the curent number of items creates
        ///     (currentItemCount) and creates a new node wiht this as the ID
        ///  It gets the parent from the list view index of the selected node
        ///  It Adds the node to the dictionary, adjusts the Parent node
        ///  and resets the display to reflect the Parents children
        /// </summary>
        /// <param name="currentItemCount"></param>
        private void CreateNewChildSubjectNode(int currentItemCount)
        {

            // Instantiate a SubjectNode with the currentItemCount
            NewChildNode = new SubjectNodes(currentItemCount);

            // Get the Parent Node
            ParentNode = GetParentNode();

            //Get the Parent's Number of Children to calcuate the child's NodeLevelName
            int ParentsNumchildren = ParentNode.NOC;

            //Get the Child's NodeLevelPosition
            string ChildsNodeLevelPosition = SubjectStaticMembers.GetNodeLevelPosition(ParentsNumchildren);

            // Set the NewChildNodes NodeLevel
            NewChildNode.NodeLevelName = ParentNode.NodeLevelName + ChildsNodeLevelPosition;

            // Set the Child node's leading char sgtring
            NewChildNode.LeadingChars = SubjectStaticMembers.GetLeadingChars(NewChildNode.NodeLevelName);

            // Set the Child indicator to no children
            NewChildNode.CI = "- ";

            //Set the TitleText to the text in the node name textbox
            NewChildNode.TitleText = tbxNodeName.Text;

            // set the has associated data files to false
            NewChildNode.HasData = false;

            // set the child node's number of children to 0
            NewChildNode.NOC = 0;

            // Add this child to the dictionary
            SubjectStaticMembers.AddNodeToDictionary(NewChildNode);

            // Increment the parents NOC and CI
            ParentNode.CI = "+ ";
            ParentNode.NOC++;

            // NEW 20210222 START

            SubjectStaticMembers.DisplayParentsAndChildren(ParentNode.NodeLevelName);
            lvSubjects.Items.Clear();
            foreach (string item in SubjectStaticMembers.DisplayList)
            {
                lvSubjects.Items.Add(item);
            }

            // Clear the NodeName textbox
            tbxNodeName.Text = "";

            //UnCheck the new child radio button
            rbNewChild.IsChecked = false;

            //store the updated parents node in the dictionary
            SubjectStaticMembers.SubjectNodeDictionary[ParentNode.NodeLevelName] = ParentNode;
            //Increment and save the ItemsIndex
            SubjectStaticMembers.ItemCount++;
            SelectedNode = null;
        }// End CreateNewChildSubjectNode


        #endregion (CreateNewChildSubjectNode)

        #region Get the Parent (ie the selected node) of a new child node  (GetParentNode)

        /// <summary>
        /// Use the Selected Index to get the parent of a new child node
        /// </summary>
        /// <returns></returns>
        private SubjectNodes GetParentNode()
        {
            int SelectedIndex = lvSubjects.SelectedIndex;
            string SelectedNodesLevelName = SubjectStaticMembers.SubjectNodesLevelNameList[SelectedIndex];
            // Create ParentNode
            ParentNode = SubjectStaticMembers.SubjectNodeDictionary[SelectedNodesLevelName];
            return ParentNode;
        }// End GetParentNode
        #endregion GetParentNode

        #region Detect when the left mouse button is up to get the selected node  (lvSubjects_PreviewMouseLeftButtonUp)

        /// <summary>
        /// The preview left mouse button up is used
        /// to ascertain which node in the ListView was selected
        /// because at mouse down the selection is still null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSubjects_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvSubjects.SelectedIndex >= 0)
            {
                // Get the NodeLevelName for this node from the SubjectNodesLevelNameList
                string NodeLevelName = SubjectStaticMembers.SubjectNodesLevelNameList[lvSubjects.SelectedIndex];

                // Use the NodeLevelName to get the correct node fromthe dictionary of SubjectNodeDictionary
                SelectedNode = SubjectStaticMembers.SubjectNodeDictionary[NodeLevelName];
            }
        }// End lvSubjects_PreviewMouseLeftButtonUp


        #endregion lvSubjects_PreviewMouseLeftButtonUp

        #endregion Private Methods


    }
}
