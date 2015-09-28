using Apworks.Snapshots;
using System;

namespace EasyWeixin.Model.Snapshots
{
    public class PermissionSnapshot : Snapshot
    {
        public int PermissionId { get; set; }

        public Guid ID { get; set; }

        public string PermissionName { get; set; }

        public string PermissionDesc { get; set; }

        public string PermissionImageName { get; set; }

        public DateTime? AdminTime { get; set; }

        public string PermissionActionName { get; set; }

        public string PermissionUrl { get; set; }

        public int? IsOrder { get; set; }

        public int? IsMenu { get; set; }

        public string PermissionChineseName { get; set; }
    }
}