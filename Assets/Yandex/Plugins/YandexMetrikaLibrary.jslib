mergeInto(LibraryManager.library, {
    YM_hit: function(url) {
        ym(ymKey, 'hit', UTF8ToString(url), {});
    },
  
    YM_reachGoal: function(target) {
        ym(ymKey, 'reachGoal', UTF8ToString(target));
    }
});