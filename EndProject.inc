<Project DefaultTargets="globalInclude" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	
	<Import Project=".\gitresource.targets" />
	<ItemGroup>
		<Compile Include="$(ToRoot)\version.cs" />
	</ItemGroup>
	
	<Target Name="BeforeBuild" DependsOnTargets="_GitVersionValue" />
</Project>
