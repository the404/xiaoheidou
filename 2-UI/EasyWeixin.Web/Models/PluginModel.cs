using System.ComponentModel;
using System.Web.Mvc;

namespace EasyWeixin.Web.Models
{
    public class PluginModel
    {
        public PluginModel()
        {
            //Locales = new List<PluginLocalizedModel>();
        }

        [DisplayName("Admin.Configuration.Plugins.Fields.Group")]
        [AllowHtml]
        public string Group { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
        [AllowHtml]
        public string FriendlyName { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.SystemName")]
        [AllowHtml]
        public string SystemName { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.Version")]
        [AllowHtml]
        public string Version { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.Author")]
        [AllowHtml]
        public string Author { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.Configure")]
        public string ConfigurationUrl { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.Installed")]
        public bool Installed { get; set; }

        public bool CanChangeEnabled { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.IsEnabled")]
        public bool IsEnabled { get; set; }

        //public IList<PluginLocalizedModel> Locales { get; set; }

        //Store mapping
        [DisplayName("Admin.Configuration.Plugins.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }

        [DisplayName("Admin.Configuration.Plugins.Fields.AvailableStores")]
        public int[] SelectedStoreIds { get; set; }
    }
}