<Project DefaultTargets="globalInclude" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	
	<Import Project="$(MSBuildProjectDirectory)\gitbuild.inc" />
	<PropertyGroup>
		<BaseIntermediateOutputPath>$(ToRoot)\obj\$(MSBuildProjectName)\</BaseIntermediateOutputPath>
	</PropertyGroup>
	
	<!-- standard EGS properties. -->
	<PropertyGroup>
		<EGSTOOLSPATH>$(ToRoot)\libs</EGSTOOLSPATH>
		<EGSINCPATH>$(ToRoot)</EGSINCPATH>
	</PropertyGroup>
</Project>
