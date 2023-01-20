function AJAXPost(url, data) {
    $.ajax({
        url: url,
        dataType: "json",
        type: "Post",
        contentType: 'application/json',
        data: data,
        async: true,
        processData: false,
        cache: false,
        beforeSend: function () {
            console.log('continue');
        },
        success: function () {
            console.log('success');
        },
        error: function () {
            console.log('error');
        },
    })
}
function AJAXGet(url, onSuccess, onError) {
    $.ajax({
        url: url,
        dataType: "json",
        type: "Get",
        contentType: 'application/json; charset=utf-8',
        async: true,
        processData: false,
        cache: false,
        beforeSend: function () {
            //alert('modal is called');
            console.log('continue');
            $('#TheLoadingOuterContainer').show();

        },
        success: onSuccess,
        error: OnError
    })
}
function OnError(p1, p2, p3) {
    alert(p1);
    alert(p2);
    alert(p3);
    $('#TheLoadingOuterContainer').hide();

}
