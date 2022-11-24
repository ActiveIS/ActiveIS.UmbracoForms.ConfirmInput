using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Data.Storage;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace ActiveIS.UmbracoForms.ConfirmInput.Fields
{
    public class ConfirmInput : FieldType
    {
        public ConfirmInput()
        {
            Id = new Guid("d32fd0d0-d342-4025-8b1d-47977637740d");
            Name = "Confirm Input";
            Description = string.Empty;
            Icon = "icon-checkbox";
            DataType = FieldDataType.String;
            SortOrder = 10;
            SupportsRegex = true;
            FieldTypeViewName = "FieldType.ConfirmInput.cshtml";
        }

        public override string GetDesignView()
        {
            return $"{Consts.PluginViewRoot}/Backoffice/ConfirmInput.html";
        }

        [Setting("Validation error message", Description = "The message shown when validation fails", View = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string ValidationErrorMessage { get; set; }

        [Setting("Placeholder", Description = "Enter a HTML5 placeholder value", View = "TextField")]
        public string Placeholder { get; set; }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContextBase context,
            IFormStorage formStorage, IFieldTypeStorage fieldTypeStorage, List<string> errors)
        {
            var fieldValues = postedValues.ToList();

            if (!fieldValues[0].ToString().Equals(fieldValues[1].ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(ValidationErrorMessage) || string.IsNullOrWhiteSpace(ValidationErrorMessage))
                {
                    ValidationErrorMessage = "The values entered do not match";
                }

                errors.Add(ValidationErrorMessage);
            }

            return base.ValidateField(form, field, postedValues.Take(1), context, formStorage, fieldTypeStorage, errors);
        }

        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();
            javascriptFiles.Add($"{Consts.PluginScriptRoot}/validate.confirminput.js");

            return javascriptFiles;
        }

        public override IEnumerable<object> ProcessSubmittedValue(Field field, IEnumerable<object> postedValues, HttpContextBase context)
        {
            var fieldValues = postedValues.ToList();
            if (field != null)
            {
                var updatedField = field;
                if (fieldValues.Any())
                {
                    if (fieldValues.Count() > 1)
                    {
                        fieldValues.RemoveAt(1);
                    }

                    updatedField.Values = fieldValues;
                    return base.ProcessSubmittedValue(updatedField, fieldValues, context);
                }
            }
            return base.ProcessSubmittedValue(field, fieldValues, context);
        }
    }
}
