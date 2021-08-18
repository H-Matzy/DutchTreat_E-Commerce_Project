$(document).ready(function () {
    console.log("Skrrt");

    var theForm = $("#theForm");
    theForm.hide();


    var buyButton = $("#buyButton");
    buyButton.on("click", function () {
        console.log("Buying Item");
    });

    var productInfo = $(".productProps li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text())
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popupForm");

    $loginToggle.on("click", function () {
        $popupForm.slideToggle(1000);
    })

});