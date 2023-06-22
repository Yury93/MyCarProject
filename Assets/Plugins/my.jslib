mergeInto(LibraryManager.library, {

  Hello: function () {
    console.log("Hello, world!"); //вызывается всплывающее сообщение
    console.log("Hello world");
  },

   GiveMeUserInfo: function () {

 myGameInstance.SendMessage('Yandex', 'SetName', player.getName());

    myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));

    window.alert(player.getName()); 
    console.log(player.getPhoto("medium"));
  
  },

     ShowAdv: function () {
    ysdk.adv.showFullscreenAdv({
    callbacks: {
        onOpen: function() {
          // some action after close
          console.log("-----open ADV-js-function----")
       
          myGameInstance.SendMessage('Yandex', 'OnOpenAdvFullscreen');
        },

        onClose: function() {
          // some action after close
          console.log("-----close ADV-js-function----")
         
          myGameInstance.SendMessage('Yandex', 'OnCloseAdvFullscreen');
        },
        onError: function(error) {
          // some action on error

        }
    }
})
  },

  AdvByRewards: function () {
   ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          console.log('Video ad open-js-function--.');
           myGameInstance.SendMessage('Yandex','StartVideoAdvReward');
        },
        onRewarded: () => {
          console.log('Rewarded!-js-function--');
          myGameInstance.SendMessage('Yandex','AddReward');
        },
        onClose: () => {
          myGameInstance.SendMessage('Yandex','CloseAdvReward');

        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
})
  },

});