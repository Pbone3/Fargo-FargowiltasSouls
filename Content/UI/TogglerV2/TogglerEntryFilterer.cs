using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace FargowiltasSouls.Content.UI.TogglerV2;

/// <summary>
/// A cleaned-up version of <see cref="Terraria.DataStructures.EntryFilterer{T,U}"/> with the redundant
/// <code>where T : new()</code> constraint removed.
/// </summary>
public class TogglerEntryFilterer<T, U>
    where U : IEntryFilter<T> {
    protected readonly List<U> AvailableFilters;
    protected readonly List<U> ActiveFilters;
    protected readonly List<U> AlwaysActiveFilters;
    protected ISearchFilter<T> _searchFilter;
    protected ISearchFilter<T> _searchFilterFromConstructor;

    public TogglerEntryFilterer() {
        AvailableFilters = new List<U>();
        ActiveFilters = new List<U>();
        AlwaysActiveFilters = new List<U>();
    }

    public void AddFilters(List<U> filters) {
        AvailableFilters.AddRange(filters);
    }

    public bool FitsFilter(T entry) {
        if (_searchFilter != null && !_searchFilter.FitsFilter(entry))
            return false;
        for (var index = 0; index < AlwaysActiveFilters.Count; ++index)
            if (!AlwaysActiveFilters[index].FitsFilter(entry))
                return false;

        if (ActiveFilters.Count == 0)
            return true;
        for (var index = 0; index < ActiveFilters.Count; ++index)
            if (ActiveFilters[index].FitsFilter(entry))
                return true;

        return false;
    }

    public void ToggleFilter(int filterIndex) {
        var availableFilter = AvailableFilters[filterIndex];
        if (ActiveFilters.Contains(availableFilter))
            ActiveFilters.Remove(availableFilter);
        else
            ActiveFilters.Add(availableFilter);
    }

    public bool IsFilterActive(int filterIndex) {
        return AvailableFilters.IndexInRange(filterIndex) &&
               ActiveFilters.Contains(AvailableFilters[filterIndex]);
    }

    public void SetSearchFilterObject<Z>(Z searchFilter) where Z : ISearchFilter<T>, U {
        _searchFilterFromConstructor = searchFilter;
    }

    public void SetSearchFilter(string searchFilter) {
        if (string.IsNullOrWhiteSpace(searchFilter)) {
            _searchFilter = null;
        }
        else {
            _searchFilter = _searchFilterFromConstructor;
            _searchFilter.SetSearch(searchFilter);
        }
    }

    public string GetDisplayName() {
        return Language.GetTextValueWith("BestiaryInfo.Filters", new {
            ActiveFilters.Count
        });
    }
}