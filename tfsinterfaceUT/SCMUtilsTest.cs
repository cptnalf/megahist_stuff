using tfsinterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TeamFoundation.VersionControl.Client;
using treelib;

namespace tfsinterfaceUT
{
    
    
    /// <summary>
    ///This is a test class for SCMUtilsTest and is intended
    ///to contain all SCMUtilsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class SCMUtilsTest
	{
		private string _tfsServer = "rnotfsat";

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
			{
			get
				{
				return testContextInstance;
				}
			set
				{
				testContextInstance = value;
				}
			}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for GetTFSServer
		///</summary>
		[TestMethod()]
		public void GetTFSServerTest()
		{
			VersionControlServer actual;
			actual = SCMUtils.GetTFSServer(_tfsServer);
			
			Assert.IsNotNull(actual);
			Assert.AreEqual<string>(_tfsServer, actual.TeamFoundationServer.Name);
		}

		/// <summary>
		///A test for GetBranches
		///</summary>
		[TestMethod()]
		public void GetBranchesTest()
		{
			VersionControlServer vcs = SCMUtils.GetTFSServer(_tfsServer); 
			string srcPath = "$/IGT_0803/main/EGS/";
			VersionSpec ver = VersionSpec.Latest;
			TreapDict<int, Item> expected = null;
			TreapDict<int, Item> actual;
			actual = SCMUtils.GetBranches(vcs, srcPath, ver);
			
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.size() > 0);
		}
	}
}
