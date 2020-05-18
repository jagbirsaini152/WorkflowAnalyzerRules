using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace WA_UiPath
{    
    internal static class AtleastOneLogMessageRule
    {
        // This should be as unique as possible, and should follow the naming convention.
        private const string RuleId = "ST-USG-020";

        internal static Rule<IWorkflowModel> Get()
        {
            var rule = new Rule<IWorkflowModel>("At Least One Log Message Usage", RuleId, Inspect)
            {
                RecommendationMessage = "Using LogMessage activities is important both during development and after release. They help understanding the flow path which is very useful for maintenance and, when an error occurs, to easily identify the source and the cause.",//Recommendation,
                /// Off and Verbose are not supported.
                ErrorLevel = System.Diagnostics.TraceLevel.Warning,
                ApplicableScopes = new List<string> { RuleConstants.DevelopmentRuleConstant }
            };
            return rule;
        }

        // This is the function that executes for each activity in all the files. Might impact performance.
        // The rule instance is the rule provided above which also contains the user-configured data.
        private static InspectionResult Inspect(IWorkflowModel workflowModel, Rule ruleInstance)
        {
            var messageList = new List<string>();
            var rootActivity = workflowModel.Root;
            bool isLogmessagePresent = false;

            isLogmessagePresent = ContainLogMessage(rootActivity);            
            if (!isLogmessagePresent)
            {
                messageList.Add($"LogMessage not used in workflow {workflowModel.DisplayName}.");
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

        private static bool ContainLogMessage(IActivityModel activityModel)
        {
            bool isLogmessagePresent = false;
            if (activityModel.Children.Count == 0)
            {
                var ActivityType = activityModel.Type;

                var activityTypeData = ActivityType.Split(',');

                if (activityTypeData.Length > 0 && activityTypeData[0].Contains("."))
                {
                    var activityNameOnly = activityTypeData[0].Substring(activityTypeData[0].LastIndexOf('.') + 1);
                    
                    if (activityNameOnly.ToLower() == "logmessage")
                    {
                        isLogmessagePresent = true;
                    }
                }
            }
            else
            {
                var ActivityTypeTop = activityModel.Type;

                var activityTypeDataTop = ActivityTypeTop.Split(',');

                if (activityTypeDataTop.Length > 0 && activityTypeDataTop[0].Contains("."))
                {
                    var activityNameOnly = activityTypeDataTop[0].Substring(activityTypeDataTop[0].LastIndexOf('.') + 1);

                    if (activityNameOnly.ToLower() == "commentout")
                    {
                        isLogmessagePresent = false;
                    }
                    else
                    {
                        foreach (var activity in activityModel.Children)
                        {
                            isLogmessagePresent = ContainLogMessage(activity);
                            if (isLogmessagePresent)
                            {
                                break;
                            }
                        }
                      
                    }
                }
                //foreach (var activity in activityModel.Children)
                //{
                //    var ActivityType = activity.Type;

                //    var activityTypeData = ActivityType.Split(',');

                //    if (activityTypeData.Length > 0 && activityTypeData[0].Contains("."))
                //    {
                //        var activityNameOnly = activityTypeData[0].Substring(activityTypeData[0].LastIndexOf('.') + 1);
                        
                //        if (activityNameOnly.ToLower() == "logmessage")
                //        {
                //            isLogmessagePresent = true;
                //        }
                //    }
                //}
            }
            return isLogmessagePresent;
        }
    }
}
