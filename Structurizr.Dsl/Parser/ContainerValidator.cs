namespace Structurizr.DslReader.Parser;

public static class ContainerValidator
{
  private static readonly string[] _prefix = { "bk_", "ui_", "wf_" };
  private static readonly string[] _backendTech = { ".NET", "Java", "C++" };
  private static readonly string[] _frontendTech = { "" };
  private const string WorkflowTag = "workflow";

  public static void Validate(Container container, ContextualWorkspace contextualWorkspace, int lineNumber, DirectoryInfo directoryInfo)
  {
    if (IsBackend(container))
    {
      AssertTechnology(container, contextualWorkspace, lineNumber, directoryInfo, _backendTech);
    }
    else if (IsFrontend(container))
    {
      AssertTechnology(container, contextualWorkspace, lineNumber, directoryInfo, _frontendTech);
    }
    else if (IsWorkflow(container))
    {
      var tag = container.GetAllTags().FirstOrDefault(t => string.Compare(t, WorkflowTag, true) == 0);
      if (tag == null)
        container.AddTags(WorkflowTag);
    }
    else
    {
      contextualWorkspace.AddNamingConventionError(directoryInfo.Name, lineNumber, $"Container prefix MUST be {string.Join(" or ", _prefix)} ({container.Id})");
    }

    container.AddTags(container.Technology);
  }

  private static bool IsBackend(Container container) => container.Id.StartsWith(_prefix[0]);
  private static bool IsWorkflow(Container container) => container.Id.StartsWith(_prefix[2]);
  private static bool IsFrontend(Container container) => container.Id.StartsWith(_prefix[1]);

  private static void AssertTechnology(Container container, ContextualWorkspace contextualWorkspace, int lineNumber, DirectoryInfo directoryInfo, IEnumerable<string> technology)
  {
    if(string.IsNullOrWhiteSpace(container.Technology))
      throw new Exception($"Container {container.Name} should have one technology");
  }
}