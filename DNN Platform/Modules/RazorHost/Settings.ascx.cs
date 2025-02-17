﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Modules.RazorHost
{
    using System;
    using System.IO;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;

    /// <summary>Implements the Settings view logic.</summary>
    [Obsolete("Deprecated in 9.3.2, will be removed in 11.0.0, use Razor Pages instead")]
    public partial class Settings : ModuleSettingsBase
    {
        private string razorScriptFolder = "~/DesktopModules/RazorModules/RazorHost/Scripts/";

        /// <inheritdoc/>
        [Obsolete("Deprecated in 9.3.2, will be removed in 11.0.0, use Razor Pages instead")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void LoadSettings()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            string basePath = this.Server.MapPath(this.razorScriptFolder);
            var scriptFileSetting = this.Settings["ScriptFile"] as string;

            foreach (string script in Directory.GetFiles(this.Server.MapPath(this.razorScriptFolder), "*.??html"))
            {
                string scriptPath = script.Replace(basePath, string.Empty);
                var item = new ListItem(scriptPath, scriptPath);
                if (!string.IsNullOrEmpty(scriptFileSetting) && scriptPath.ToLowerInvariant() == scriptFileSetting.ToLowerInvariant())
                {
                    item.Selected = true;
                }

                this.scriptList.Items.Add(item);
            }

            base.LoadSettings();
        }

        /// <inheritdoc/>
        [Obsolete("Deprecated in 9.3.2, will be removed in 11.0.0, use Razor Pages instead")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void UpdateSettings()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            ModuleController.Instance.UpdateModuleSetting(this.ModuleId, "ScriptFile", this.scriptList.SelectedValue);
        }
    }
}
