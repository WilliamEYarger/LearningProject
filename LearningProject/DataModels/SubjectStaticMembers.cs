
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using LearningProject.HelperMethods;
namespace LearningProject.DataModels
{
    public static class SubjectStaticMembers
    {

        #region Properties

        #region SelectedNode
        /// <summary>
        /// The selected node is the subject node in the ListView which the user
        /// clicked
        /// </summary>
        private static SubjectNodes _SelectedNode;

        public static SubjectNodes SelectedNode
        {
            get { return _SelectedNode; }
            set { _SelectedNode = value; }
        }

        #endregion SelectedNode


        #region ItemCount
        /// <summary>
        /// ItemCount is the number of items created,
        ///     Not the number of items present because
        ///     some may have been deleted
        /// It is used to tie a subject to 
        ///    various data files
        /// </summary>
        private static int _ItemCount = 0;

        public static int ItemCount
        {
            get { return _ItemCount; }
            set { _ItemCount = value; }
        }
        #endregion ItemCount

        private static string ItemsCountFilePath;       


        #region Dictionary of Subject Nodes (SubjectNodeDictionary)
        // Create a dictionary of all subject nodes whose key is the node.NodeLevelName
        public static Dictionary<string, SubjectNodes> SubjectNodeDictionary = new Dictionary<string, SubjectNodes>();

        #endregion  SubjectNodeDictionary


        #region List of Display strings (DisplayList)

        //Create an List of strings for the ListView display and array of SubjectNodes to match it

        public static List<string> DisplayList = new List<string>();


        #endregion DisplayList


        #region List of Subject NodeLevelName strings ListView display (SubjectNodesLevelName)

        // Create a List of SubjectNode NodeLevelName strings to match DisplayList

        public static List<string> SubjectNodesLevelNameList = new List<string>();



       

        #endregion DisplaySubjectNodesList

        #endregion Properties

        #region Public Methods

        #region Open Data Files method (OpenFiles)

        /// <summary>
        /// This method receives the path to the main holding folder
        /// for a particular subject and opens add of the
        /// reqired data files to populate the ListView display
        /// of Subjects. The Name of the main holding folder
        /// is the display name of the Subects as well
        /// as of the .txt file that holds the display data
        /// </summary>
        /// <param name="HomeFolderPat"></param>
        public static void OpenFiles(string HomeFolderPath)
        {
            // Get the name of the subject from the last item in the path 

            // Get the number of '\\'s in FolderPath
            int NumberOfSlashes = StringHelper.ReturnNumberOfDeliniters(HomeFolderPath, '\\');

            // Get the Subjects Name from the item a position NumberOfSlashes -1
            var FolderName = StringHelper.ReturnItemAtPos(HomeFolderPath, '\\', NumberOfSlashes - 1);

            //Use the FolderName to name the Subject, and its main data files
            string SubjectName = FolderName;

            // Create path to this subjects data file
            var SubjectsDataFile = HomeFolderPath + SubjectName + ".txt";
            string SubjectDataFilePath = SubjectsDataFile;

            // Test to see if this file exist and if not create it
            if (!File.Exists(SubjectsDataFile))
            {
                /* This is a newly created subject so initialize the  ItemCounter to 0, 
                 * create the string [] array, ArrayOfSubjectNodes to hold all of the subject nodes,
                 * Create the Root node and set its counter to ItemCounter and update ItemCounter
                 */

                // create the initial count and write it to the ItemCount.bin file
                int CurrentItemCount = 0;

                ////Create a binary file to hold the current number of items created
                //ItemsCountFilePath = HomeFolderPath + "\\ItemCount.bin";
                //FileStream fs = new FileStream(ItemsCountFilePath, FileMode.Create);
                //BinaryWriter bw = new BinaryWriter(fs);

                // Create a new file to hold the items in the subject dictionary

                //Create a new RootNode
                SubjectNodes RootNode = new SubjectNodes(ItemCount);

                // Assign the CurrentItemCount to the Root node's ID
                RootNode.ID = ItemCount;
                ItemCount++;

                ////increment the current count and write it to the ItemsCount.bin file
                //CurrentItemCount++;
                //bw.Write(CurrentItemCount);

                //bw.Close();

                RootNode.CI = "- ";
                RootNode.NodeLevelName = "*";
                int LengthNodeLevelName = RootNode.NodeLevelName.Length;
                string LeadingChars = new string(' ', LengthNodeLevelName);
                RootNode.LeadingChars = LeadingChars;
                RootNode.NOC = 0;
                RootNode.TitleText = "Root";
                RootNode.HasData = false;

                //Add this rootnode to the SubjectNodeDictionary
                AddNodeToDictionary(RootNode);
                ////SubjectNodeDictionary.Add(RootNode.NodeLevelName, RootNode);

                // Create the DisplayString for the root node
                string RootDisplayString = $"{RootNode.LeadingChars}{RootNode.CI}{RootNode.TitleText}";

                //Add this root node to the DisplayList
                DisplayList.Add(RootDisplayString);

                // Add the root Node's NodeLevelName to the DisplaySubjectNodesList
                SubjectNodesLevelNameList.Add(RootNode.NodeLevelName);
            }
            else
            {
                // read in the SubjectsDataFile
            }// End if else file subject file exists


        }//End OpenFiles method

