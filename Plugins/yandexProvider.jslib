mergeInto(LibraryManager.library, {
  Internal_InitPurchases: function() {
    initPayments();
  },
  
  Internal_SetToClipboard: function(text) {
	setToClipboard(UTF8ToString(text));
  },
  
  Internal_GetEnvironment: function() {
	getEnvironment();
  },
  
  Internal_GetGameData: function() {
	getGameData();
  },
  
  Internal_GetUniqueID: function() {
	getUniqueID();
  },
  
  Internal_SetGameData: function(text) {
	setGameData(UTF8ToString(text));
  },

  Internal_Purchase: function(id) {
    buy(UTF8ToString(id));
  },

  Internal_AuthenticateUser: function() {
    auth();
  },

  Internal_GetUserData: function() {
    getUserData();
  },

  Internal_ShowFullscreenAd: function () {
    showFullscrenAd();
  },

  Internal_ShowRewardedAd: function(placement) {
    showRewardedAd(placement);
    return placement;
  },

  Internal_OpenWindow: function(link){
    var url = UTF8ToString(link);
      document.onmouseup = function()
      {
        window.open(url);
        document.onmouseup = null;
      }
  }
});