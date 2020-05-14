using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace WA_UiPath
{
    // This static class is not mandatory. It just helps organizining the code.
     static class LengthRule
    {
        // This should be as unique as possible, and should follow the naming convention.
        private const string RuleId = "AT-NMG-001";

        internal static Rule<IActivityModel> Get()
        {
            var rule = new Rule<IActivityModel>("Variable Length Custom Rule", RuleId, Inspect)
            { 
                RecommendationMessage = "Variable length can't be greater than 40",//Recommendation,
                /// Off and Verbose are not supported.
                ErrorLevel = System.Diagnostics.TraceLevel.Error,
                ApplicableScopes= new List<string> { RuleConstants.BusinessRuleConstant,RuleConstants.DevelopmentRuleConstant,RuleConstants.TestAutomationRuleConstant }
            };
            return rule;
        }

        // This is the function that executes for each activity in all the files. Might impact performance.
        // The rule instance is the rule provided above which also contains the user-configured data.
        private static InspectionResult Inspect(IActivityModel activityModel, Rule ruleInstance)
        {
            var messageList = new List<string>();
            foreach (var activityModelVariable in activityModel.Variables)
            {
                if (activityModelVariable.DisplayName.Length > 40)
                {
                    messageList.Add($"The variable {activityModelVariable.DisplayName} has a length longer than 40");
                }
            }

            if (messageList.Count > 0)
            {
                return new InspectionResult()
                {
                    ErrorLevel = ruleInstance.ErrorLevel,
                    HasErrors = true,
                    RecommendationMessage = ruleInstance.RecommendationMessage,
                    // When inspecting a model, a rule can generate more than one message.
                    Messages = messageList
                };
            }
            else
            {
                return new InspectionResult() { HasErrors = false };
            }
        }
    }
}
