using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace WA_UiPath
{
    // This static class is not mandatory. It just helps organizining the code.
    internal static class REFrameworkUsageRule
    {
        // This should be as unique as possible, and should follow the naming convention.
        private const string RuleId = "ST-USG-021";

        internal static Rule<IProjectModel> Get()
        {
            var rule = new Rule<IProjectModel>("ReFramework Usage", RuleId, Inspect)
            {
                RecommendationMessage = "The Robotic Enterprise Framework (ReFramework) is a well structured and tested template that should be used for developing new projects.",//Recommendation,
                /// Off and Verbose are not supported.
                ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                ApplicableScopes= new List<string> { RuleConstants.DevelopmentRuleConstant }
            };
            return rule;
        }

        // This is the function that executes for each activity in all the files. Might impact performance.
        // The rule instance is the rule provided above which also contains the user-configured data.
        private static InspectionResult Inspect(IProjectModel projectModel, Rule ruleInstance)
        {            
            var messageList = new List<string>();
            string jsonFilePath=projectModel.ProjectFilePath;
            string directoryPath = jsonFilePath.ToLower().Replace("project.json", "");
            string[] allDirectories=Directory.GetDirectories(directoryPath);
            allDirectories = allDirectories.Select(a => a.Substring(a.LastIndexOf('\\') + 1)).ToArray();
            bool isSetTransactionStatusFileExist = false;
            //if(allDirectories.Any(dr => dr.ToLower() == "framework"))
            //{
                //messageList.Add("This project does not use REFramework.---DPath:"+directoryPath+ "\framework");
                //var allFrameworkFiles=Directory.GetFiles(directoryPath + "\framework");
                //isSetTransactionStatusFileExist = allFrameworkFiles.Any(a => a.ToLower().Contains("settransactionstatus"));
            //}

            if ((!allDirectories.Any(dr => dr.ToLower() == "framework")|| !isSetTransactionStatusFileExist)  && projectModel.ProjectOutputType.ToLower()=="process")
            {
                messageList.Add("This project does not use REFramework.");
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
