$(() => {
    imageId = $("#image-id").val();

    $("#like-button").on("click", function () {
        $.post("/home/likeimage", { imageId });
        $("#like-button").attr('disabled', true);
    });

    setInterval(() => {
        $.get("/home/getlikes", { imageId }, function (likes) {
            $("#likes-count").text(likes);
            console.log(likes);
        });
    }, 1000);
});