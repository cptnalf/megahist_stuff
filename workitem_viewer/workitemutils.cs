
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

/*
 * use 'WorkItem' as the stuff that's shown in the listview
 * tie the link-count to children.
 * 
 * use the worker-stuff i've already written to do background work
 * 
 * 
 * what we'll do here is tree the items we're given.
 * eg:
 * query foo returns all of the tasks/scenarios/bugs against ez pay mag card
 * it'll find the scenarios first, then add children for those
 */

namespace workitem_viewer
{
	internal class WorkItemThingy
	{
		private TeamFoundationServer _server;
		private WorkItemStore _store;
		
		internal WorkItemThingy(string srvr, string query)
		{
			_server = TeamFoundationServerFactory.GetServer(srvr);
			//_query = query;
			
			_store = _server.GetService(typeof(WorkItemStore)) as WorkItemStore;
		}
		
		internal Dictionary<string,List<StoredQuery>> getQueries()
		{
			WorkItemStore store;
			Dictionary<string, List<StoredQuery>> queries = new Dictionary<string, List<StoredQuery>>();
			
			foreach(Project proj in _store.Projects)
				{
					List<StoredQuery> lsq = new List<StoredQuery>();
					foreach(StoredQuery sq in proj.StoredQueries)
						{
							lsq.Add(sq);
						}
					queries.Add(proj.Name, lsq);
				}
			
			return queries;
		}
		
		//internal List<TaskItem> runQuery(string query)
		//{
		//  WorkItemCollection items = _store.Query(query);
			
		//}
		