        internal static string GetLeadingChars(string nodeLevelName)
        {
            int LengthOfNodeLevelName = nodeLevelName.Length - 1;
            return new string(' ', LengthOfNodeLevelName*3);
        }

        internal static string GetNodeLevelPosition(int ParentsNOC)
        {
            string NodeLevelPositionString = "0123456789abcedfghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char [] NodeLevelPositionChars = NodeLevelPositionString.ToCharArray();
            char NodeLevelChar = NodeLevelPositionChars[ParentsNOC];
            return NodeLevelChar.ToString();

        }

        internal static string GetSubjectNodeAssociatedWithThisNodeLevelName(int indexOfListViewItem)
        {
            string ThisNodesLevelName = SubjectNodesLevelNameList[indexOfListViewItem];
            return ThisNodesLevelName;
        }


        #endregion OpenFiles


        #region Display a node's parents, the node and the nodes children (DisplayParentsAndChildren)

        /// <summary>
        /// This method creates the List of display strings for the ListView
        /// as well as a dictionary of Subject nodes whose
        /// </summary>
        /// <param name="ThisNode"></param>
        /// <returns></returns>
        public static List<string> DisplayParentsAndChildren(string ThisNodesLevelName)
        {

            // Clear the existing List
            DisplayList.Clear();
            SubjectNodesLevelNameList.Clear();

            // Display Parents and chosen node
            for(int i=0; i< ThisNodesLevelName.Length; i++)
            {
                string CurrentNodeLevelName = ThisNodesLevelName.Substring(0, i + 1);
                SubjectNodes CurrentNode = SubjectNodeDictionary[CurrentNodeLevelName];
                string ThisNodesDisplayString = ReturnDisplayString(CurrentNode);
                DisplayList.Add(ThisNodesDisplayString);
                SubjectNodesLevelNameList.Add(CurrentNodeLevelName);
            }

        // Get the children of the chosen node
           
            // Get the Length of the choden nodes node level name because its children NLN will be 1 longer 
            int L = ThisNodesLevelName.Length;
            // begins witn ThisNodesLevelName and whose length is 1 greater that that of ThisNodesLevelName

            foreach (KeyValuePair<string,SubjectNodes> KVP in SubjectNodeDictionary)
            {
                string Key = KVP.Key;
                SubjectNodes ThisNode = SubjectNodeDictionary[Key];
                if ( (Key.IndexOf(ThisNodesLevelName) == 0)) 
                {
                    if(ThisNode.NodeLevelName.Length == L + 1)
                    {
                        string ThisDisplaystring = ReturnDisplayString(ThisNode);
                        DisplayList.Add(ThisDisplaystring);
                        string ThisNodeLevelName = ThisNode.NodeLevelName;
                        SubjectNodesLevelNameList.Add(ThisNodeLevelName);
                    }
                }
            }

            return DisplayList;

        }// End DisplayParentsAndChildren method

        #endregion Reset the ListView Display (ResetDisplayList)

        #region Read in the count of items existing at startup  (GetCurrentItemCount)

        public static int GetCurrentItemCount()
        {
            //ItemCount = -1;
            if (File.Exists(ItemsCountFilePath))
            {
                BinaryReader binReader = new BinaryReader(File.Open(ItemsCountFilePath, FileMode.Open));
                ItemCount = binReader.ReadInt32();
            }

            return ItemCount;
        }// End GetCurrentItemCount

        #endregion (GetCurrentItemCount)

        #region AddNodeToDictionary

        public static void AddNodeToDictionary(SubjectNodes ThisNode)
        {
            SubjectNodeDictionary.Add(ThisNode.NodeLevelName, ThisNode);
        }
        #endregion AddNodeToDictionary

        #endregion Public Methods

        #region Priate Methods

        private static string ReturnDisplayString(SubjectNodes ThisNode)
        {
            string DisplayString = "";
            string LeadingString = ThisNode.LeadingChars;
            string ChildIndicator = ThisNode.CI;
            string NodeText = ThisNode.TitleText;
            DisplayString = LeadingString + ChildIndicator + NodeText;

            return DisplayString;
        }

        #endregion Priate Methods
    }
}
