using tfsinterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterfaceUT
{
    
    
    /// <summary>
    ///This is a test class for UtilsTest and is intended
    ///to contain all UtilsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class UtilsTest
		{


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
		///A test for IsMergeChangeset
		///</summary>
		[TestMethod()]
		public void IsMergeChangesetTest()
		{
			Changeset cs = null; // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = Utils.IsMergeChangeset(cs);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetPathPart
		///</summary>
		[TestMethod()]
		public void GetPathPartTest()
		{
			string path = "";
			string expected = string.Empty; // TODO: Initialize to an appropriate value
			string actual;
			actual = Utils.GetPathPart(path);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for GetEGSBranch
		///</summary>
		[TestMethod()]
		public void GetEGSBranch_FullFile()
		{
			string fullPath = "$/IGT_0803/development/dev_adv_cr/EGS/shared/lib/win32/PinUtil.dll";
			string expected = "$/IGT_0803/development/dev_adv_cr";
			string actual = Utils.GetEGSBranch(fullPath);
			
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetEGSBranch_FullFolder()
		{
			string fullPath = "$/IGT_0803/development/dev_adv_cr/EGS/shared/";
			string expected = "$/IGT_0803/development/dev_adv_cr";
			string actual = Utils.GetEGSBranch(fullPath);
			
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetEGSBranch_EGSFolder()
		{
			string fullPath = "$/IGT_0803/development/dev_adv_cr/EGS/";
			string expected = "$/IGT_0803/development/dev_adv_cr";
			string actual = Utils.GetEGSBranch(fullPath);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetEGSBranch_ReleaseTest()
		{
			string fullPath = "$/IGT_0803/release/EGS8.2/dev_sp/EGS/shared/lib/win32/PinUtil.dll";
			string expected = "$/IGT_0803/release/EGS8.2/dev_sp";
			string actual = Utils.GetEGSBranch(fullPath);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetEGSBranch_BadTest()
		{
			string fullPath = "$/IGT_0803/release/EGS8.2/dev_sp/EGS";
			string expected = string.Empty;
			string actual = Utils.GetEGSBranch(fullPath);

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void testRE()
		{
			System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("^(.+)/EGS(/?|/.*)$");
			
			System.Text.RegularExpressions.Match m = re.Match("foo/bar/baz");
			Assert.IsFalse(m.Success);
			
			m = re.Match("foo/bar/EGS/flarg");
			Assert.IsNotNull(m);
			Assert.IsTrue(m.Success);
			Assert.AreEqual<string>("foo/bar", m.Groups[1].Value);
			Assert.AreEqual<string>("/flarg", m.Groups[2].Value);
			
			m = re.Match("foo/bar/EGS");
			Assert.IsTrue(m.Success);
			Assert.AreEqual<string>("foo/bar", m.Groups[1].Value);
			Assert.AreEqual<string>(string.Empty, m.Groups[2].Value);
			
			m = re.Match("foo/var/EGS/");
			Assert.IsTrue(m.Success);
			Assert.AreEqual<string>("foo/var", m.Groups[1].Value);
			Assert.AreEqual<string>("/", m.Groups[2].Value);
		}
	}
}
