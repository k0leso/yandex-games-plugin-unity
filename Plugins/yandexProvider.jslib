mergeInto(LibraryManager.library, {
  InitPurchases: function() {
    initPayments();
  },
  
  SetToClipboard: function(text) {
	setToClipboard(text);
  },
  
  GetEnvironment: function() {
	getEnvironment();
  },
  
  GetGameData: function() {
	getGameData();
  },
  
  SetGameData: function(_data) {
	setGameData(_data);
  },

  Purchase: function(id) {
    buy(id);
  },

  AuthenticateUser: function() {
    auth();
  },

  GetUserData: function() {
    getUserData();
  },

  ShowFullscreenAd: function () {
    showFullscrenAd();
  },

  ShowRewardedAd: function(placement) {
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