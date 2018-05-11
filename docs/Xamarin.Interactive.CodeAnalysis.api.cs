[assembly: AssemblyConfiguration ("Release")]
[assembly: AssemblyCopyright ("Copyright 2016-2018 Microsoft. All rights reserved.\nCopyright 2014-2016 Xamarin Inc. All rights reserved.")]
[assembly: AssemblyProduct ("Xamarin.Interactive.CodeAnalysis")]
[assembly: AssemblyTitle ("Xamarin.Interactive.CodeAnalysis")]
[assembly: InternalsVisibleTo ("Workbook Mac App (Desktop Profile)")]
[assembly: InternalsVisibleTo ("Workbook Mac App (Mobile Profile)")]
[assembly: InternalsVisibleTo ("workbook")]
[assembly: InternalsVisibleTo ("workbooks-server")]
[assembly: InternalsVisibleTo ("Xamarin Inspector")]
[assembly: InternalsVisibleTo ("Xamarin Workbooks Agent (WPF)")]
[assembly: InternalsVisibleTo ("Xamarin Workbooks")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Client")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Client.Console")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Client.Desktop")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.CodeAnalysis")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.CodeAnalysis.Tests")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Console")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.DotNetCore")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Forms")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Forms.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Forms.iOS")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.iOS")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Mac.Desktop")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Mac.Mobile")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Telemetry.Server")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests.Core")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests.Mac")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Tests.Windows")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.VS")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.Wpf")]
[assembly: InternalsVisibleTo ("Xamarin.Interactive.XS")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.Client.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.Client.iOS")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.DotNetCore")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.Forms.Android")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.Forms.iOS")]
[assembly: InternalsVisibleTo ("Xamarin.Workbooks.iOS")]
[assembly: InternalsVisibleTo ("xic")]
[assembly: TargetFramework (".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
namespace Xamarin.Interactive.CodeAnalysis
{
    public interface IWorkspaceService
    {
        WorkspaceConfiguration Configuration {
            get;
        }

        Task<Compilation> EmitCellCompilationAsync (CodeCellId cellId, EvaluationEnvironment evaluationEnvironment, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetCellBufferAsync (CodeCellId cellId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<Diagnostic>> GetCellDiagnosticsAsync (CodeCellId cellId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<CompletionItem>> GetCompletionsAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<ExternalDependency> GetExternalDependencies ();

        Task<Hover> GetHoverAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        Task<SignatureHelp> GetSignatureHelpAsync (CodeCellId cellId, Position position, CancellationToken cancellationToken = default(CancellationToken));

        IReadOnlyList<CodeCellId> GetTopologicallySortedCellIds ();

        CodeCellId InsertCell (CodeCellId previousCellId, CodeCellId nextCellId);

        bool IsCellComplete (CodeCellId cellId);

        Task<bool> IsCellOutdatedAsync (CodeCellId cellId, CancellationToken cancellationToken = default(CancellationToken));

        void RemoveCell (CodeCellId cellId, CodeCellId nextCellId);

        void SetCellBuffer (CodeCellId cellId, string buffer);
    }
    public interface IWorkspaceServiceActivator
    {
        Task<IWorkspaceService> CreateNew (LanguageDescription languageDescription, WorkspaceConfiguration configuration, CancellationToken cancellationToken);
    }
    public static class IWorkspaceServiceExtensions
    {
        public static CodeCellId InsertCell (this IWorkspaceService workspaceService, string initialBuffer, CodeCellId previousCellId, CodeCellId nextCellId);
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

        public WorkspaceConfiguration (TargetCompilationConfiguration compilationConfiguration, InteractiveDependencyResolver dependencyResolver);

        public static Task<WorkspaceConfiguration> CreateAsync (IEvaluationContextManager evaluationContextManager, CancellationToken cancellationToken = default(CancellationToken));
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
        public static async Task<IWorkspaceService> CreateWorkspaceServiceAsync (LanguageDescription languageDescription, WorkspaceConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken));
    }
}
namespace Xamarin.Interactive.CodeAnalysis.Evaluating
{
    public sealed class EvaluationService : IEvaluationService, IDisposable
    {
        public bool CanEvaluate {
            get;
        }

        public IObservable<ICodeCellEvent> Events {
            get;
        }

        public TargetCompilationConfiguration TargetCompilationConfiguration {
            get;
        }

        public EvaluationService (IWorkspaceService workspace, EvaluationEnvironment evaluationEnvironment, IEvaluationContextManager evaluationContextManager = null);

        public Task<bool> AddTopLevelReferencesAsync (IReadOnlyList<string> references, CancellationToken cancellationToken = default(CancellationToken));

        public void Dispose ();

        public Task EvaluateAllAsync (CancellationToken cancellationToken = default(CancellationToken));

        public Task<CodeCellEvaluationFinishedEvent> EvaluateAsync (CodeCellId targetCodeCellId = default(CodeCellId), bool evaluateAll = false, CancellationToken cancellationToken = default(CancellationToken));

        public IDisposable InhibitEvaluate ();

        public Task<CodeCellId> InsertCodeCellAsync (string initialBuffer = null, CodeCellId relativeToCodeCellId = default(CodeCellId), bool insertBefore = false, CancellationToken cancellationToken = default(CancellationToken));

        public void NotifyEvaluationComplete (CodeCellId targetCodeCellId, EvaluationStatus status);

        public void NotifyEvaluationContextManagerChanged (IEvaluationContextManager evaluationContextManager);

        public void OutdateAllCodeCells ();

        public Task RemoveCodeCellAsync (CodeCellId codeCellId, CancellationToken cancellationToken = default(CancellationToken));

        public Task<CodeCellUpdatedEvent> UpdateCodeCellAsync (CodeCellId codeCellId, string buffer, CancellationToken cancellationToken = default(CancellationToken));
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

        public EvaluationStatus Status {
            get;
        }

        public CodeCellEvaluationFinishedEvent (CodeCellId codeCellId, EvaluationStatus status, bool shouldStartNewCell, IReadOnlyList<Diagnostic> diagnostics);
    }
    public struct CodeCellEvaluationStartedEvent : ICodeCellEvent
    {
        public CodeCellId CodeCellId {
            get;
        }

        public CodeCellEvaluationStartedEvent (CodeCellId codeCellId);
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
        public IReadOnlyList<string> Contents {
            get;
        }

        public Range Range {
            get;
        }

        [JsonConstructor]
        public Hover (Range range, IReadOnlyList<string> contents);
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
        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public int Column {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
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
        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public int EndColumn {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public int EndLineNumber {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public string FilePath {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public int StartColumn {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
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
        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
        public int ActiveParameter {
            get;
        }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Include)]
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
namespace Xamarin.Interactive.NuGet
{
    public sealed class InteractivePackageDescription
    {
        public string IdentityVersion {
            get;
        }

        public bool IsExplicitlySelected {
            get;
        }

        public string PackageId {
            get;
        }

        public InteractivePackageSource Source {
            get;
        }

        public string VersionRange {
            get;
        }

        [JsonConstructor]
        public InteractivePackageDescription (string packageId, string identityVersion = null, string versionRange = null, bool isExplicitlySelected = true, InteractivePackageSource source = null);

        public SourceRepository GetSourceRepository ();
    }
    public sealed class InteractivePackageSource
    {
        public string Source {
            get;
        }

        [JsonConstructor]
        public InteractivePackageSource (string source);

        public static InteractivePackageSource FromPackageSource (PackageSource packageSource);

        public PackageSource ToPackageSource ();
    }
}
