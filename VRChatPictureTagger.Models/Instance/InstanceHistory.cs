// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.Models.Instance
{
    public class InstanceHistory
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<InstanceSession> SessionHistoray { get; set; }


    }
}
