using RussellLib.Assets;
using System;
using System.Collections.Generic;

namespace RussellLib.Base
{
    public class ResourceTreeItem
    {
        // WARNING this implementation of a resource tree is pretty horrible.
        // but it's enough to make it work in WinForms :p

        public ResourceStatus Status;
        public Type ResType;
        public int Index;
        public string Name;
        public List<ResourceTreeItem> Resources;

        public enum ResourceStatus
        {
            UNKNOWN,
            PRIMARY,
            GROUP,
            SECONDARY
        }

        public static Type[] RESOURCE_KIND =
        {
            null, typeof(GMObject), typeof(GMSprite), typeof(GMSound),
            typeof(GMRoom),null,typeof(GMBackground),typeof(GMScript),typeof(GMPath),typeof(GMFont),typeof(GMGameInformation),
            typeof(GMOptions),typeof(GMTimeline), typeof(string) // extension pkgs, since making an additional class like GMExtensionPackage isn't the "nik-way"
        };

        public ResourceTreeItem(ProjectReader reader)
        {
            Status = (ResourceStatus)reader.ReadInt32();
            ResType = RESOURCE_KIND[reader.ReadInt32()];
            Index = reader.ReadInt32();
            Name = reader.ReadString();
            int content = reader.ReadInt32();
            Resources = new List<ResourceTreeItem>(content);
            for (int i = 0; i < content; i++)
            {
                Resources.Add(new ResourceTreeItem(reader));
            }
        }
    }
}
