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
            // Naming
            workflowAnalyzerConfigService.AddRule(LengthRule.Get());
            workflowAnalyzerConfigService.AddRule(ProjectDescriptionMissing.Get());
            //workflowAnalyzerConfigService.AddRule(VariableNameDuplicationRule.Get());
            //workflowAnalyzerConfigService.AddRule(ArgumentNamingRule.Get());
            //workflowAnalyzerConfigService.AddRule(VariableLengthRule.Get());
        }
    }
}
