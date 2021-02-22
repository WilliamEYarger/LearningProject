using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProject.HelperMethods;

namespace LearningProject.DataModels
{
    public class SubjectNodes
    {

        #region Constructors

        public SubjectNodes()
        {

        }

        public SubjectNodes(int ItemCount)
        {
            ID = ItemCount;
        }

        #endregion Constructor

        #region Public Properties

        #region LeadingChars

        /// <summary>
        /// This string of ' ' will be used to indicate the node's position in the
        /// hiderarchy of nodes displayes in the ListView
        /// Level 0 = ""
        /// Level 1 = "   "
        /// Level 2 = "      " 
        /// etc
        /// </summary>
        private string _LeadingChars;

        public string LeadingChars
        {
            get { return _LeadingChars; }
            set { _LeadingChars = value; }
        }
        #endregion LeadingChar

        #region ChildIndicatorString
        /// <summary>
        /// This string has only 3 possibilities
        /// 1. "- "; indicating currently there are no child nodes
        /// 2. "+ "; indicating currently there are are child nodes
        /// 3. "T "; indicating a terminal node (with no possiblity of having children)
        /// </summary>
        private string _CI = "- ";

        public string CI
        {
            get { return _CI; }
            set
            {
                if (value.Length == 2)
                {
                    _CI = value;
                }
                else
                {
                    throw new FormatException("The Child Indicator length must be 2 characters");
                }
            }    
        }


        #endregion ChildIndicatorString

        #region TitleText
        /// <summary>
        /// This is the text string that will appear in the List View
        /// </summary>
        private string _TitleText;

        public string TitleText
        {
            get { return _TitleText; }
            set { _TitleText = value; }
        }


        #endregion TitleText

        #region NodeLevelName
        /// <summary>
        /// this mutable porperty  of AlphaNumeric characters will be a unique identifier that is created using 
        /// single alpahnumeric characters [0..9][a..z[A..Z] to create a 
        /// string, in which the length of the string indicates its position in the hierachy
        /// Its terminal char will refelect its child number of its parent
        /// and the leading characters will be its parent's NodeLevelName
        /// </summary>
        private string _NodeLevelName;

        public string NodeLevelName
        {
            get { return _NodeLevelName; }
            set { _NodeLevelName = value; }
        }

        #endregion NodeLevelName

        #region ID
        /// <summary>
        /// this immutalbe integer will be created by adding 1 the the number of object created. 
        /// It cannot be changed. If for some reason the user decides to move an object to some 
        /// other place in the hierachy its name and the name of all of its children would
        /// change to reflect its new position in the hierarchy, but its ID, which could possible 
        /// be used to link this object to some external data resource, such as a QA file, 
        /// a information URL, a data file etc would never change.
        /// </summary>
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        #endregion ID

        #region NumberOfChildren (NOC)
        /// <summary>
        /// This integer indicated the number of primary children that the objects has at any time, 
        /// subject to the addition or removal at some later time.It does not indicate the number 
        /// of any grand children etc. It is used to calculate the object NodeLevelName terminal 
        /// character as described above
        /// </summary>
        private int _NOC;

        public int NOC
        {
            get { return _NOC; }
            set { _NOC = value; }
        }


        #endregion NumberOfChildren (NOC)

        #region HasData
        /// <summary>
        /// this boolean indicates whether there are any accessory data files assigned 
        /// to this node's ID. If it is true, this precludes deleting this node.
        /// </summary>
        private bool _HasData;

        public bool HasData
        {
            get { return _HasData; }
            set { _HasData = value; }
        }


        #endregion HasData


        #endregion Public Properties

        //#region Static Properties

        //private static int _ItemCount = 0;

        //public static int ItemCount
        //{
        //    get { return _ItemCount ; }
        //    set { _ItemCount  = value; }
        //}

        //#endregion Static Properties

        #region Public static Methods

        //public static void OpenFiles(string HomeFolderPat)
        //{
        //    // Get the name of the subject from the last entry in the path + ".txt"
        //    // Get the number of '\\'s in FolderPath
        //    var NumberOfSlashes = StringHelper.ReturnNumberOfDeliniters(HomeFolderPat, '\\');


        //    // Get the Subjects Name from the item a position NumberOfSlashes -1
        //    var FolderName = StringHelper.ReturnItemAtPos(HomeFolderPat, '\\', NumberOfSlashes - 1);


        //    string SubjectName = FolderName;
        //    // Create path to this subjects  data file
        //    var SubjectsDataFile = HomeFolderPat + SubjectName + ".txt";
        //    string SubjectDataFilePath = SubjectsDataFile;

        //    #region Create a SubjectsName.txt file if it doesn't exist

        //    // Test to see if this file exist and if not create it
        //    if (!File.Exists(SubjectsDataFile))
        //    {
        //        /* This is a newly created subject so initialize the  ItemCounter to 0, 
        //         * create the string [] array, ArrayOfSubjectNodes to hold all of the subject nodes,
        //         * Create the Root node and set its counter to ItemCounter and update ItemCounter
        //         */

        //        SubjectNodes.ItemCount = 0;
        //        int CurrentItemCount = SubjectNodes.ItemCount;
        //        SubjectNodes RootNode = new SubjectNodes(CurrentItemCount);
        //        CurrentItemCount++;
        //        SubjectNodes.ItemCount = CurrentItemCount;
        //        RootNode.CI = "- ";

        //    }

            #endregion Public static Methods




        }//End class SubjectNode
}// End namespace LearningProject.DataModels