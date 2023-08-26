using System.Collections.Generic;

namespace FargowiltasSouls.Content.UI.TogglerV2; 

public partial class TogglerUIV2 {
    public List<TogglerEntry> Entries = new();
    public TogglerEntryFilterer<TogglerEntry, ITogglerEntryFilter>? EntryFilterer;

    public void InitializeEntryFilterer() {
        EntryFilterer = new();
        //TODO add filters here
    }
}