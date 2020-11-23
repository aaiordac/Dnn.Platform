﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.EditBar.UI.Items
{
    using System;
    using System.Linq;

    using Dnn.EditBar.Library;
    using Dnn.EditBar.Library.Items;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Services.Personalization;
    using DotNetNuke.Web.Components.Controllers;

    [Serializable]
    public class QuickAddModuleMenu : BaseMenuItem
    {
        private const string LocalResourceFile = "~/DesktopModules/Admin/Dnn.EditBar/App_LocalResources/QuickAddModule.resx";

        public override string Name { get; } = "QuickAddModule";

        public override string Text
        {
            get
            {
                return DotNetNuke.Services.Localization.Localization.GetString("QuickAddModule", LocalResourceFile, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            }
        }

        public override string CssClass
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool CustomLayout => true;

        public override string Template
        {
            get
            {
                var portalSettings = PortalSettings.Current;
                if (portalSettings == null)
                {
                    return string.Empty;
                }

                var moduleList = ControlBarController.Instance.GetCategoryDesktopModules(portalSettings.PortalId, "Common", string.Empty).Select(m =>
                {
                    var selected = m.Value.FriendlyName == "HTML" ? " selected" : string.Empty;
                    return $"<option value=\"{m.Value.DesktopModuleID}\"{selected}>{m.Value.FriendlyName}</option>";
                });
                var panes = string.Empty;
                foreach (string paneName in portalSettings.ActiveTab.Panes)
                {
                    var selected = paneName == "ContentPane" ? " selected" : string.Empty;
                    panes += $"<option value=\"{paneName}\"{selected}>{paneName}</options>";
                }

                var toolTip = DotNetNuke.Services.Localization.Localization.GetString("QuickAddModule.Tooltip", LocalResourceFile, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                return $"<div><select id=\"menu-QuickAddModule-module\">{string.Join(string.Empty, moduleList)}</select><select id=\"menu-QuickAddModule-pane\">{panes}</select><button href=\"javascript: void(0);\">{this.Text}</button></div><div class=\"submenuEditBar\">{toolTip}</div>";
            }
        }

        public override string Parent { get; } = Constants.LeftMenu;

        public override string Loader { get; } = "QuickAddModule";

        public override int Order { get; } = 4;

        public override bool Visible()
        {
            var portalSettings = PortalSettings.Current;
            if (portalSettings == null)
            {
                return false;
            }

            if (!portalSettings.ShowQuickModuleAddMenu)
            {
                return false;
            }

            return Personalization.GetUserMode() == PortalSettings.Mode.Edit
                && ControlBarController.Instance.GetCategoryDesktopModules(portalSettings.PortalId, "All", string.Empty).Any();
        }
    }
}
