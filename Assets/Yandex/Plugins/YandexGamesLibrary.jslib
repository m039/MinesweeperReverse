mergeInto(LibraryManager.library, {
    YG_getLang: function() {
        var lang = ysdk.environment.i18n.lang;
        var bufferSize = lengthBytesUTF8(lang) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(lang, buffer, bufferSize);
        return buffer;
    },
    
    YG_showFullscreenAdv: function() {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
                    unityInstance.SendMessage('YandexGamesManager', 'OnShowFullscreenAdvClose', new Boolean(wasShown).toString());
                },
                onError: function(error) {
                }
            }
        });
    },

    YG_setLeaderboardScore: function(leaderboard, number) {
        var name = UTF8ToString(leaderboard);

        ysdk.getLeaderboards().then(lb => {
            lb.setLeaderboardScore(name, number);
        });
    },

    YG_showRewardedVideo: function() {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onClose: function(wasShown) {
                    unityInstance.SendMessage('YandexGamesManager', 'OnShowRewardedVideoClose', new Boolean(wasShown).toString());
                },
                onRewarded: function() {
                    unityInstance.SendMessage('YandexGamesManager', 'OnShowRewardedVideoRewarded');
                },
                onError: function(error) {
                }
            }
        });
    },

    YG_setData: function(data) {
        var dataString = UTF8ToString(data);
        var myobj = JSON.parse(dataString);
        player.setData(myobj);
    },

    YG_getData: function() {
        player.getData().then(_data => {
            const myJSON = JSON.stringify(_data);
            unityInstance.SendMessage('YandexGamesManager', 'OnGetData', myJSON);
        });
    },
  });