[assembly: AssemblyCompany ("Xamarin.Interactive.CodeAnalysis")]
[assembly: AssemblyConfiguration ("Release")]
[assembly: AssemblyInformationalVersion ("1.0.0")]
[assembly: AssemblyProduct ("Xamarin.Interactive.CodeAnalysis")]
[assembly: AssemblyTitle ("Xamarin.Interactive.CodeAnalysis")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Client")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests")]
[assembly: InternalsVisibleTo ("Xamarin Workbooks")]
[assembly: InternalsVisibleTo ("Xamarin Inspector")]
[assembly: TargetFramework (".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
namespace Xamarin.Interactive.CodeAnalysis
{
    public enum CodeCellEvaluationStatus
    {
        Success,
        Disconnected,
        Interrupted,
        ErrorDiagnostic,
        EvaluationException
    }
    public sealed class EvaluationService : IEvaluationService, IDisposable
    {
        public bool CanEvaluate {
            get;
        }

        public IObservable<ICodeCellEvent> Events {
            get;
        }

        public EvaluationContextId Id {
            get;
        }

        public EvaluationService (IWorkspaceService workspace, IEvaluationEnvironment evaluationEnvironment);

        [AsyncStateMachine (typeof(EvaluationService.<AddTopLevelReferencesAsync>d__27))]
        public Task<bool> AddTopLevelReferencesAsync (IReadOnlyList<string> references, CancellationToken cancellationToken = default(CancellationToken));

        public void Dispose ();

        public Task EvaluateAllAsync (CancellationToken cancellationToken = default(CancellationToken));

        [AsyncStateMachine (typeof(EvaluationService.<EvaluateAsync>d__34))]
        public Task<CodeCellEvaluationFinishedEvent> EvaluateAsync (CodeCellId targetCodeCellId = default(CodeCellId), bool evaluateAll = false, CancellationToken cancellationToken = default(CancellationToken));

        public Task EvaluateAsync (string input, CancellationToken cancellationToken = default(CancellationToken));

        public IDisposable InhibitEvaluate ();

        public Task<CodeCellId> InsertCodeCellAsync (string initialBuffer = null, CodeCellId relativeToCodeCellId = default(CodeCellId), bool insertBefore = false, CancellationToken cancellationToken = default(CancellationToken));

        public Task LoadWorkbookDependencyAsync (string dependency, CancellationToken cancellationToken = default(CancellationToken));

        public void OutdateAllCodeCells ();

        public Task RemoveCodeCellAsync (CodeCellId codeCellId, CancellationToken cancellationToken = default(CancellationToken));

        [AsyncStateMachine (typeof(EvaluationService.<UpdateCodeCellAsync>d__31))]
        public Task<CodeCellUpdatedEvent> UpdateCodeCellAsync (CodeCellId codeCellId, string buffer, CancellationToken cancellationToken = default(CancellationToken));
    }
    public interface IWorkspaceService
    {
        WorkspaceConfiguration Configuration {
            get;
        }

        EvaluationContextId EvaluationContextId {
            get;
        }

        string GetCellBuffer (CodeCellId cellId);

        [@return: TupleElementNames (new string[] {
            "compilation",
            "diagnostics"
        })]
        Task<ValueTuple<Compilation, ImmutableList<Diagnostic>>> GetCellCompilationAsync (CodeCellId cellId, IEvaluationEnvironment evaluationEnvironment, CancellationToken cancellationToken = default(CancellationToken));

        Task<ImmutableList<Diagnostic>> GetCellDiagnosticsAsync (CodeCellId cellId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<CompletionItem>> GetCompletionsAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        ImmutableList<ExternalDependency> GetExternalDependencies ();

        Task<Hover> GetHoverAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        Task<SignatureHelp> GetSignatureHelpAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        ImmutableList<CodeCellId> GetTopologicallySortedCellIds ();

        CodeCellId InsertCell (string initialBuffer, CodeCellId previousCellId, CodeCellId nextCellId);

        bool IsCellComplete (CodeCellId cellId);

        bool IsCellOutdated (CodeCellId cellId);

        void RemoveCell (CodeCellId cellId, CodeCellId nextCellId);

        void SetCellBuffer (CodeCellId cellId, string buffer);
    }
    public interface IWorkspaceServiceActivator
    {
        Task<IWorkspaceService> CreateNew (LanguageDescription languageDescription, WorkspaceConfiguration configuration, CancellationToken cancellationToken);
    }
    public struct LanguageDescription
    {
        public static implicit operator LanguageDescription (string languageId);

        public string Name {
            get;
        }

        public string Version {
            get;
        }

        [JsonConstructor]
        public LanguageDescription (string name, string version = null);

        public override string ToString ();
    }
    public sealed class WorkspaceConfiguration
    {
        public TargetCompilationConfiguration CompilationConfiguration {
            get;
        }

        public InteractiveDependencyResolver DependencyResolver {
            get;
        }

        public Type HostObjectType {
            get;
        }

        public bool IncludePEImagesInDependencyResolution {
            get;
        }

        public WorkspaceConfiguration (TargetCompilationConfiguration compilationConfiguration, InteractiveDependencyResolver dependencyResolver, bool includePEImagesInDependencyResolution, Type hostObjectType = null);
    }
    [AttributeUsage (AttributeTargets.Assembly)]
    public sealed class WorkspaceServiceAttribute : Attribute
    {
        public string LanguageName {
            get;
        }

        public Type WorkspaceServiceActivatorType {
            get;
        }

        public WorkspaceServiceAttribute (string languageName, Type workspaceServiceActivatorType);
    }
    public static class WorkspaceServiceFactory
    {
    }
}
namespace Xamarin.Interactive.CodeAnalysis.Events
{
    public struct CodeCellEvaluationFinishedEvent : ICodeCellEvent
    {
        public CodeCellId CodeCellId {
            get;
        }

        public IReadOnlyList<Diagnostic> Diagnostics {
            get;
        }

        public bool ShouldStartNewCell {
            get;
        }

        public CodeCellEvaluationStatus Status {
            get;
        }

        public CodeCellEvaluationFinishedEvent (CodeCellId codeCellId, CodeCellEvaluationStatus status, bool shouldStartNewCell, IReadOnlyList<Diagnostic> diagnostics);
    }
    public struct CodeCellEvaluationStartedEvent : ICodeCellEvent
    {
        public CodeCellId CodeCellId {
            get;
        }

        public CodeCellEvaluationStartedEvent (CodeCellId codeCellId);
    }
    public sealed class CodeCellResultEvent : ICodeCellEvent
    {
        public CodeCellId CodeCellId {
            get;
        }

        public EvaluationResultHandling ResultHandling {
            get;
        }

        public IRepresentedType Type {
            get;
        }

        public IReadOnlyList<object> ValueRepresentations {
            get;
        }

        public CodeCellResultEvent (CodeCellId codeCellId, EvaluationResultHandling resultHandling, object value);
    }
    public struct CodeCellUpdatedEvent : ICodeCellEvent
    {
        public CodeCellId CodeCellId {
            get;
        }

        public IReadOnlyList<Diagnostic> Diagnostics {
            get;
        }

        public bool IsSubmissionComplete {
            get;
        }

        public CodeCellUpdatedEvent (CodeCellId codeCellId, bool isSubmissionComplete, IReadOnlyList<Diagnostic> diagnostics);
    }
}
namespace Xamarin.Interactive.CodeAnalysis.Models
{
    [MonacoSerializable ("monaco.languages.CompletionItem")]
    public struct CompletionItem
    {
        [JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
        public string Detail {
            get;
        }

        [JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
        public string InsertText {
            get;
        }

        public CompletionItemKind Kind {
            get;
        }

        public string Label {
            get;
        }

        [JsonConstructor]
        public CompletionItem (CompletionItemKind kind, string label, string insertText = null, string detail = null);
    }
    [MonacoSerializable ("monaco.languages.CompletionItemKind")]
    public enum CompletionItemKind
    {
        Text,
        Method,
        Function,
        Constructor,
        Field,
        Variable,
        Class,
        Interface,
        Module,
        Property,
        Unit,
        Value,
        Enum,
        Keyword,
        Snippet,
        Color,
        File,
        Reference
    }
    [InteractiveSerializable ("evaluation.Diagnostic", true)]
    public struct Diagnostic
    {
        public string Id {
            get;
        }

        public string Message {
            get;
        }

        public Range Range {
            get;
        }

        public DiagnosticSeverity Severity {
            get;
        }

        [JsonConstructor]
        public Diagnostic (Range range, DiagnosticSeverity severity, string message, string id);

        public Diagnostic (DiagnosticSeverity severity, string message);
    }
    [InteractiveSerializable ("evaluation.DiagnosticSeverity", true)]
    public enum DiagnosticSeverity
    {
        Hidden,
        Info,
        Warning,
        Error
    }
    [MonacoSerializable ("monaco.languages.Hover")]
    public struct Hover
    {
        public string[] Contents {
            get;
        }

        public Range Range {
            get;
        }

        [JsonConstructor]
        public Hover (Range range, string[] contents);
    }
    [MonacoSerializable ("monaco.languages.ParameterInformation")]
    public struct ParameterInformation
    {
        [JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
        public string Documentation {
            get;
        }

        public string Label {
            get;
        }

        [JsonConstructor]
        public ParameterInformation (string label, string documentation = null);
    }
    [MonacoSerializable ("monaco.IPosition")]
    public struct Position
    {
        public int Column {
            get;
        }

        public int LineNumber {
            get;
        }

        [JsonConstructor]
        public Position (int lineNumber, int column);

        public void Deconstruct (out int lineNumber, out int column);
    }
    [MonacoSerializable ("monaco.IRange")]
    public struct Range
    {
        public int EndColumn {
            get;
        }

        public int EndLineNumber {
            get;
        }

        public string FilePath {
            get;
        }

        public int StartColumn {
            get;
        }

        public int StartLineNumber {
            get;
        }

        [JsonConstructor]
        public Range (int startLineNumber, int startColumn, int endLineNumber, int endColumn, string filePath = null);

        public void Deconstruct (out int startLineNumber, out int startColumn);

        public void Deconstruct (out int startLineNumber, out int startColumn, out int endLineNumber, out int endColumn);
    }
    [MonacoSerializable ("monaco.languages.SignatureHelp")]
    public struct SignatureHelp
    {
        public int ActiveParameter {
            get;
        }

        public int ActiveSignature {
            get;
        }

        public IReadOnlyList<SignatureInformation> Signatures {
            get;
        }

        [JsonConstructor]
        public SignatureHelp (IReadOnlyList<SignatureInformation> signatures, int activeSignature = 0, int activeParameter = 0);
    }
    [MonacoSerializable ("monaco.languages.SignatureInformation")]
    public struct SignatureInformation
    {
        [JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
        public string Documentation {
            get;
        }

        public string Label {
            get;
        }

        public IReadOnlyList<ParameterInformation> Parameters {
            get;
        }

        [JsonConstructor]
        public SignatureInformation (string label, IReadOnlyList<ParameterInformation> parameters = null, string documentation = null);
    }
}
namespace Xamarin.Interactive.CodeAnalysis.Resolving
{
    public class DependencyResolver
    {
        public FilePath BaseDirectory {
            get;
            set;
        }

        public DependencyResolver ();

        public DependencyResolver AddAssemblySearchPath (FilePath searchPath, bool scanRecursively = false);

        public static ImmutableArray<FilePath> EnumerateAssembliesInDirectory (FilePath directory, bool scanRecursively, CancellationToken cancellationToken);

        public DependencyResolver RemoveAssemblySearchPath (FilePath searchPath);

        public ImmutableArray<ResolvedAssembly> Resolve (IEnumerable<AssemblyDefinition> assemblies, ResolveOperationOptions resolveOptions = ResolveOperationOptions.ResolveReferences, bool resolveByNameFallback = true, CancellationToken cancellationToken = default(CancellationToken));

        public ImmutableArray<ResolvedAssembly> Resolve (IEnumerable<AssemblyName> assemblyNames, ResolveOperationOptions resolveOptions = ResolveOperationOptions.ResolveReferences, CancellationToken cancellationToken = default(CancellationToken));

        public ImmutableArray<ResolvedAssembly> Resolve (IEnumerable<FilePath> assemblyPaths, ResolveOperationOptions resolveOptions = ResolveOperationOptions.ResolveReferences, CancellationToken cancellationToken = default(CancellationToken));

        public ResolvedAssembly ResolveWithoutReferences (AssemblyName assemblyName, CancellationToken cancellationToken = default(CancellationToken));

        public ResolvedAssembly ResolveWithoutReferences (FilePath assemblyPath, CancellationToken cancellationToken = default(CancellationToken));
    }
    public class ExternalDependency
    {
        public FilePath Location {
            get;
        }

        public ExternalDependency (FilePath location);
    }
    public static class GacCache
    {
        public static IReadOnlyList<string> GacPaths {
            get;
        }

        public static Task InitializingTask {
            get;
            private set;
        }

        public static bool TryGetCachedAssemblyName (FilePath key, out AssemblyName assemblyName);

        public static bool TryGetCachedFilePaths (FilePath key, out ImmutableArray<FilePath> files);

        public static bool TryGetCachedResolvedAssembly (AssemblyName assemblyName, out ResolvedAssembly resolvedAssembly);
    }
    public class InteractiveDependencyResolver : NativeDependencyResolver
    {
        public void AddDefaultReferences (IEnumerable<AssemblyDefinition> defaultReferences);

        public string CacheRemoteAssembly (AssemblyDefinition remoteAssembly);

        public static byte[] GetDebugSymbolsFromAssemblyPath (FilePath path);

        public static byte[] GetFileBytes (FilePath path);

        public ImmutableArray<ResolvedAssembly> ResolveDefaultReferences ();

        public Task<AssemblyDefinition[]> ResolveReferencesAsync (IEnumerable<FilePath> references, bool includePeImages, CancellationToken cancellationToken);
    }
    public class NativeDependency : ExternalDependency
    {
        public string Name {
            get;
        }

        public NativeDependency (string name, FilePath location);
    }
    public class NativeDependencyResolver : DependencyResolver
    {
    }
    public sealed class ResolvedAssembly : IEquatable<ResolvedAssembly>
    {
        public sealed class NameEqualityComparer : IEqualityComparer<ResolvedAssembly>, IEqualityComparer<AssemblyName>
        {
            public static readonly ResolvedAssembly.NameEqualityComparer Default;

            public NameEqualityComparer ();

            public bool Equals (AssemblyName x, AssemblyName y);

            public bool Equals (ResolvedAssembly x, ResolvedAssembly y);

            public int GetHashCode (AssemblyName obj);

            public int GetHashCode (ResolvedAssembly obj);
        }

        public AssemblyName AssemblyName {
            get;
        }

        public ImmutableArray<ExternalDependency> ExternalDependencies {
            get;
        }

        public bool HasIntegration {
            get;
        }

        public FilePath Path {
            get;
        }

        public ImmutableHashSet<AssemblyName> References {
            get;
        }

        public ImmutableHashSet<ResolvedAssembly> ResolvedReferences {
            get;
        }

        public static ResolvedAssembly Create (string path, AssemblyName assemblyName, IEnumerable<AssemblyName> references = null, IEnumerable<ResolvedAssembly> resolvedReferences = null, IEnumerable<ExternalDependency> externalDependencies = null, bool hasIntegration = false);

        public void Dump (TextWriter writer);

        public bool Equals (ResolvedAssembly obj);

        public override bool Equals (object obj);

        public override int GetHashCode ();

        public ResolvedAssembly With (bool? hasIntegration = null, IEnumerable<AssemblyName> references = null, IEnumerable<ResolvedAssembly> resolvedReferences = null, IEnumerable<ExternalDependency> externalDependencies = null);

        public ResolvedAssembly WithExternalDependencies (IEnumerable<ExternalDependency> externalDependencies);

        public ResolvedAssembly WithHasIntegration (bool hasIntegration);

        public ResolvedAssembly WithReferences (IEnumerable<AssemblyName> references);

        public ResolvedAssembly WithResolvedReferences (IEnumerable<ResolvedAssembly> resolvedReferences);
    }
    [Flags]
    public enum ResolveOperationOptions
    {
        None = 0,
        ResolveReferences = 1,
        SkipGacCache = 2
    }
}