		/*
			id;name;value;fielddef.systemtype;fielddef.name;fielddef.fieldtype
			1;Title;ACX - EZPaySmartCardRequesterPlugin. change all exception to call XIFBase.Application.Globals.Logging.GetDetailedExceptionMessage(e) instead  of using e.message;System.String;Title;String
			2;State;Resolved;System.String;State;String
			8;Rev;4;System.Int32;Rev;Integer
			9;Changed By;Burden.John;System.String;Changed By;String
			22;Reason;Fixed;System.String;Reason;String
			24;Assigned To;Burden.John;System.String;Assigned To;String
			25;Work Item Type;Enhancement;System.String;Work Item Type;String
			32;Created Date;04/10/2008 09:23:15;System.DateTime;Created Date;DateTime
			33;Created By;Burden.John;System.String;Created By;String
			10031;Issue;No;System.String;Issue;String
			10032;State Change Date;04/17/2008 08:26:29;System.DateTime;State Change Date;DateTime
			10033;Activated Date;04/10/2008 09:23:15;System.DateTime;Activated Date;DateTime
			52;Description;In the module EZPaySmartCardRequesterPlugin all of the caught exceptions are just using e.message for logging which provides error messages that are not very descriptive or helpful (for example, “Object reference not set to an instance of an object”).
			
			change all calls to XIFBase.Application.Globals.Logging.GetDetailedExceptionMessage(e) in place of e.message.;System.String;Description;PlainText
			10037;Resolved Reason;New;System.String;Resolved Reason;String
			54;History;Associated with changeset 137939.;System.String;History;History
			10039;Closed By;;System.String;Closed By;String
			10038;Closed Date;;System.DateTime;Closed Date;DateTime
			10040;Priority;999;System.Int32;Priority;Integer
			10041;Triage;;System.String;Triage;String
			10044;ItemStatus;Submitted;System.String;ItemStatus;String
			10043;ItemSeverity;1-Low;System.String;ItemSeverity;String
			10046;PropertyRequesting;Internal;System.String;PropertyRequesting;String
			10047;Estimated Start Date;;System.DateTime;Estimated Start Date;DateTime
			10045;UTTech;;System.String;UTTech;String
			10036;Resolved By;Burden.John;System.String;Resolved By;String
			10042;Rank;;System.String;Rank;String
			10034;Activated By;Burden.John;System.String;Activated By;String
			10035;Resolved Date;04/17/2008 08:26:29;System.DateTime;Resolved Date;DateTime
			10048;Test Name;;System.String;Test Name;String
			10054;Completed Work;0;System.Double;Completed Work;Double
			10053;Remaining Work;0;System.Double;Remaining Work;Double
			10056;Start Date;;System.DateTime;Start Date;DateTime
			10057;Finish Date;;System.DateTime;Finish Date;DateTime
			10058;MSProject;;System.String;MSProject;String
			75;RelatedLinkCount;0;System.Int32;RelatedLinkCount;Integer
			10060;Discipline;Development;System.String;Discipline;String
			10055;Baseline Work;;System.Double;Baseline Work;Double
			10062;Task Hierarchy;;System.String;Task Hierarchy;String
			10063;Scheduled Fix In Release;TBD;System.String;Scheduled Fix In Release;String
			10050;Test Path;;System.String;Test Path;String
			10049;Test Id;;System.String;Test Id;String
			10052;Integration Build;;System.String;Integration Build;String
			10051;Found In;;System.String;Found In;String
			10068;Manager;CCB;System.String;Manager;String
			10076;ServicePack;<None>;System.String;ServicePack;String
			10078;ProjectDuration;0;System.Double;ProjectDuration;Double
			-105;Iteration Path;Advantage;System.String;Iteration Path;TreePath
			-104;IterationID;82;System.Int32;IterationID;Integer
			-57;ExternalLinkCount;1;System.Int32;ExternalLinkCount;Integer
			-42;Team Project;Advantage;System.String;Team Project;String
			-32;HyperLinkCount;0;System.Int32;HyperLinkCount;Integer
			-31;AttachedFileCount;0;System.Int32;AttachedFileCount;Integer
			-12;Node Name;ACX;System.String;Node Name;String
			-7;Area Path;Advantage\ACX;System.String;Area Path;TreePath
			-5;Revised Date;01/01/9999 00:00:00;System.DateTime;Revised Date;DateTime
			-4;Changed Date;04/17/2008 08:26:29;System.DateTime;Changed Date;DateTime
			-3;ID;3670;System.Int32;ID;Integer
			-2;AreaID;130;System.Int32;AreaID;Integer
			-1;Authorized As;Burden.John;System.String;Authorized As;String		
			
			/*
			<FORM><Layout><Group><Column PercentWidth="70"><Control FieldName="System.Title" Type="FieldControl" Label="&amp;Title:" LabelPosition="Left" /></Column><Column PercentWidth="30"><Control FieldName="Microsoft.VSTS.Common.Discipline" Type="FieldControl" Label="&amp;Discipline:" LabelPosition="Left" /></Column></Group><Group><Column PercentWidth="100"><Group Label="Classification"><Column PercentWidth="100"><Control FieldName="System.AreaPath" Type="WorkItemClassificationControl" Label="&amp;Area:" LabelPosition="Left" /><Control FieldName="System.IterationPath" Type="WorkItemClassificationControl" Label="&amp;Iteration:" LabelPosition="Left" /></Column></Group></Column></Group><Group Label="Status"><Column PercentWidth="33"><Control FieldName="System.AssignedTo" Type="FieldControl" Label="Assi&amp;gned to:" LabelPosition="Left" /><Control FieldName="IGT.Common.Manager" Type="FieldControl" Label="Manager:" LabelPosition="Left" /><Control FieldName="System.State" Type="FieldControl" Label="&amp;State:" LabelPosition="Left" /><Control FieldName="IGT.Common.ItemStatus" Type="FieldControl" Label="Status:" LabelPosition="Left" /></Column><Column PercentWidth="33"><Control FieldName="IGT.Common.ItemSeverity" Type="FieldControl" Label="Severity:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Common.Priority" Type="FieldControl" Label="Priorit&amp;y:" LabelPosition="Left" /><Control FieldName="IGT.Common.PropertyRequesting" Type="FieldControl" Label="Property:" LabelPosition="Left" /><Control FieldName="System.Reason" Type="FieldControl" Label="Reason:" LabelPosition="Left" /></Column><Column PercentWidth="33"><Control FieldName="IGT.Common.EstStartDate" Type="FieldControl" Label="Est Start Date:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Common.ClosedDate" Type="FieldControl" Label="Closed Date:" LabelPosition="Left" ReadOnly="True" /></Column></Group><TabGroup><Tab Label="Description"><Control FieldName="System.Description" Type="HtmlFieldControl" Label="Des&amp;cription:" LabelPosition="Top" Dock="Fill" /></Tab><Tab Label="History"><Control FieldName="System.History" Type="WorkItemLogControl" Label="&amp;History:" LabelPosition="Top" Dock="Fill" /></Tab><Tab Label="Links"><Control Type="LinksControl" LabelPosition="Top" /></Tab><Tab Label="File Attachments"><Control Type="AttachmentsControl" LabelPosition="Top" /></Tab><Tab Label="Details"><Group><Column PercentWidth="50"><Group Label="Build Information"><Column PercentWidth="100"><Control FieldName="IGT.Common.ScheduledFixInRelease" Type="FieldControl" Label="Scheduled For Release:" LabelPosition="Left" /><Control FieldName="IGT.Common.ServicePack" Type="FieldControl" Label="Service Pack:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Build.IntegrationBuild" Type="FieldControl" Label="Resolved in build:" LabelPosition="Left" /></Column></Group></Column><Column PercentWidth="50"><Group Label="UT Test"><Column PercentWidth="100"><Control FieldName="IGT.Common.UTTech" Type="FieldControl" Label="UT Tech:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Test.TestName" Type="FieldControl" Label="Test Name:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Test.TestId" Type="FieldControl" Label="Test ID:" LabelPosition="Left" /><Control FieldName="Microsoft.VSTS.Test.TestPath" Type="FieldControl" Label="Test Path:" LabelPosition="Left" /></Column></Group></Column></Group></Tab><Tab Label="Project Info"><Group><Column PercentWidth="50"><Group Label="MS Project Information"><Column PercentWidth="100"><Control FieldName="Microsoft.VSTS.Common.MSProject" Type="FieldControl" Label="Associated Project:" LabelPosition="Left" ReadOnly="True" /><Control FieldName="Microsoft.VSTS.Scheduling.TaskHierarchy" Type="FieldControl" Label="Task Context:" LabelPosition="Left" ReadOnly="True" /><Control FieldName="Microsoft.VSTS.Scheduling.StartDate" Type="FieldControl" Label="Start Dat&amp;e:" LabelPosition="Left" ReadOnly="True" /><Control FieldName="Microsoft.VSTS.Scheduling.FinishDate" Type="FieldControl" Label="&amp;Finish Date:" LabelPosition="Left" ReadOnly="True" /><Control FieldName="IGT.Common.ProjectDuration" Type="FieldControl" Label="Est. Project Duration in Hours:" LabelPosition="Left" /></Column></Group></Column><Column PercentWidth="50"><Group Label="Schedule"><Column PercentWidth="100"><Control Type="WitIgtHoursClass" FieldName="Microsoft.VSTS.Scheduling.RemainingWork" Label="" LabelPosition="Left" /></Column></Group></Column></Group></Tab></TabGroup></Layout></FORM>
				*/
	}
}
