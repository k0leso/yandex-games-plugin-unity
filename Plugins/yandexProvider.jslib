mergeInto(LibraryManager.library, {
  Internal_InitPurchases: function() {
    initPayments();
  },
  
  Internal_SetToClipboard: function(text) {
	setToClipboard(text);
  },
  
  Internal_GetEnvironment: function() {
	getEnvironment();
  },
  
  Internal_GetGameData: function() {
	getGameData();
  },
  
  Internal_SetGameData: function(_data) {
	setGameData(_data);
  },

  Internal_Purchase: function(id) {
    buy(id);
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

  OpenWindow: function(link){
    var url = Pointer_stringify(link);
      document.onmouseup = function()
      {
        window.open(url);
        document.onmouseup = null;
      }
  }
});