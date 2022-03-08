{{buildDetails.definition.name }} {{buildDetails.buildNumber}} {{get_parameter buildDetails.parameters 'publishOptions'}} ({{moment buildDetails.startTime 'DD/MM/YYYY' }}) {{#forEach commits}}

{{get_only_message_firstline this.message}} {{/forEach}}
{{#forEach workItems}} {{#if isFirst}}Tickets résolus {{/if}}

{{lookup this.fields 'System.WorkItemType'}} {{lookup this.fields 'System.Title'}} {{/forEach}}