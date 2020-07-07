using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Data.Storage;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;

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

        [Setting("Initial input field alias", Description = "The alias for the initial field to be compared against", View = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string InitialInputFieldAlias { get; set; }

        [Setting("Validation error message", Description = "The message shown when validation fails", View = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string ValidationErrorMessage { get; set; }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContextBase context,
            IFormStorage formStorage)
        {
            var errors = new List<string>();
            var fieldValues = postedValues.ToList();
            if (fieldValues.Any())
            {
                foreach (var fieldValue in fieldValues)
                {
                    var initialFieldValues = form.AllFields.FirstOrDefault(x => x.Alias == InitialInputFieldAlias)?.Values;
                    if (initialFieldValues != null && initialFieldValues.Any())
                    {
                        foreach (var initialInputFieldValue in initialFieldValues)
                        {
                            if (fieldValue != initialInputFieldValue)
                            {
                                errors.Add(ValidationErrorMessage);
                            }
                        }
                    }
                }

            }

            if (errors.Any())
            {
                return errors;
            }

            return base.ValidateField(form, field, fieldValues, context, formStorage);
        }
    }
}
