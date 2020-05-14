using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;
using Newtonsoft.Json;
using System.IO;

namespace WA_UiPath
{
    // This static class is not mandatory. It just helps organizining the code.
    static class ProjectDescriptionMissing
    {
        class ProjectModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }


        // This should be as unique as possible, and should follow the naming convention.
        private const string RuleId = "ST-NMG-007";

        internal static Rule<IProjectModel> Get()
        {
            var rule = new Rule<IProjectModel>("Project Description Missing Custom Rule", RuleId, Inspect)
            {
                RecommendationMessage = "Projects should have meaningful names and descriptions. Avoid using defaults.",//Recommendation,
                /// Off and Verbose are not supported.
                ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                ApplicableScopes = new List<string> { RuleConstants.BusinessRuleConstant, RuleConstants.DevelopmentRuleConstant, RuleConstants.TestAutomationRuleConstant }
            };
            return rule;
        }

        private static InspectionResult Inspect(IProjectModel fileModel, Rule ruleInstance)
        {
            var messageList = new List<string>();


            using (StreamReader r = new StreamReader(fileModel.ProjectFilePath))
            {
                string json = r.ReadToEnd();
                ProjectModel items = JsonConvert.DeserializeObject<ProjectModel>(json);

                if (string.IsNullOrEmpty(items.Description) || items.Description.Equals("Blank Process") || string.IsNullOrEmpty(items.Name) || items.Name.Equals("BlankProcess"))
                {
                    messageList.Add($"Project has no or default description.");
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
