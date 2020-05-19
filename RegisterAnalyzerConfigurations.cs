using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.Analyzer;

namespace WA_UiPath
{
    public class RegisterAnalyzerConfiguration : IRegisterAnalyzerConfiguration
    {
        public void Initialize(IAnalyzerConfigurationService workflowAnalyzerConfigService)
        {
            workflowAnalyzerConfigService.AddRule(ProjectDescriptionMissing.Get());
            workflowAnalyzerConfigService.AddRule(REFrameworkUsageRule.Get()); ;
            workflowAnalyzerConfigService.AddRule(AtleastOneLogMessageRule.Get());

        }
    }
}
