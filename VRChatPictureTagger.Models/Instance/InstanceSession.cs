// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.Models.Instance
{
    public class InstanceSession
    {
        public string World { get; set; }
        public string Session { get; set; }
        public DateTime Join { get; set; }
        public DateTime Leave { get; set; }

        public List<PlayerSession> Sessions { get; set; }
    }
}
