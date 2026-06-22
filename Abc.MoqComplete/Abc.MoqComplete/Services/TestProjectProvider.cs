using System.Collections.Concurrent;
using System.Linq;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Modules;

namespace Abc.MoqComplete.Services
{
    [SolutionComponent(Instantiation.DemandAnyThreadSafe)]
    public class TestProjectProvider : ITestProjectProvider
    {
        private readonly ConcurrentDictionary<string, bool> _isMoqContainedByProjectName = new ConcurrentDictionary<string, bool>();

        private static readonly string[] MoqReferenceNames =
        {
            "Moq",
            "Moq.AutoMock"
        };

        public bool IsTestProject(IPsiModule psiModule)
        {
            return _isMoqContainedByProjectName.GetOrAdd(psiModule.DisplayName, _ =>
            {
                var references = psiModule.GetPsiServices().Modules.GetModuleReferences(psiModule);
                return references.Any(r => MoqReferenceNames.Contains(r.Module.Name));
            });
        }
    }
}